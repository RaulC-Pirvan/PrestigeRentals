import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../components/navbar/navbar.component';
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
  imports: [
    CommonModule,
    NavbarComponent,
    OrderPreviewComponent,
    ReviewPreviewComponent,
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
  firstName: string = '';
  lastName: string = '';
  userId: number | null = null;
  photoUrl: string = 'assets/default-avatar.jpg';

  orders: Order[] = [];
  reviews: Review[] = [];

  displayedOrders: Order[] = [];
  displayedReviews: Review[] = [];

  currentView: 'orders' | 'reviews' = 'orders';
  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor(
    private profileService: ProfileService,
    private orderService: OrderService,
    private reviewService: ReviewService
  ) {}

  ngOnInit(): void {
    console.log('ProfileComponent initialized');
    this.profileService.getProfile().subscribe({
      next: (profile: UserProfile) => {
        console.log('Profile retrieved:', profile);
        this.firstName = profile.firstName || 'USER';
        this.lastName = profile.lastName || 'NAME';
        this.userId = profile.userId;
    
        if (this.userId) {
          console.log('Loading user photo for userId:', this.userId);
          this.loadUserPhoto(this.userId); // ✅ only call here
        } else {
          console.warn('UserId is null or undefined');
        }
      },
      error: (err) => {
        console.error('Error loading profile', err);
      },
    });
  
    this.loadOrders();
    this.loadReviews();
    this.updateDisplayedItems();
  }

  loadOrders() {
    this.orderService.getUserOrders().subscribe({
      next: (orders: Order[]) => {
        this.orders = orders;
        this.currentPage = 1; // <-- Reset pagination
        if (this.currentView === 'orders') {
          this.updateDisplayedItems(); // <-- Only update if 'orders' is the active view
        }
      },
      error: (err) => {
        console.error('Error loading orders', err);
      },
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
      },
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
    const dataLength =
      this.currentView === 'orders' ? this.orders.length : this.reviews.length;
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
    const dataLength =
      this.currentView === 'orders' ? this.orders.length : this.reviews.length;
    return Math.ceil(dataLength / this.itemsPerPage);
  }

  loadUserPhoto(userId: number) {
    console.log(`Fetching user image for userId: ${userId}`);
  
    this.profileService.getUserImageUrl(userId).subscribe({
      next: (blob: Blob) => {
        console.log('User image blob received:', blob);
        const reader = new FileReader();
        reader.onload = () => {
          console.log('Image converted to data URL');
          this.photoUrl = reader.result as string;
          console.log('photoUrl set:', this.photoUrl);
        };
        reader.onerror = (error) => {
          console.error('Error reading blob as data URL:', error);
        };
        reader.readAsDataURL(blob);
      },
      error: (err) => {
        console.error('Error loading user photo', err);
      },
    });
  }
}
