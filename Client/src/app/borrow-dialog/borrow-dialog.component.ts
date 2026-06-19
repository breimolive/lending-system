import {
  ChangeDetectorRef,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  OnDestroy,
  OnInit,
  Output,
  ViewChild
} from '@angular/core';
import {
  ApiService,
  BorrowerCreateDto,
  BorrowerDto,
  EquipmentDto,
  loanCreateDto, UserDto
} from "../api.service";
import {FormBuilder, FormGroup, ReactiveFormsModule, Validators} from "@angular/forms";
import {catchError, debounceTime, distinctUntilChanged, filter, of, Subject, switchMap, takeUntil} from "rxjs";

@Component({
  selector: 'app-borrow-dialog',
  standalone: true,
  templateUrl: './borrow-dialog.component.html',
  imports: [
    ReactiveFormsModule
  ],
  styleUrls: ['./borrow-dialog.component.css', "../dialog.css"]
})
export class BorrowDialogComponent implements OnInit, OnDestroy {
  EmailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  @ViewChild('dialog') private dialogRef!: ElementRef<HTMLDialogElement>;
  @Output() refresh = new EventEmitter<boolean>();
  @Input() equipment: EquipmentDto | null = null;
  user: UserDto | null = null;
  borrowerForm: FormGroup | null = null;
  loanForm: FormGroup | null = null;
  isDisabled = false;
  isOpen = false;
  borrowerFound = false;
  dueDate: string | null = null;
  private destroy$ = new Subject<void>();
  loanPayload: loanCreateDto | null = null;

  constructor(
    private cdr: ChangeDetectorRef,
    private api: ApiService,
    private formBuilder: FormBuilder) {
  }

