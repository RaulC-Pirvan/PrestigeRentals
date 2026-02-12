import { Component } from '@angular/core';
import { ButtonComponent } from '../../../../shared/button/button.component';
import { UserService } from '../../../../services/user.service';
import { NotificationService } from '../../../../services/notification.service';
import { AuthService } from '../../../../services/auth.service';

@Component({
  selector: 'app-delete-account-form',
  imports: [ButtonComponent],
  templateUrl: './delete-account-form.component.html',
  styleUrl: './delete-account-form.component.scss',
})
export class DeleteAccountFormComponent {
  userId!: number;

  constructor(
    private userService: UserService,
    private notificationService: NotificationService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.authService.userDetails$.subscribe((user) => {
      if (user?.id) {
        this.userId = user.id;
      }
    });
  }

  deleteAccount() {
    if (!this.userId) {
      console.error('User ID is undefined');
      return;
    }
    this.userService.deleteAccount(this.userId).subscribe({
      next: () => {
        this.notificationService.show(
          'Account successfully deleted',
          'success'
        );
        setTimeout(() => {
          this.authService.logout();
        }, 1000);
      },
      error: (err) => {
        this.notificationService.show('Failed to delete account', 'error');
      },
    });
  }
}
