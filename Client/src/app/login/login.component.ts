import {Component} from '@angular/core';
import {FormBuilder} from "@angular/forms";
import {ApiService} from "../api.service";
import {Router} from "@angular/router";

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrl: './login.component.css',
    standalone: false
})
export class LoginComponent {

  constructor(
    private formBuilder: FormBuilder,
    private api: ApiService,
    private router: Router
  ) {
  }

  loginForm = this.formBuilder.group({
    email: '',
    password: ''
  });

  onSubmit(): void {
    if (this.loginForm.invalid || !this.loginForm.value.email || !this.loginForm.value.password) {
      return;
    }

    this.api
      .login(this.loginForm.value.email, this.loginForm.value.password)
      .subscribe({
        next: (_) => this.router.navigate(['']),
        error: (error) => alert('Login failed')
      })
  }
}
