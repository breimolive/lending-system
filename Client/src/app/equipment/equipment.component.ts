import {Component, OnDestroy, OnInit} from '@angular/core';
import {ApiService, EquipmentDto, EquipmentStatus, loanDto, UserDto} from "../api.service";
import {ActivatedRoute, Router} from "@angular/router";
import {Subscription} from "rxjs";
import {DatePipe, NgClass} from "@angular/common";

@Component({
  selector: 'app-equipment',
  standalone: true,
  templateUrl: './equipment.component.html',
  imports: [
    NgClass,
    DatePipe
  ],
  styleUrl: './equipment.component.css'
})
export class EquipmentComponent implements OnInit, OnDestroy {
  user: UserDto | null = null;
  currentTaskValue: EquipmentDto | null = null;
  equipment: EquipmentDto | null = null;
  loans: loanDto[] | null = null;
  selectedStatus: string | null = null;
  subs = new Subscription();
  equipmentId: string | null = null;
  currentLoan: loanDto | null = null;

  constructor(
    private api: ApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    this.reFetchEquipment();

    this.api.me().subscribe({
      next: (data) => {
        this.user = data;
        console.log(this.user);
      },
      error: (err) => {
        console.error('Error fetching user:', err);
      }
    })
    /*Subscription solution to update the task details when navigating to the same task or when the task is
    updated in the TaskNavbarComponent. This ensures that the task details are always up to date
    without needing to refresh the page.

    Took inspiration from the following sources:

    Documentation:
    https://rxjs.dev/guide/subscription
    https://stackoverflow.com/questions/38008334/angular-rxjs-when-should-i-unsubscribe-from-subscription

    Plus: I've Implemented this in a previous practice exam.
    */

    this.subs.add(
      this.route.paramMap.subscribe(params => {
        this.equipmentId = params.get('id');
        if (this.equipmentId) {
          this.api.getEquipment(this.equipmentId).subscribe({
            next: (e) => {
              this.equipment = e;
              this.equipmentId = e.id
              this.fetchLoans();
            },
            error: (err) => {
              console.error('Error fetching task:', err);
              void this.router.navigate(['/']);
            }
          });
        }
      }) ?? new Subscription()
    );
  }

  reFetchEquipment() {
    if (this.equipmentId) {
      this.api.getEquipment(this.equipmentId)?.subscribe(e => {
        this.currentTaskValue = e;
        if (e) {
          this.selectedStatus = e.status;
        }
      });
    }
  }

  ngOnDestroy(): void {
    this.subs.unsubscribe();
  }

  fetchLoans() {
    console.log(this.equipmentId);
    if (this.equipmentId) {
      this.api.getLoans(this.equipmentId)?.subscribe(e => {
        this.loans = e;
        if (this.loans) {
          const activeLoans = this.loans.filter(loan => !loan.returnDate);
          this.currentLoan = activeLoans[0];
          activeLoans.splice(0);
        }
      });
    }
  }

  protected readonly EquipmentStatus = EquipmentStatus;
}
