import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../services/admin.service';
import { NotificationService } from '../../../services/notification.service';
import { User } from '../../../models/user.model';
import { TitleComponent } from '../../../shared/title/title.component';
import { UserCardComponent } from '../../../components/user-card/user-card.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.scss'],
  imports: [TitleComponent, UserCardComponent, CommonModule],
})
export class UsersComponent implements OnInit {
  users: any[] = [];
  displayedUsers: any[] = [];

  currentPage = 1;
  pageSize = 5;

  constructor(
    private adminService: AdminService,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.adminService.getAllUsers().subscribe({
      next: (data: User[]) => {
        this.users = data.map((user) => ({
          ...user,
          photoUrl: `https://localhost:7093/api/image/user/${user.id}`,
        }));
        this.updateDisplayedUsers();
      },
      error: () =>
        this.notificationService.show('Failed to load users', 'error'),
    });
  }

  updateDisplayedUsers(): void {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.displayedUsers = this.users.slice(start, end);
  }

  changePage(offset: number): void {
    this.currentPage += offset;
    this.updateDisplayedUsers();
  }

  totalPages(): number {
    return Math.ceil(this.users.length / this.pageSize);
  }

  onPromote(user: any): void {
    this.adminService.promoteUser(user.id).subscribe({
      next: () => {
        user.role = 'Admin';
        this.notificationService.show('User promoted to Admin', 'success');
      },
      error: () =>
        this.notificationService.show('Failed to promote user', 'error'),
    });
  }

  onDemote(user: User): void {
    this.adminService.demoteUser(user.id).subscribe({
      next: () => {
        user.role = 'User';
        this.notificationService.show('User demoted to User', 'success');
      },
      error: () => {
        this.notificationService.show('Failed to demote user', 'error');
      },
    });
  }

  onToggleBan(user: User): void {
    if (user.banned) {
      this.adminService.unbanUser(user.id).subscribe({
        next: () => {
          user.banned = false;
          this.notificationService.show(
            'User unbanned successfully',
            'success'
          );
        },
        error: () => {
          this.notificationService.show('Failed to unban user', 'error');
        },
      });
    } else {
      this.adminService.banUser(user.id).subscribe({
        next: () => {
          user.banned = true;
          this.notificationService.show('User banned successfully', 'success');
        },
        error: () => {
          this.notificationService.show('Failed to ban user', 'error');
        },
      });
    }
  }
}
