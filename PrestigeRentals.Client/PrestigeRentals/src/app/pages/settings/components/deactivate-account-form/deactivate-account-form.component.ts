import { Component } from '@angular/core';
import { ButtonComponent } from '../../../../shared/button/button.component';
import { UserService } from '../../../../services/user.service';
import { NotificationService } from '../../../../services/notification.service';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-deactivate-account-form',
  imports: [ButtonComponent],
  templateUrl: './deactivate-account-form.component.html',
  styleUrl: './deactivate-account-form.component.scss',
})
export class DeactivateAccountFormComponent {
  userId!: number;

  constructor(
    private userService: UserService,
    private notificationService: NotificationService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.authService.userDetails$.subscribe(user => {
      if (user?.id) {
        this.userId = user.id;
      }
    });
  }

  deactivateAccount() {
    if (!this.userId) {
      console.error('User ID is undefined');
      return;
    }
    this.userService.setInactive(this.userId).subscribe({
      next: () => {
        this.notificationService.show('Account successfully deactivated', 'success');
        setTimeout(() => {
          this.authService.logout();
        }, 1000);
      },
      error: (err) => {
        this.notificationService.show('Failed to deactivate account', 'error');
      }
    });
  }
}
