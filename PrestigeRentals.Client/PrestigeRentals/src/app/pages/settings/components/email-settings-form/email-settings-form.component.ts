import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { UserService } from '../../../../services/user.service';
import { AuthService, UserDetailsRequest } from '../../../../services/auth.service';
import { NotificationService } from '../../../../services/notification.service';
import { ButtonComponent } from '../../../../shared/button/button.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-email-settings-form',
  standalone: true,
  imports: [ButtonComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './email-settings-form.component.html',
  styleUrls: ['./email-settings-form.component.scss'],
})
export class EmailSettingsFormComponent implements OnInit {
  form!: FormGroup;
  userId!: number;

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      email: [''],
    });
  
    this.authService.userDetails$.subscribe((user: UserDetailsRequest | null) => {
      console.log('User data in email-settings-form:', user);
      if (user && user.id) {
        this.userId = user.id;
        this.form.patchValue({
          email: user.email?.trim() || '', 
        });
      }
    });
  }
  
  onSubmit() {
    console.log('Form valid:', this.form.valid);
    console.log('User ID:', this.userId);
    console.log('Form values:', this.form.value);
  
    if (this.form.valid && this.userId) {
      const email = this.form.value.email?.trim();
  
      if (!email) {
        this.notificationService.show('Please enter a valid email address.', 'error');
        return;
      }
  
      // proceed with update
      this.userService.changeEmail(this.userId, email).subscribe({
        next: () => {
          this.notificationService.show('Email updated successfully!', 'success');
          setTimeout(() => location.reload(), 1000);
        },
        error: (err) => {
          if(err.status === 409) {
            this.notificationService.show('Email already taken.', 'error');
            console.error('Email update error:', err);
          } else {
            this.notificationService.show('Failed to update email.', 'error');
            console.error('Email update error:', err);
          }
        }
      });
    } else {
      console.warn('Form invalid or userId missing, not sending API request');
    }
  }
}
