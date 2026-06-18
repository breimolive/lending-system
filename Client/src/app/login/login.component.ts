import {Component} from '@angular/core';
import {AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators} from "@angular/forms";
import {ApiService, UserLoginDto} from "../api.service";
import {Router} from "@angular/router";
import {CurrentUserService} from "../currentUserService";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  imports: [
    ReactiveFormsModule
  ],
  standalone: true
})
export class LoginComponent {
  EmailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;
  PasswordVisible = false;
  wasIncorrect = false;
  ValidForm = false;

  constructor(
    private formBuilder: FormBuilder,
    private api: ApiService,
    private userService: CurrentUserService,
    private router: Router
  ) {
  }

  honeypotValidator(control: AbstractControl): ValidationErrors | null {
    if (control.value) {
      return {botSubmission: true};
    }
    return null;
  }

  loginForm = this.formBuilder.group({
    email: ['', [Validators.required,
      Validators.pattern(this.EmailRegex)]],
    password: ['', Validators.required],
    website: [
      '', [this.honeypotValidator
      ]
    ]
  });

  onSubmit(): void {
    if (this.loginForm.invalid || !this.loginForm.value.email || !this.loginForm.value.password) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.wasIncorrect = false;

    const user: UserLoginDto = {
      email: this.loginForm.value.email,
      password: this.loginForm.value.password
    }

    this.api
      .login(user)
      .subscribe({
        next: (userDto) => {
          this.userService.setCurrentUser(userDto);

          this.loginForm.reset();
          void this.router.navigate(['']);
        },
        error: (_) => alert('Incorrect email or password. Please try again.')
      });

    this.ValidForm = true;
  }
  togglePasswordVisibility(): void {
    this.PasswordVisible = !this.PasswordVisible;
  }
}

