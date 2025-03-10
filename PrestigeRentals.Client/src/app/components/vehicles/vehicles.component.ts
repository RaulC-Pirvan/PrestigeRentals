import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  VehicleService,
  Vehicle,
  VehicleOptions,
  VehiclePhotos,
} from '../../services/vehicle.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-vehicles',
  standalone: true,
  imports: [CommonModule, FormsModule], // Include HttpClientModule here
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.scss'],
})
export class VehiclesComponent implements OnInit {
  vehicles: Vehicle[] = [];
  vehicleOptions: { [key: number]: VehicleOptions } = {};
  vehiclePhotos: { [key: number]: VehiclePhotos[] } = {};

  newVehicle = {
    make: '',
    model: '',
    engineSize: '',
    fuelType: '',
    transmission: '',
  };

  newVehicleOptions = {
    navigation: false,
    headsUpDisplay: false,
    hillAssist: false,
    cruiseControl: false,
  };

  photoPreviews: string[] = [];

  constructor(private vehicleService: VehicleService) {}

  ngOnInit(): void {
    this.loadVehicles();
  }

  loadVehicles(): void {
    this.vehicleService.getVehicles().subscribe(
      (data) => {
        this.vehicles = data;
        this.vehicles.forEach((vehicle) => {
          this.loadVehicleOptions(vehicle.id);
          this.loadVehiclePhotos(vehicle.id);
        });
      },
      (error) => {
        console.error('Error loading vehicles:', error);
      }
    );
  }

  loadVehicleOptions(vehicleId: number): void {
    this.vehicleService.getVehicleOptions(vehicleId).subscribe(
      (options) => {
        this.vehicleOptions[vehicleId] = options;
      },
      (error) => {
        console.error(`Error loading options for vehicle ${vehicleId}:`, error);
      }
    );
  }

  loadVehiclePhotos(vehicleId: number): void {
    this.vehicleService.getVehiclePhotos(vehicleId).subscribe(
      (photos) => {
        this.vehiclePhotos[vehicleId] = photos;
      },
      (error) => {
        console.error(`Error loading photos for vehicle ${vehicleId}:`, error);
      }
    );
  }
}
