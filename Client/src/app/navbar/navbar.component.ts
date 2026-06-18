import {Component, Input, OnInit} from '@angular/core';
import {ApiService, UserDto} from "../api.service";
import {CurrentUserService} from "../currentUserService";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-navbar',
  standalone: true,
  templateUrl: './navbar.component.html',
  imports: [
    RouterLink
  ],
  styleUrl: './navbar.component.css'
})
export class NavbarComponent implements OnInit {
  @Input({required:true}) user: UserDto | null = null;
  constructor(private api: ApiService, private currentUserService: CurrentUserService) {
  }

  ngOnInit() {
    this.currentUserService.getCurrentUser$().subscribe({
      next: data => {
        this.user = data;
      },
      error: _ => {
        this.api.me().subscribe({
          next: data => {
            this.currentUserService.setCurrentUser(data);
            this.user = data;
          }
        })
      }
    })
  }
  onLogout() {
    this.api.logout().subscribe({
      next: () => {
        window.location.href = '/login';
      },
      error: err => {
        console.error('Logout failed', err);
      }
    });
  }

}
