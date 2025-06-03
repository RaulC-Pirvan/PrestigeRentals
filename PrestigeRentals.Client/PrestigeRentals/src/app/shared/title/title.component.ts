import { CommonModule } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { ProfileService, UserProfile } from '../../services/profile.service';

@Component({
  selector: 'app-title',
  imports: [CommonModule],
  templateUrl: './title.component.html',
  styleUrls: ['./title.component.scss'] 
})
export class TitleComponent implements OnInit {
  @Input() text: string = 'Default Title';
  @Input() username: string = '';

  constructor(private profileService: ProfileService) {}

  ngOnInit(): void {
    this.profileService.getProfile().subscribe({
      next: (profile: UserProfile | null) => {
        if (profile && profile.firstName) {
          this.username = profile.firstName;
        } else {
          this.username = 'Guest';
        }
      },
      error: (err) => {
        console.error('Failed to load user profile', err);
        this.username = 'Guest';
      }
    });
  }
}
