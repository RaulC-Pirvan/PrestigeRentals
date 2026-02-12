import { Component, EventEmitter, Input, Output } from '@angular/core';
import { User } from '../../models/user.model';
import { CommonModule } from '@angular/common';
import { ButtonComponent } from "../../shared/button/button.component";

@Component({
  selector: 'app-user-card',
  imports: [CommonModule, ButtonComponent],
  templateUrl: './user-card.component.html',
  styleUrl: './user-card.component.scss',
})
export class UserCardComponent {
  @Input() user!: User;

  @Output() promote = new EventEmitter<User>();
  @Output() demote = new EventEmitter<User>();
  @Output() ban = new EventEmitter<User>();

  onPromote(): void {
    this.promote.emit(this.user);
  }

  onDemote(): void {
    this.demote.emit(this.user);
  }

  onBan(): void {
    this.ban.emit(this.user);
  }

}
