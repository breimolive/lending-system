import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import {Observable} from "rxjs";

interface UserDto {
  id: string;
  fullName: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) { }

  me(): Observable<UserDto> {
    return this.http.get<UserDto>('/api/v1/user/me');
  }

  login(email: string, password: string): Observable<UserDto> {
    return this.http.post<UserDto>('/api/v1/user/login', {
      email,
      password
    });
  }

  register(email: string, password: string, fullName: string): Observable<UserDto> {
    return this.http.post<UserDto>('/api/v1/user/register', {
      email,
      password,
      fullName
    });
  }

}
