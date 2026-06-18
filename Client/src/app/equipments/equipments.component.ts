import {AfterViewInit, Component} from '@angular/core';
import {ApiService, EquipmentQueriedDto, EquipmentQueryDto} from "../api.service";
import {EquipmentsHeaderComponent} from "../equipments-header/equipments-header.component";
import {NgClass} from "@angular/common";

@Component({
  selector: 'app-equipments',
  standalone: true,
  templateUrl: './equipments.component.html',
  imports: [
    EquipmentsHeaderComponent,
    NgClass
  ],
  styleUrls: ['./equipments.component.css']
})
export class EquipmentsComponent implements AfterViewInit {
  request: EquipmentQueryDto = {};
  response: EquipmentQueriedDto | null = null;

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

  onRequestChange(newRequest: EquipmentQueryDto) {
    this.request = newRequest;
    this.reFresh();
  }
}
