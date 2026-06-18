import {AfterViewInit, Component} from '@angular/core';
import {ApiService, EquipmentQueriedDto, EquipmentQueryDto} from "../api.service";
import {EquipmentsHeaderComponent} from "../equipments-header/equipments-header.component";
import {NgClass} from "@angular/common";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-equipments',
  standalone: true,
  templateUrl: './equipments.component.html',
  imports: [
    EquipmentsHeaderComponent,
    NgClass,
    RouterLink
  ],
  styleUrls: ['./equipments.component.css']
})
export class EquipmentsComponent implements AfterViewInit {
  response: EquipmentQueriedDto | null = null;
  ariaStatus: string | null = null;
  request: EquipmentQueryDto = {};
  start: number | null = null;
  end: number | null = null;
  currentPage = 1;

  constructor(private api: ApiService) {
  }
  ngAfterViewInit(): void {
    this.reFresh();
  }

  reFresh(): void {
    this.api.getEquipments(this.request).subscribe({
      next: (response) => {
        this.response = response;
      },
      error: (_) => {
        alert('Failed to load equipments');
      }
    });
  }

  announce(message: string) {
    this.ariaStatus = message;
    if (message) {
      setTimeout(() => this.ariaStatus = '', 700);
    }
  }

  onRequestChange(newRequest: EquipmentQueryDto) {
    this.request = newRequest;
    this.reFresh();
  }

  fetchPage(page: number): void {
    if (this.response) {
      this.request.pageNumber = page;
      this.api.getEquipments(this.request).subscribe({
        next: (response) => {
          this.response = response;
          this.currentPage = page;
          this.start = (response.pageNumber - 1) * response.pageSize + 1;
          this.end = Math.min(response.pageNumber * response.pageSize, response.totalItems);
        },
        error: (_) => {
          alert('Failed to load inventory');
        }
      });
    }
  }

  protected readonly Math = Math;
}
