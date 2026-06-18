import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {Observable} from "rxjs";

/* Support enums and interfaces */
export enum EquipmentStatus {
  available = 'Available',
  borrowed = 'Borrowed',
  maintained = 'Maintained'
}

enum LoanStatus {
  onLoan = 'on loan',
  returned = 'returned'
}

enum EquipmentSortBy {
  none,
  name,
  category,
  status
}

/* User interfaces */
export interface UserDto {
  id: string;
  fullName: string;
  email: string;
}

export interface UserLoginDto {
  email: string;
  password: string;
}

/* Equipment interfaces */

export interface EquipmentDto {
  id: string;
  name: string;
  description?: string;
  status: EquipmentStatus;
  serialNumber?: string;
  createdAt: string;
  modifiedAt: string;
  currentLoan?: loanDto;
  category: CategoryDto;
}

export interface EquipmentCreateDto {
  name: string;
  status: EquipmentStatus;
  categoryName: string;
  description?: string;
  serialNumber?: string;
}

export interface EquipmentUpdateDto {
  name?: string;
  description?: string;
  categoryName?: string;
  serialNumber?: string;
}

export interface EquipmentQueryDto {
  name?: string;
  status?: EquipmentStatus;
  category?: string;
  serialNumber?: string;
  borrower?: string;
  sortBy?: EquipmentSortBy;
  ascending?: boolean;
  pageNumber?: number;
  pageSize?: number;
}

export interface EquipmentQueriedDto {
  equipments: EquipmentDto[];
  pageNumber: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
}

/* loan interfaces */

export interface loanDto {
  id: string;
  preformedBy: UserDto;
  equipment: EquipmentDto;
  loanDate: string;
  dueDate: string;
  returnDate?: string;
  status: LoanStatus;
}

export interface loanCreateDto {
  borrowerId: string;
  preformedById: string;
  equipmentId: string;
  status: LoanStatus;
  loanDate: string;
  dueDate: string;
}

/* Borrower interfaces */

export interface BorrowerDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  createdAt: string;
  modifiedAt: string;
}

export interface BorrowerCreateDto {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
}

/* Category interfaces */
export interface CategoryDto {
  id: string;
  name: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) {
  }

  me(): Observable<UserDto> {
    return this.http.get<UserDto>('/api/v1/user/me');
  }

  login(user: UserLoginDto): Observable<UserDto> {
    return this.http.post<UserDto>('/api/v1/user/login', user);
  }
  logout(): Observable<void> {
    return this.http.post<void>('/api/v1/user/logout', {});
  }

  createBorrower(borrower: BorrowerCreateDto): Observable<BorrowerDto> {
    return this.http.post<BorrowerDto>('/api/v1/user/create/borrower', borrower);
  }

  getEquipments(query: EquipmentQueryDto): Observable<EquipmentQueriedDto> {
    return this.http.get<EquipmentQueriedDto>('/api/v1/equipment', {params: query as any});
  }

  getCategories(): Observable<CategoryDto[]> {
    return this.http.get<CategoryDto[]>('/api/v1/equipment/categories');
  }

  getEquipment(equipmentId: string): Observable<EquipmentDto> {
    return this.http.get<EquipmentDto>(`/api/v1/equipment/${equipmentId}`);
  }

  createEquipment(equipment: EquipmentCreateDto): Observable<EquipmentDto> {
    return this.http.post<EquipmentDto>('/api/v1/equipment', equipment);
  }

  createCategory(name: string): Observable<CategoryDto> {
    return this.http.post<CategoryDto>('/api/v1/equipment/category', {name});
  }

  updateTask(equipmentId: string, equipment: EquipmentUpdateDto): Observable<EquipmentDto> {
    return this.http.put<EquipmentDto>(`/api/v1/equipment/update/${equipmentId}`, equipment);
  }

  deleteTask(equipmentId: string): Observable<void> {
    return this.http.delete<void>(`/api/v1/equipment/delete/${equipmentId}`);
  }

  getLoans(loanId: string): Observable<loanDto[]> {
    return this.http.get<loanDto[]>(`/api/v1/loans/equipment/${loanId}/loans`);
  }

  createLoan(loan: loanCreateDto): Observable<loanDto> {
    return this.http.post<loanDto>('/api/v1/loans', loan);
  }

  returnLoan(loanId: string): Observable<loanDto> {
    return this.http.post<loanDto>(`/api/v1/loans/${loanId}/return`, {});
  }
}
