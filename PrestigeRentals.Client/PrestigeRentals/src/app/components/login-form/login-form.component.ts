import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { TitleComponent } from '../../shared/title/title.component';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { NotificationComponent } from '../notification/notification.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login-form',
  imports: [
    ButtonComponent,
    TitleComponent,
    ReactiveFormsModule,
    NotificationComponent,
  ],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.scss',
})
export class LoginFormComponent {
  loginForm: FormGroup;

  notificationMessage: string | null = null;
  notificationType: 'success' | 'error' = 'success';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authService: AuthService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', Validators.required],
      rememberMe: [false],
    });
  }

  showNotification(message: string, type: 'success' | 'error') {
    this.notificationMessage = message;
    this.notificationType = type;

    setTimeout(() => {
      this.notificationMessage = null;
    }, 3000);
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { email, password, rememberMe } = this.loginForm.value;

      this.http
        .post<{ token: string }>('/api/login', { email, password })
        .subscribe({
          next: (response) => {
            const token = response.token;
            if (rememberMe) {
              localStorage.setItem('authToken', token);
            } else {
              sessionStorage.setItem('authToken', token);
            }

            this.authService.login(token); // <-- notify the AuthService here

            this.showNotification('Login successful!', 'success');
            setTimeout(() => {
              this.router.navigate(['/']);
            }, 1000);
          },
          error: (error) => {
            this.showNotification('Invalid credentials', 'error');
            console.error('Login failed', error);
          },
        });
    }
  }
}
