import {ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {ApiService, CategoryDto, EquipmentDto, loanDto} from "../api.service";

@Component({
  selector: 'app-return-dialog',
  standalone: true,
  templateUrl: './return-dialog.component.html',
  styleUrls: ['./return-dialog.component.css', '../dialog.css']
})
export class ReturnDialogComponent implements OnInit {
  @ViewChild('dialog') private dialogRef!: ElementRef<HTMLDialogElement>;
  @Output() refresh = new EventEmitter<boolean>();
  @Input() equipment: EquipmentDto | null = null;
  categories: CategoryDto[] | null = null;
  loan: loanDto | null = null;
  isDisabled = false;
  isOpen = false;

  constructor(
    private cdr: ChangeDetectorRef,
    private api: ApiService) {
  }

  ngOnInit(): void {
    this.LoadLoan();
  }

  LoadLoan(): void {
    if (!this.equipment) return;
    if (!this.equipment.currentLoan) {
      this.api.getEquipment(this.equipment.id).subscribe(equipment => {
        if (equipment.currentLoan) {
          this.loan = equipment.currentLoan;
          return;
        } else {
          console.error("No current loan found for equipment after refresh", equipment);
          return;
        }
      })
    } else {
      this.loan = this.equipment.currentLoan;
    }
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
    this.LoadLoan();
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
    if (!this.loan || !this.loan.id) {
      console.error('Cannot return loan: loan or loan.id is missing', this.loan);
      return;
    }

    this.api.returnLoan(this.loan.id).subscribe({
      next: () => {
        this.closeDialog();
      },
      error: (err) => {
        console.error("Failed to return equipment", err);
      }
    });
  }
}