  ngOnInit(): void {
    this.api.me().subscribe({
      next: (user) => {
        this.user = user;
      },
      error: (err) => {
        console.error('Error fetching user:', err);
      }
    })
    this.borrowerForm = this.formBuilder.group({
      firstName: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(100)]
      ],
      lastName: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(100)]
      ],
      email: ['', [
        Validators.required,
        Validators.pattern(this.EmailRegex),
        Validators.minLength(3),
        Validators.maxLength(200)]
      ],
      phoneNumber: ['', [
        Validators.maxLength(8),
        Validators.minLength(8)]
      ],
    })

    this.loanForm = this.formBuilder.group({
      dueDate: ['', Validators.required],
      equipmentId: ['', Validators.required],
      borrowerId: ['', Validators.required]
    })

    /* Used AI (Claude Opus 4.6 - Antigravity) to implement the following logic:

    Listen for changes in the email field to check for existing borrower

      - Debounce input to avoid excessive API calls
      - Only proceed if the email is valid
      - If a borrower is found, populate the form and disable name/phone fields
      - If no borrower is found, enable name/phone fields for new entry

    Reason: I wanted to implement a user-friendly feature that allows staff to quickly check if a borrower
    already exists in the system by simply entering their email. This helps prevent duplicate entries and
    speeds up the loan creation process for returning borrowers.

    I did make a simular code to this but I wasnt as fast nor did it include the debounce and distinctUntilChanged
    operators which are crucial for performance and user experience.

    Nothing that's too advanced here but I did have to read into switchMap since I'm new to using it.
    https://rxjs.dev/api/operators/switchMap
    */
    this.borrowerForm.get('email')?.valueChanges.pipe(
      takeUntil(this.destroy$),
      debounceTime(500),
      distinctUntilChanged(),
      filter((email: string) => this.EmailRegex.test(email)),
      switchMap((email: string) =>
        this.api.getBorrower(email).pipe(
          catchError(() => of(null))
        )
      )
    ).subscribe((borrower: BorrowerDto | null) => {
      if (borrower) {
        this.borrowerFound = true;
        this.borrowerForm?.patchValue({
          firstName: borrower.firstName,
          lastName: borrower.lastName,
          phoneNumber: borrower.phoneNumber
        });
        this.borrowerForm?.get('firstName')?.disable();
        this.borrowerForm?.get('lastName')?.disable();
        this.borrowerForm?.get('phoneNumber')?.disable();
        this.loanForm?.get('borrowerId')?.setValue(borrower.id);
      } else {
        this.borrowerFound = false;
        this.borrowerForm?.get('firstName')?.enable();
        this.borrowerForm?.get('lastName')?.enable();
        this.borrowerForm?.get('phoneNumber')?.enable();
      }
    });
  }

  openDialog(): void {
    this.isOpen = true;

    this.cdr.detectChanges();

    const dlg = this.dialogRef?.nativeElement;
    if (!dlg) {
      return;
    }
    if (!dlg.open) {
      dlg.showModal();
    }
  }

  closeDialog(): void {
    const dlg = this.dialogRef?.nativeElement;
    if (dlg && dlg.open
    ) {
      dlg.close();
    }
    this.isOpen = false;
    this.refresh.emit(true);
  }

  onDialogClick(event: MouseEvent) {
    if ((event.target as HTMLDialogElement).tagName === 'DIALOG') {
      this.closeDialog();
    }
  }

  onSubmit() {
    if (!this.borrowerForm || !this.loanForm || !this.equipment) {
      return;
    }

    if (this.borrowerFound && this.loanForm.valid && this.user) {
      this.loanValidationMethod();
      const loanPayload = {
        borrowerId: this.loanForm.get('borrowerId')?.value,
        equipmentId: this.equipment.id,
        preformedById: this.user.id,
        dueDate: this.dueDate
      };
      this.api.createLoan(loanPayload as loanCreateDto).subscribe({
        next: () => {
          this.closeDialog();
        },
        error: (err) => {
          console.error('Error creating loan:', err);
        }
      });
    } else {
      this.borrowerValidationMethod();
      const payload = {
        firstName: this.borrowerForm.getRawValue().firstName,
        lastName: this.borrowerForm.getRawValue().lastName,
        email: this.borrowerForm.get('email')?.value || null,
        phoneNumber: this.borrowerForm.getRawValue().phoneNumber || null
      };
      this.api.createBorrower(payload as BorrowerCreateDto).subscribe({
        next: () => {
          if (this.user && this.equipment) {
            this.api.getBorrower(payload.email).subscribe({
              next: (borrower) => {
                this.loanForm!.get('borrowerId')?.setValue(borrower.id);
                this.loanValidationMethod();

                if (this.user && this.equipment) {
                this.loanPayload = {
                  dueDate: this.dueDate,
                  equipmentId: this.equipment!.id,
                  borrowerId: borrower.id,
                  preformedById: this.user!.id
                } as loanCreateDto;
                }
                this.api.createLoan(this.loanPayload as loanCreateDto).subscribe({
                  next: () => {
                    this.closeDialog();
                  },
                  error: (err) => {
                    console.error('Error creating loan:', err);
                  }
                });
              },
              error: (err) => {
                console.error('Error fetching borrower:', err);
              }
            });
          }
        },
        error: (err) => {
          console.error('Error creating borrower:', err);
        }
      });
    }
  }

  borrowerValidationMethod() {
    if (this.borrowerForm) {
      if (this.borrowerForm.invalid ||
        !this.borrowerForm.get('firstName')?.value ||
        !this.borrowerForm.get('LastName')?.value ||
        !this.borrowerForm.get('email')?.value ||
        !this.borrowerForm.get('phoneNumber')?.value
      ) {
        this.borrowerForm.markAllAsTouched();
        return;
      }
    }
  }

  loanValidationMethod() {
    if (this.borrowerForm) {
      if (this.borrowerForm.invalid ||
        !this.borrowerForm.get('dueDate')?.value
      ) {
        this.borrowerForm.markAllAsTouched();
        return;
      }
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
