import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { OrderPreviewComponent } from '../../components/order-preview/order-preview.component';
import { ProfileService, UserProfile } from '../../services/profile.service';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models/order.model';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, NavbarComponent, FooterComponent, OrderPreviewComponent],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
})
export class ProfileComponent implements OnInit {
  firstName: string = 'USER';

  orders: Order[] = [];
  reviews: string[] = Array.from({ length: 14 }, (_, i) => `Review #${i + 1}`);

  displayedOrders: Order[] = [];
  displayedReviews: string[] = [];

  currentView: 'orders' | 'reviews' = 'orders';
  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor(private profileService: ProfileService, private orderService: OrderService) {}

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
