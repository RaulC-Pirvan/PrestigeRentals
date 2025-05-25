import { Component, OnInit } from '@angular/core';
import { Vehicle, VehicleService } from '../../services/vehicle.service';
import { VehicleCardComponent } from '../vehicle-card/vehicle-card.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-vehicles-list',
  standalone: true,
  imports: [VehicleCardComponent, CommonModule],
  templateUrl: './vehicles-list.component.html',
  styleUrls: ['./vehicles-list.component.scss']
})
export class VehiclesListComponent implements OnInit {
  vehicles: Vehicle[] = [];
  loading = false;
  error: string | null = null;

  // Pagination
  currentPage = 1;
  pageSize = 9; // 3 vehicles per row * 3 rows
  pagedVehicles: Vehicle[] = [];

  constructor(private vehicleService: VehicleService) {}

  ngOnInit(): void {
    this.loadVehicles();
  }

  loadVehicles(): void {
    this.loading = true;
    this.vehicleService.getAllVehicles(false).subscribe({
      next: (vehicles) => {
        this.vehicles = vehicles;
        this.setPage(1);
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load vehicles.';
        console.error(err);
        this.loading = false;
      }
    });
  }

  setPage(page: number): void {
    if (page < 1) page = 1;
    if (page > this.totalPages) page = this.totalPages;

    this.currentPage = page;
    const startIndex = (page - 1) * this.pageSize;
    const endIndex = startIndex + this.pageSize;
    this.pagedVehicles = this.vehicles.slice(startIndex, endIndex);
  }

  get totalPages(): number {
    return Math.ceil(this.vehicles.length / this.pageSize);
  }
}
