import {ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {ApiService, CategoryDto, EquipmentCreateDto, EquipmentDto} from "../api.service";
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from "@angular/forms";

@Component({
  selector: 'app-create-dialog',
  standalone: true,
  templateUrl: './create-dialog.component.html',
  imports: [
    ReactiveFormsModule,
    FormsModule
  ],
  styleUrls: ['./create-dialog.component.css', "../dialog.css"]
})
export class CreateDialogComponent implements OnInit {
  @ViewChild('dialog') private dialogRef!: ElementRef<HTMLDialogElement>;
  @Output() refresh = new EventEmitter<boolean>();
  @Input() equipment: EquipmentDto | null = null;
  categories: CategoryDto[] | null = null;
  selectedStatus: string | null = null;
  form: FormGroup | null = null;
  isDisabled = false;
  isOpen = false;

  constructor(
    private cdr: ChangeDetectorRef,
    private api: ApiService,
    private formBuilder: FormBuilder) {
  }

  ngOnInit(): void {
    this.api.getCategories().subscribe(categories => {
      this.categories = categories;
    })
    this.form = this.formBuilder.group({
      name: ['', [
        Validators.required,
        Validators.minLength(3),
        Validators.maxLength(100)]
      ],
      categoryName: ['', Validators.required],
      serialNumber: ['', [
        Validators.maxLength(14),
        Validators.minLength(14)]
      ],
      description: ['', Validators.maxLength(500)
      ],
    })
  }

  openDialog(): void {
    console.log("test");
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
    if (this.form) {
      this.validationMethod();
      const payload = {
        name: this.form.get('name')?.value,
        categoryName: this.form.get('categoryName')?.value,
        serialNumber: this.form.get('serialNumber')?.value || null,
        description: this.form.get('description')?.value || null
      }
      this.api.createEquipment(payload as EquipmentCreateDto).subscribe({
        next: () => {
          this.closeDialog();
        },
        error: (err) => {
          console.error('Error creating task:', err);
        }
      });
    }
  }

  validationMethod() {
    if (this.form) {
      if (this.form.invalid ||
        !this.form.get('name')?.value ||
        !this.form.get('status')?.value ||
        !this.form.get('categoryName')?.value ||
        !this.form.get('description')?.value ||
        !this.form.get('serialNumber')?.value
      ) {
        this.form.markAllAsTouched();
        return;
      }
    }
  }
}
