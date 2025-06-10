import {
  Component,
  Input,
  OnChanges,
  SimpleChanges,
  OnInit,
} from '@angular/core';
import { VehicleService } from '../../services/vehicle.service';
import { VehicleCardComponent } from '../vehicle-card/vehicle-card.component';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { Vehicle } from '../../models/vehicle.model';
import { Order } from '../../models/order.model';
import { OrderService } from '../../services/order.service';

@Component({
  selector: 'app-vehicles-list',
  standalone: true,
  imports: [VehicleCardComponent, CommonModule],
  templateUrl: './vehicles-list.component.html',
  styleUrls: ['./vehicles-list.component.scss'],
})
export class VehiclesListComponent implements OnInit, OnChanges {
  @Input() filters: any = {};
  @Input() searchTerm: string = '';
  @Input() sortBy: string = '';

  vehicles: Vehicle[] = [];
  filteredVehicles: Vehicle[] = [];
  pagedVehicles: Vehicle[] = [];
  loading = false;
  error: string | null = null;

  currentPage = 1;
  pageSize = 9;

  orders: Order[] = [];
  orderMap = new Map<number, string>(); // vehicleId -> endTime

  constructor(
    private vehicleService: VehicleService,
    private authService: AuthService,
    private orderService: OrderService
  ) {}

  ngOnInit(): void {
    this.authService.userLoaded$.subscribe((loaded) => {
      if (loaded) {
        this.loadVehicles();
        this.loadOrders();
      }
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['filters'] || changes['searchTerm'] || changes['sortBy']) {
      this.applyAllFilters();
    }
  }

  loadVehicles(): void {
    this.loading = true;
    this.vehicleService.getAllVehicles(false).subscribe({
      next: (vehicles) => {
        this.vehicles = vehicles;
        this.applyAllFilters();
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load vehicles.';
        console.error(err);
        this.loading = false;
      },
    });
  }

  loadOrders(): void {
    this.orderService.getAllOrders().subscribe({
      next: (orders) => {
        const now = new Date();
        this.orders = orders;
        this.orderMap.clear();

        for (const order of orders) {
          const start = new Date(order.startTime);
          const end = new Date(order.endTime);

          if (!order.isCancelled && start <= now && end > now) {
            this.orderMap.set(order.vehicleId, order.endTime);
          }
        }
      },
      error: (err) => {
        console.error('Failed to load orders.', err);
      },
    });
  }

  applyAllFilters(): void {
    let results = [...this.vehicles];

    // Apply filters
    for (const key of Object.keys(this.filters)) {
      const value = this.filters[key];
      if (value) {
        results = results.filter(
          (v: any) => (v[key] ?? '').toLowerCase() === value.toLowerCase()
        );
      }
    }

    // Apply search
    if (this.searchTerm) {
      const term = this.searchTerm.toLowerCase();
      results = results.filter(
        (vehicle) =>
          vehicle.make.toLowerCase().includes(term) ||
          vehicle.model.toLowerCase().includes(term)
      );
    }

    // Apply sorting
    if (this.sortBy === 'priceAsc') {
      results.sort((a, b) => (a.horsepower ?? 0) - (b.horsepower ?? 0)); // Replace with actual price field
    } else if (this.sortBy === 'priceDesc') {
      results.sort((a, b) => (b.horsepower ?? 0) - (a.horsepower ?? 0)); // Replace with actual price field
    }

    this.filteredVehicles = results;
    this.setPage(1);
  }

  setPage(page: number): void {
    if (page < 1) page = 1;
    const totalPages = this.totalPages;
    if (page > totalPages) page = totalPages;

    this.currentPage = page;
    const startIndex = (page - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pagedVehicles = this.filteredVehicles.slice(startIndex, endIndex);
  }

  get totalPages(): number {
    return Math.ceil(this.filteredVehicles.length / this.pageSize);
  }
}
