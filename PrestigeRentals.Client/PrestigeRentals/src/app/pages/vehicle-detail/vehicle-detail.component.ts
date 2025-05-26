import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { VehicleService } from '../../services/vehicle.service';
import { VehicleDto } from '../../models/vehicle-dto.model';
import { CommonModule } from '@angular/common';
import { FooterComponent } from '../../components/footer/footer.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';

@Component({
  selector: 'app-vehicle-detail',
  imports: [CommonModule, FooterComponent, NavbarComponent],
  templateUrl: './vehicle-detail.component.html',
  styleUrls: ['./vehicle-detail.component.scss'],
})
export class VehicleDetailComponent implements OnInit {
  vehicleId!: number;
  vehicleData?: VehicleDto;
  imageUrls: string[] = [];

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
              .filter(name => !name.toLowerCase().startsWith('main'))
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
}
