import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { OrderPreviewComponent } from '../../components/order-preview/order-preview.component';
import { ProfileService, UserProfile } from '../../services/profile.service';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models/order.model';
import { ReviewService } from '../../services/review.service';
import { Review } from '../../models/review.model';
import { ReviewPreviewComponent } from '../../components/review-preview/review-preview.component';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, NavbarComponent, FooterComponent, OrderPreviewComponent, ReviewPreviewComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
  firstName: string = 'USER';

  orders: Order[] = [];
  reviews: Review[] = []

  displayedOrders: Order[] = [];
  displayedReviews: Review[] = [];

  currentView: 'orders' | 'reviews' = 'orders';
  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor(private profileService: ProfileService, private orderService: OrderService, private reviewService: ReviewService) {}

  ngOnInit(): void {
    this.profileService.getProfile().subscribe({
      next: (profile: UserProfile) => {
        this.firstName = profile.firstName || 'USER';
      },
      error: (err) => {
        console.error('Error loading profile', err);
      },
    });

    this.loadOrders();
    this.updateDisplayedItems();
  }

  loadOrders() {
    this.orderService.getOrdersForCurrentUser().subscribe({
      next: (orders: Order[]) => {
        this.orders = orders;
        this.updateDisplayedItems();
      },
      error: (err) => {
        console.error('Error loading orders', err);
      }
    });
  }

    loadReviews() {
    this.reviewService.getReviewsForCurrentUser().subscribe({
      next: (reviews: Review[]) => {
        this.reviews = reviews;
        this.updateDisplayedItems();
      },
      error: (err) => {
        console.error('Error loading reviews', err);
      }
    });
  }

  updateDisplayedItems() {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;

    if (this.currentView === 'orders') {
      this.displayedOrders = this.orders.slice(start, end);
    } else {
      this.displayedReviews = this.reviews.slice(start, end);
    }
  }

  changePage(offset: number) {
    const dataLength = this.currentView === 'orders' ? this.orders.length : this.reviews.length;
    const totalPages = Math.ceil(dataLength / this.itemsPerPage);
    const newPage = this.currentPage + offset;

    if (newPage > 0 && newPage <= totalPages) {
      this.currentPage = newPage;
      this.updateDisplayedItems();
    }
  }

  switchView(view: 'orders' | 'reviews') {
    this.currentView = view;
    this.currentPage = 1;
    this.updateDisplayedItems();
  }

  totalPages(): number {
    const dataLength = this.currentView === 'orders' ? this.orders.length : this.reviews.length;
    return Math.ceil(dataLength / this.itemsPerPage);
  }
}
