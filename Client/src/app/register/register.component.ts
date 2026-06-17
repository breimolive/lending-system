import { Component } from '@angular/core';
import {FormBuilder} from "@angular/forms";
import {ApiService} from "../api.service";
import {Router} from "@angular/router";

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrl: './register.component.css',
    standalone: false
})
export class RegisterComponent {

  constructor(
    private formBuilder: FormBuilder,
    private api: ApiService,
    private router: Router
  ) {}

  registerForm = this.formBuilder.group({
    fullname: '',
    email: '',
    password: ''
  });

  onSubmit(): void {
    if (this.registerForm.invalid || !this.registerForm.value.email || !this.registerForm.value.password || !this.registerForm.value.fullname) {
      return;
    }

    this.api
      .register(this.registerForm.value.email, this.registerForm.value.password, this.registerForm.value.fullname)
      .subscribe({
        next: (_) => this.router.navigate(['']),
        error: (error) => alert('Register failed')
      })
  }
}
