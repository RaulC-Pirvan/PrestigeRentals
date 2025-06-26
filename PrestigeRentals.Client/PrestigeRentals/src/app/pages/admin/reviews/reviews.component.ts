import { Component } from '@angular/core';
import { ReviewService } from '../../../services/review.service';
import { Review } from '../../../models/review.model';
import { AdminReviewOverviewComponent } from "../components/admin-review-overview/admin-review-overview.component";
import { TitleComponent } from "../../../shared/title/title.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reviews',
  imports: [AdminReviewOverviewComponent, TitleComponent, CommonModule],
  templateUrl: './reviews.component.html',
  styleUrl: './reviews.component.scss',
})
export class ReviewsComponent {
  allReviews: Review[] = [];
  displayedReviews: Review[] = [];

  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor(private reviewService: ReviewService) {}

  ngOnInit(): void {
    this.loadAllReviews();
  }

  private loadAllReviews(): void {
    this.reviewService.getAllReviews().subscribe({
      next: (reviews) => {
        this.allReviews = reviews;
        this.currentPage = 1;
        this.updateDisplayedReviews();
      },
      error: (err) => console.error('Error loading reviews', err),
    });
  }

  updateDisplayedReviews(): void {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.displayedReviews = this.allReviews.slice(start, end);
  }

  changePage(offset: number): void {
    const newPage = this.currentPage + offset;
    if (newPage > 0 && newPage <= this.totalPages()) {
      this.currentPage = newPage;
      this.updateDisplayedReviews();
    }
  }

  totalPages(): number {
    return Math.ceil(this.allReviews.length / this.itemsPerPage);
  }
}
