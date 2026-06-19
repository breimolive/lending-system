import {ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {ApiService, CategoryDto, EquipmentCreateDto, EquipmentDto, loanDto} from "../api.service";
import {FormBuilder, FormGroup} from "@angular/forms";

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
    private api: ApiService){
  }

  ngOnInit(): void {
      if (this.equipment && this.equipment.currentLoan) {
        this.loan = this.equipment.currentLoan;
        console.log(this.loan);
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
    if (this.equipment && this.equipment.currentLoan) {
      this.api.returnLoan(this.equipment.currentLoan.id).subscribe({
        next: () => {
          this.closeDialog();
        },
        error: (err) => {
          console.error("Failed to return equipment", err);
        }
      });
    }
  }
}
