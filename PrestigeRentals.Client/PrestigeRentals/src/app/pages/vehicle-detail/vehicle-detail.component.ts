import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VehicleService } from '../../services/vehicle.service';
import { VehicleDto } from '../../models/vehicle-dto.model';
import { CommonModule } from '@angular/common';
import { FooterComponent } from '../../components/footer/footer.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { ButtonComponent } from '../../shared/button/button.component';

export interface VehicleOptions {
  navigation: boolean;
  headsUpDisplay: boolean;
  hillAssist: boolean;
  cruiseControl: boolean;
}

@Component({
  selector: 'app-vehicle-detail',
  standalone: true,
  imports: [
    CommonModule,
    FooterComponent,
    NavbarComponent,
    FormsModule,
    ButtonComponent,
  ],
  templateUrl: './vehicle-detail.component.html',
  styleUrls: ['./vehicle-detail.component.scss'],
})
export class VehicleDetailComponent implements OnInit {
  vehicleId!: number;
  vehicleData?: VehicleDto;
  imageUrls: string[] = [];

  startTime: string = '';
  endTime: string = '';
  totalDays: number = 0;
  totalCost: number = 0;

  vehicleOptions: VehicleOptions | null = null;
  trueFeatures: string[] = [];

  constructor(
    private route: ActivatedRoute,
    private vehicleService: VehicleService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.vehicleId = Number(idParam);
      this.vehicleService.getVehicleById(this.vehicleId).subscribe((data) => {
        this.vehicleData = data;
        this.loadVehicleImages();
        this.loadVehicleOptions();
      });
    }
  }

  loadVehicleImages() {
    const vehicleId = this.vehicleData?.id;
    if (!vehicleId) return;

    console.log('Loading images for vehicleId:', vehicleId);

    this.vehicleService.getVehicleImage(vehicleId).subscribe({
      next: (mainBlob) => {
        const mainImageUrl = URL.createObjectURL(mainBlob);
        this.imageUrls = [mainImageUrl];
        console.log('Main image URL:', mainImageUrl);

        this.vehicleService.getAdditionalVehicleImages(vehicleId).subscribe({
          next: (filenames) => {
            console.log('Additional filenames:', filenames);

            const baseFileUrl = `${this.vehicleService.baseUrl}/image/vehicle/file/`;
            const additionalUrls = filenames
              .filter((name) => !name.toLowerCase().startsWith('main'))
              .map((name) => `${baseFileUrl}${name}`);

            console.log('Additional image URLs:', additionalUrls);
            this.imageUrls.push(...additionalUrls);
          },
          error: (err) =>
            console.error('Error fetching vehicle image filenames', err),
        });
      },
      error: (err) => console.error('Error fetching main vehicle image', err),
    });
  }

  currentImageIndex = 0;

  prevImage() {
    if (this.imageUrls.length <= 1) return;
    this.currentImageIndex =
      (this.currentImageIndex - 1 + this.imageUrls.length) %
      this.imageUrls.length;
  }

  nextImage() {
    if (this.imageUrls.length <= 1) return;
    this.currentImageIndex =
      (this.currentImageIndex + 1) % this.imageUrls.length;
  }

  goToImage(index: number) {
    this.currentImageIndex = index;
  }

  calculateCostAndDuration() {
    console.log('startTime:', this.startTime, 'endTime:', this.endTime);

    if (!this.startTime || !this.endTime || !this.vehicleData) return;

    const start = new Date(this.startTime);
    const end = new Date(this.endTime);

    console.log('start date:', start, 'end date:', end);

    if (end < start) {
      this.totalDays = 0;
      this.totalCost = 0;
      return;
    }

    const diffMs = end.getTime() - start.getTime();
    const diffDays = diffMs / (1000 * 60 * 60 * 24);

    this.totalDays = Math.ceil(diffDays);

    this.totalCost = this.totalDays * this.vehicleData.pricePerDay;

    console.log('totalDays:', this.totalDays, 'totalCost:', this.totalCost);
  }

  termsAccepted = false;
  showTermsPopup = false;

  openTermsPopup() {
    this.showTermsPopup = true;
  }

  closeTermsPopup() {
    this.showTermsPopup = false;
  }

  loadVehicleOptions() {
    this.vehicleService.getVehicleOptions(this.vehicleId).subscribe({
      next: (options) => {
        this.vehicleOptions = options;

        const excludedKeys = ['active', 'deleted', 'id', 'vehicleId'];

        this.trueFeatures = Object.entries(options)
          .filter(
            ([key, value]) => value === true && !excludedKeys.includes(key)
          )
          .map(([key]) =>
            key
              .replace(/([A-Z])/g, ' $1')
              .replace(/^./, (str) => str.toUpperCase())
          );
      },
      error: (err) => console.error('Error fetching vehicle options', err),
    });
  }
}
