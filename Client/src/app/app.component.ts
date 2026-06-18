import { Component } from '@angular/core';
import {CurrentUserService} from "./currentUserService";
import {RouterOutlet} from "@angular/router";
import {NavbarComponent} from "./navbar/navbar.component";
import {AsyncPipe} from "@angular/common";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [
    RouterOutlet,
    NavbarComponent,
    AsyncPipe
  ],
  standalone: true
})
export class AppComponent {
  user$ = this.userService.currentUser$;
  constructor(private userService: CurrentUserService) {}
}
