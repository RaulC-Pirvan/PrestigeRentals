import { Component, OnInit } from '@angular/core';
import { AdminOrderOverviewComponent } from '../components/admin-order-overview/admin-order-overview.component';
import { CommonModule } from '@angular/common';
import { OrderService } from '../../../services/order.service';
import { Order } from '../../../models/order.model';
import { TitleComponent } from "../../../shared/title/title.component";

@Component({
  selector: 'app-orders',
  imports: [AdminOrderOverviewComponent, CommonModule, TitleComponent],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss',
})
export class OrdersComponent implements OnInit {
  allOrders: Order[] = [];
  displayedOrders: Order[] = [];

  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor(private orderService: OrderService) {}

  ngOnInit(): void {
    this.loadAllOrders();
  }

  private loadAllOrders(): void {
    this.orderService.getAllOrders().subscribe({
      next: (orders) => {
        this.allOrders = orders;
        this.currentPage = 1;
        this.updateDisplayedOrders();
      },
      error: (err) => console.error('Failed to load orders', err),
    });
  }

  updateDisplayedOrders(): void {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.displayedOrders = this.allOrders.slice(start, end);
  }

  changePage(offset: number): void {
    const newPage = this.currentPage + offset;
    if (newPage > 0 && newPage <= this.totalPages()) {
      this.currentPage = newPage;
      this.updateDisplayedOrders();
    }
  }

  totalPages(): number {
    return Math.ceil(this.allOrders.length / this.itemsPerPage);
  }
}
