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
import { AuthService, UserDetailsRequest } from '../../services/auth.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-login-form',
  imports: [ButtonComponent, TitleComponent, ReactiveFormsModule],
  templateUrl: './login-form.component.html',
  styleUrl: './login-form.component.scss',
})
export class LoginFormComponent {
  loginForm: FormGroup;

  rememberMe = false;
  notificationMessage: string | null = null;
  notificationType: 'success' | 'error' = 'success';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authService: AuthService,
    private notificationService: NotificationService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required]],
      password: ['', Validators.required],
      rememberMe: [false],
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      const { email, password, rememberMe } = this.loginForm.value;

      this.http.post<{ token: string; firstName: string; lastName: string; imageData: string }>('/api/login', { email, password })
      .subscribe({
        next: (response) => {
          const token = response.token;
    
          const user = {
            name: `${response.firstName} ${response.lastName}`,
            photo: response.imageData, // Only the base64 string
          };
    
          this.authService.login(token, rememberMe);
          this.notificationService.show('Login successful!', 'success');
    
          setTimeout(() => {
            this.authService.loggedInSubject.next(true);
            this.authService.loadUserProfile();
            this.router.navigate(['/']);
          }, 1000);
        },
        error: () => {
          this.notificationService.show('Invalid credentials', 'error');
        }
      });
    }
  }
}
