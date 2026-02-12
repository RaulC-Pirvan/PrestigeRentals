import { Component, Input } from '@angular/core';
import { Review } from '../../../../models/review.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-review-overview',
  imports: [CommonModule],
  templateUrl: './admin-review-overview.component.html',
  styleUrl: './admin-review-overview.component.scss',
})
export class AdminReviewOverviewComponent {
goToInventoryOrLogin() {
throw new Error('Method not implemented.');
}
  @Input() review!: Review;
}
