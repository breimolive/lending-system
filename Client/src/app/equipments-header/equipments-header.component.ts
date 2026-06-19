import {Component, ElementRef, EventEmitter, OnInit, Output, ViewChild} from '@angular/core';
import {ApiService, CategoryDto, EquipmentQueryDto, EquipmentStatus} from "../api.service";
import {debounceTime, Subject} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-equipments-header',
  standalone: true,
  templateUrl: './equipments-header.component.html',
  imports: [
    FormsModule
  ],
  styleUrl: './equipments-header.component.css'
})
export class EquipmentsHeaderComponent implements OnInit {
  @Output() requestSender = new EventEmitter<EquipmentQueryDto>();
  private borrowerSubject = new Subject<string>();
  private nameSubject = new Subject<string>();
  sortByOptions = ['Name', 'Category', 'Status'];
  selectedCategory: string | null = 'None';
  selectedStatus: string | null = 'None';
  Categories: CategoryDto[] | null = null;
  ariaStatus: string | null = null;
  request: EquipmentQueryDto = {};
  sortLabel: string | null = null;
  categorySortAsc = true;
  sortedBy: string | null = null;
  statusSortAsc = true;
  nameSortAsc = true;

  constructor(private api: ApiService) {}
  ngOnInit(): void {
    this.api.getCategories().subscribe({
      next: (response) => {
        this.Categories = response;
      },
      error: (_) => {
        alert('Failed to load categories');
      }
    });

    this.nameSubject
      .pipe(debounceTime(1000))
      .subscribe(value => {
        this.request = {
          ...this.request,
          name: value
        };

        this.requestSender.emit(this.request);
      });

    this.borrowerSubject
      .pipe(debounceTime(1000))
      .subscribe(value => {
        this.request = {
          ...this.request,
          borrower: value
        };
        this.requestSender.emit(this.request);
      });
  }

  sortBy(type: string | null) {
    if (type) {
      this.sortLabel = type
      if (type === "Name") {
        this.nameSortAsc = !this.nameSortAsc;
        this.request.ascending = !this.nameSortAsc;
        this.sortedBy = type;
        this.request.sortBy = 1;
      } else if (type === "Category") {
        this.categorySortAsc = !this.categorySortAsc;
        this.request.ascending = !this.categorySortAsc;
        this.sortedBy = type;
        this.request.sortBy = 2;
      } else if (type === "Status") {
        this.statusSortAsc = !this.statusSortAsc;
        this.request.ascending = !this.statusSortAsc;
        this.sortedBy = type;
        this.request.sortBy = 3;
      }
      this.sortFilter(this.request);
    }
  }

  Filter(key: string, value: string) {
    const trimmedValue = value ? value.trim() : '';
    switch (key) {
      case 'status':
        if (trimmedValue === 'None') {
          const {status, ...rest} = this.request;
          this.request = rest;
        } else {
          this.request = {
            ...this.request,
            status: trimmedValue as EquipmentStatus
          };
        }
        this.requestSender.emit(this.request);
        break;
      case 'category':
        if (trimmedValue === 'None') {
          const {category, ...rest} = this.request;
          this.request = rest;
        } else {
          this.request = {
            ...this.request,
            category: trimmedValue
          };
        }
        this.requestSender.emit(this.request);
        break;
      case 'name':
        this.nameSubject.next(value);
        break;
      case 'borrower':
        this.borrowerSubject.next(value);
        break;
    }
  }

  sortFilter(request: EquipmentQueryDto): void {
    // emit a shallow copy to avoid mutations affecting debounce/distinct checks
    this.requestSender.emit({...request});
  }

  announce(message: string) {
    this.ariaStatus = message;
    if (message) {
      setTimeout(() => this.ariaStatus = '', 700);
    }
  }

  onSortSelect(value: string) {
    this.sortedBy = value || null;
  }

}
