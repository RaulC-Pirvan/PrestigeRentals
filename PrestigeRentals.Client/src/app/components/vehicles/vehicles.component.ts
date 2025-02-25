import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { VehicleService, Vehicle, VehicleForPOST } from '../../services/vehicle.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-vehicles',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './vehicles.component.html',
  styleUrl: './vehicles.component.scss'
})
export class VehiclesComponent implements OnInit {
  vehicles: Vehicle[] = [];
  selectedVehicle: Vehicle | null = null;
  newVehicle: VehicleForPOST = { make: '', model: '', fuelType: '', transmission: '', engineSize: 0 };
  vehicleId: number | null = null;

  constructor(private vehicleService: VehicleService) {}

  ngOnInit(): void {
    console.log('VehiclesComponent initialized.');
    this.loadVehicles();
  }

  loadVehicles() {
    console.log('Loading vehicles...');
    this.vehicleService.getVehicles().subscribe(
      (data) => {
        this.vehicles = data;
        console.log('Vehicles loaded:', this.vehicles);
      },
      (error) => {
        console.error('Error loading vehicles:', error);
      }
    );
  }

  getVehicleById() {
    console.log('Attempting to get vehicle by ID:', this.vehicleId);
    if (this.vehicleId != null) {
      this.vehicleService.getVehicleById(this.vehicleId).subscribe(
        (data) => {
          this.selectedVehicle = data;
          console.log('Vehicle fetched by ID:', this.selectedVehicle);
        },
        (error) => {
          console.error('Error fetching vehicle by ID:', error);
        }
      );
    } else {
      console.log('Vehicle ID is null or invalid.');
    }
  }

  selectVehicle(id: number) {
    console.log('Selecting vehicle with ID:', id);
    this.vehicleService.getVehicleById(id).subscribe(
      (data) => {
        this.selectedVehicle = data;
        console.log('Selected vehicle:', this.selectedVehicle);
      },
      (error) => {
        console.error('Error selecting vehicle:', error);
      }
    );
  }

  addVehicle() {
    console.log('Adding new vehicle:', this.newVehicle);
    this.vehicleService.addVehicle(this.newVehicle).subscribe(
      () => {
        console.log('New vehicle added successfully');
        this.loadVehicles();
        this.newVehicle = { make: '', model: '', fuelType: '', transmission: '', engineSize: 0 };
        console.log('New vehicle form reset:', this.newVehicle);
      },
      (error) => {
        console.error('Error adding vehicle:', error);
      }
    );
  }

  updateVehicle() {
    if (this.selectedVehicle) {
      console.log('Updating vehicle with ID:', this.selectedVehicle.id, 'Data:', this.selectedVehicle);
      this.vehicleService.updateVehicle(this.selectedVehicle.id, this.selectedVehicle).subscribe(
        () => {
          console.log('Vehicle updated successfully');
          this.loadVehicles();
          this.selectedVehicle = null;
          console.log('Selected vehicle reset:', this.selectedVehicle);
        },
        (error) => {
          console.error('Error updating vehicle:', error);
        }
      );
    } else {
      console.log('No vehicle selected for update.');
    }
  }

  deleteVehicle(id: number) {
    console.log('Attempting to delete vehicle with ID:', id);
    if (id != null) {
      this.vehicleService.deleteVehicle(id).subscribe(
        () => {
          console.log('Vehicle deleted successfully');
          this.loadVehicles();
        },
        (error) => {
          console.error('Error deleting vehicle:', error);
        }
      );
    } else {
      console.log('Invalid vehicle ID for deletion.');
    }
  }
}
