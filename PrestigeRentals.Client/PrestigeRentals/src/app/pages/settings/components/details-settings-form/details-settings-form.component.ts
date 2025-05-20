import { Component, OnInit } from '@angular/core';
import { ButtonComponent } from '../../../../shared/button/button.component';
import { AuthService, UserDetailsRequest } from '../../../../services/auth.service';
import { UserService } from '../../../../services/user.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NotificationService } from '../../../../services/notification.service';

@Component({
  selector: 'app-details-settings-form',
  imports: [ButtonComponent, ReactiveFormsModule],
  templateUrl: './details-settings-form.component.html',
  styleUrls: ['./details-settings-form.component.scss'],
})
export class DetailsSettingsFormComponent implements OnInit {
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
      firstName: [''], 
      lastName: [''],
    });
  
    this.authService.userDetails$.subscribe((user: UserDetailsRequest | null) => {
      console.log('User data in details-settings-form:', user);
      if (user && user.id) {
        this.userId = user.id;
        this.form.patchValue({
          firstName: user.name.split(' ')[0] || '',
          lastName: user.name.split(' ')[1] || '',
        });
      }
    });
  }

  onSubmit() {
    console.log('Form valid:', this.form.valid);
    console.log('User ID:', this.userId);
    console.log('Form values:', this.form.value);

    if (this.form.valid && this.userId) {
      const { firstName, lastName } = this.form.value;
      
      if (!firstName.trim() && !lastName.trim()) {
        this.notificationService.show('Please fill in at least one field.', 'error');
        return;
      }
      
      // proceed with update
      this.userService
        .updateUserDetails(this.userId, { firstName, lastName })
        .subscribe({
          next: () => {
            this.notificationService.show('Details successfully updated!', 'success');
            setTimeout(() => location.reload(), 1000);
          },
          error: (err) => {
            this.notificationService.show('Error while updating details', 'error');
            console.error('Update failed', err);
          }
        });
    } else {
      console.warn('Form invalid or userId missing, not sending API request');
    }
  }
}