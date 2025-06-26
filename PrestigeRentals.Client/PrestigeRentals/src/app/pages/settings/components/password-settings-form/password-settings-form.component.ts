import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { UserService } from '../../../../services/user.service';
import { AuthService } from '../../../../services/auth.service';
import { NotificationService } from '../../../../services/notification.service';
import { ButtonComponent } from '../../../../shared/button/button.component';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-password-form',
  templateUrl: './password-settings-form.component.html',
  styleUrl: './password-settings-form.component.scss',
  imports: [ButtonComponent, ReactiveFormsModule],
})
export class PasswordSettingsFormComponent implements OnInit {
  form!: FormGroup;
  userId!: number;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private notificationService: NotificationService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      oldPassword: ['', Validators.required],
      newPassword: ['', Validators.required],
      confirmNewPassword: ['', Validators.required],
    });

    this.authService.userDetails$.subscribe(user => {
      if (user?.id) this.userId = user.id;
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.notificationService.show('Please fill out all fields.', 'error');
      return;
    }
    

    const { oldPassword, newPassword, confirmNewPassword } = this.form.value;

    if (newPassword !== confirmNewPassword) {
      this.notificationService.show('New passwords do not match.', 'error');
      return;
    }
    

    this.userService.changePassword(this.userId, oldPassword, newPassword).subscribe({
      next: () => {
        this.notificationService.show('Password changed successfully.', 'success');
        setTimeout(() => {
          this.authService.logout();
        }, 1000);

      },
      error: (err) => {
        if (err.status === 401) {
          this.notificationService.show('Invalid password.', 'error');
        } else {
          this.notificationService.show('Failed to change password.', 'error');
        }
      }
    });
    
  }
}
