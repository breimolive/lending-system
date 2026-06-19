import {ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, Output, ViewChild} from '@angular/core';
import {FormBuilder, FormGroup, ReactiveFormsModule} from "@angular/forms";
import {ApiService, CategoryDto, EquipmentDto} from "../api.service";

@Component({
  selector: 'app-delete-dialog',
  standalone: true,
  templateUrl: './delete-dialog.component.html',
  imports: [
    ReactiveFormsModule
  ],
  styleUrls: ['./delete-dialog.component.css', "../dialog.css"]
})
export class DeleteDialogComponent {
  @ViewChild('dialog') private dialogRef!: ElementRef<HTMLDialogElement>;
  @Output() refresh = new EventEmitter<boolean>();
  @Input() equipment: EquipmentDto | null = null;
  isOpen = false;

  constructor(
    private cdr: ChangeDetectorRef,
    private api: ApiService,
    private formBuilder: FormBuilder) {
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
    if (this.equipment) {
      this.api.deleteEquipment(this.equipment.id).subscribe({
          next: () => {
            this.closeDialog();
          },
          error: (err) => {
            console.error('Error deleting task:', err);
          }
        }
      )
    }
  }
}
