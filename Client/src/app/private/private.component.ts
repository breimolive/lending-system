import {Component, OnInit} from '@angular/core';
import {ApiService} from "../api.service";

@Component({
    selector: 'app-private',
    templateUrl: './private.component.html',
    styleUrl: './private.component.css',
    standalone: false
})
export class PrivateComponent implements OnInit {

  userFullName: string = '';

  constructor(
    private api: ApiService,
  ) {
  }

  ngOnInit(): void {
    this.api.me().subscribe({
      next: (response) => {
        this.userFullName = response.fullName;
      },
      error: (error) => {
        console.error('Error fetching user info:', error);
      }
    });
  }
}
