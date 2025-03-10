import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common'; // Import CommonModule for common directives
import { FormsModule } from '@angular/forms'; // Import FormsModule for ngModel
import { VehicleService, Vehicle, VehicleOptions } from '../../services/vehicle.service';

@Component({
  selector: 'app-vehicles',
  standalone: true, // This makes it a standalone component
  imports: [CommonModule, FormsModule], // Import necessary modules here
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.scss'],
})

export class VehiclesComponent implements OnInit {
  vehicles: Vehicle[] = [];
  vehicleOptions: Record<number, VehicleOptions> = {};
  selectedVehicle: Vehicle | null = null;
  newVehicle: VehicleForPOST = {
    make: '',
    model: '',
    fuelType: '',
    transmission: '',
    engineSize: 0,
    active: true,
    deleted: false,
    options: {
      navigation: false,
      headsUpDisplay: false,
      hillAssist: false,
      cruiseControl: false,
    },
  };
  vehicleId: number | null = null;
  onlyActive: boolean = false;
  vehiclePhotos: any[] = [];

  constructor(private vehicleService: VehicleService) {}

  ngOnInit(): void {
    console.log('VehiclesComponent initialized.');
    this.loadVehicles();
  }

  loadVehicles() {
    console.log('Loading vehicles...');
    this.vehicleService.getVehiclesWithOptions(this.onlyActive).subscribe(
      (data) => {
        this.vehicles = data.vehicles;
        this.vehicleOptions = data.options;
        console.log('Vehicles loaded:', this.vehicles);
        console.log('Vehicle options loaded:', this.vehicleOptions);
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
        this.newVehicle = {
          make: '',
          model: '',
          fuelType: '',
          transmission: '',
          engineSize: 0,
          active: true,
          deleted: false,
          options: {
            navigation: false,
            headsUpDisplay: false,
            hillAssist: false,
            cruiseControl: false,
          },
        };
        console.log('New vehicle form reset:', this.newVehicle);
      },
      (error) => {
        console.error('Error adding vehicle:', error);
      }
    );
  }

  updateVehicle() {
    if (this.selectedVehicle) {
      console.log(
        'Updating vehicle with ID:',
        this.selectedVehicle.id,
        'Data:',
        this.selectedVehicle
      );
      this.vehicleService
        .updateVehicle(this.selectedVehicle.id, this.selectedVehicle)
        .subscribe(
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

  getOption(vehicleId: number, optionName: string): string {
    const options = this.vehicleOptions[vehicleId];
    if (!options) return 'false';

    switch (optionName.toLowerCase()) {
      case 'navigation':
        return options.navigation ? 'true' : 'false';
      case 'heads-up display':
        return options.headsUpDisplay ? 'true' : 'false';
      case 'hill assist':
        return options.hillAssist ? 'true' : 'false';
      case 'cruise control':
        return options.cruiseControl ? 'true' : 'false';
      default:
        return 'false';
    }
  }

  getVehiclePhotos(vehicleId: number) {
    this.vehicleService.getVehiclePhotos(vehicleId).subscribe(
      (photos) => {
        this.vehiclePhotos = photos;
      },
      (error) => {
        console.error("Error fetching vehicle photos", error);
        this.vehiclePhotos = [];
      }
    );
  }
}
