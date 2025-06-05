import { Component } from '@angular/core';
import { TitleComponent } from '../../../shared/title/title.component';
import { VehicleService } from '../../../services/vehicle.service';
import { Vehicle } from '../../../models/vehicle.model';
import { CommonModule } from '@angular/common';
import { AdminVehicleCardComponent } from '../components/admin-vehicle-card/admin-vehicle-card.component';
import { forkJoin, tap } from 'rxjs';
import { AdminAddVehicleComponent } from '../../../components/admin-add-vehicle/admin-add-vehicle.component';

@Component({
  selector: 'app-vehicles',
  imports: [
    TitleComponent,
    CommonModule,
    AdminVehicleCardComponent,
    AdminAddVehicleComponent
  ],
  templateUrl: './vehicles.component.html',
  styleUrl: './vehicles.component.scss',
})
export class VehiclesComponent {
  vehicles: Vehicle[] = [];
  displayedVehicles: Vehicle[] = [];

  currentPage = 1;
  pageSize = 5;
  mode: 'overview' | 'add' = 'overview';

  constructor(private vehicleService: VehicleService) {}

  ngOnInit(): void {
    this.loadVehicles();
  }

  loadVehicles() {
    this.vehicleService.getAllVehicles().subscribe((vehicles) => {
      this.vehicles = vehicles;

      const requests = this.vehicles.map((vehicle) =>
        forkJoin({
          imageBlob: this.vehicleService.getVehicleImage(vehicle.id),
          options: this.vehicleService.getVehicleOptions(vehicle.id),
        }).pipe(
          tap(({ imageBlob, options }) => {
            const reader = new FileReader();
            reader.onload = () => {
              vehicle.imageUrl = reader.result as string;
            };
            reader.readAsDataURL(imageBlob);

            vehicle.navigation = options.navigation;
            vehicle.headsupDisplay = options.headsUpDisplay;
            vehicle.hillAssist = options.hillAssist;
            vehicle.cruiseControl = options.cruiseControl;
          })
        )
      );

      forkJoin(requests).subscribe(() => {
        console.log('Toate vehiculele au fost încărcate cu poze și dotări');
        this.updateDisplayedVehicles();
      });
    });
  }

  updateDisplayedVehicles() {
    const start = (this.currentPage - 1) * this.pageSize;
    const end = start + this.pageSize;
    this.displayedVehicles = this.vehicles.slice(start, end);
  }

  totalPages(): number {
    return Math.ceil(this.vehicles.length / this.pageSize);
  }

  changePage(delta: number) {
    this.currentPage += delta;
    this.updateDisplayedVehicles();
  }

  setMode(mode: 'overview' | 'add') {
    console.log('Set mode:', mode);
    this.mode = mode;
  }

  handleVehicleAdded(event: {
    vehicleData: any;
    mainImageFile?: File | undefined; // pune ? aici
    secondaryImageFiles: File[];
  }) {
    // Trimite datele la backend

    const { vehicleData, mainImageFile, secondaryImageFiles } = event;

    this.vehicleService.createVehicle(vehicleData).subscribe({
      next: (vehicle) => {
        // Dacă ai nevoie să încarci pozele separat, fă asta după ce primești vehicle.id

        if (mainImageFile) {
          this.vehicleService
            .uploadMainImage(vehicle.id, mainImageFile)
            .subscribe(() => {
              console.log('Main image uploaded');
            });
        }
        if (secondaryImageFiles.length > 0) {
          secondaryImageFiles.forEach((file) => {
            this.vehicleService
              .uploadSecondaryImage(vehicle.id, file)
              .subscribe(() => {
                console.log('Secondary image uploaded');
              });
          });
        }

        this.vehicles.push(vehicle);
        this.setMode('overview');
        this.updateDisplayedVehicles();
      },
      error: (err) => {
        console.error('Error adding vehicle', err);
      },
    });
  }
}
