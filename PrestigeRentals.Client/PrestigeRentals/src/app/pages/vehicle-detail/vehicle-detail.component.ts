import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VehicleService } from '../../services/vehicle.service';
import { VehicleDto } from '../../models/vehicle-dto.model';
import { CommonModule } from '@angular/common';
import { FooterComponent } from '../../components/footer/footer.component';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FormsModule } from '@angular/forms';
import { ButtonComponent } from '../../shared/button/button.component';
import { Review } from '../../models/review.model';
import { ReviewCardComponent } from '../../components/review-card/review-card.component';
import { ProfileService } from '../../services/profile.service';
import { VehicleSuggestionComponent } from '../../components/vehicle-suggestion/vehicle-suggestion.component';
import { CheckoutDataService } from '../../services/checkout-data.service';

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
    ReviewCardComponent,
    VehicleSuggestionComponent,
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

  averageRating: number = 0;

  similarVehicles: number[] = [];

  reviews: Review[] = [];
  currentPage = 1;
  pageSize = 5;

  vehicleOptions: VehicleOptions | null = null;
  trueFeatures: string[] = [];

  constructor(
    private route: ActivatedRoute,
    private vehicleService: VehicleService,
    private profileService: ProfileService,
    private router: Router,
    private checkoutDataService: CheckoutDataService
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.vehicleId = Number(idParam);
      this.vehicleService.getVehicleById(this.vehicleId).subscribe((data) => {
        this.vehicleData = data;
        this.loadVehicleImages();
        this.loadVehicleOptions();
        this.loadReviewsForVehicle(this.vehicleId);
        this.loadSimilarVehicles();
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

  loadReviewsForVehicle(vehicleId: number) {
    this.vehicleService.getReviewsByVehicleId(vehicleId).subscribe({
      next: (fetchedReviews: Review[]) => {
        fetchedReviews.forEach((review) => {
          this.profileService.getUserProfileById(review.userId).subscribe({
            next: (profile) => {
              review.userFirstName = profile.firstName;
              review.userLastName = profile.lastName;
            },
            error: (err) => console.error('Failed to fetch user profile', err),
          });

          this.profileService.getUserImageUrl(review.userId).subscribe({
            next: (blob) => {
              review.userProfilePhotoUrl = URL.createObjectURL(blob);
            },
            error: (err) => console.error('Failed to fetch user image', err),
          });
        });

        this.reviews = fetchedReviews;
        this.calculateAverageRating();
      },
      error: (err) => console.error('Error fetching reviews', err),
    });
  }

  get pagedReviews(): Review[] {
    const start = (this.currentPage - 1) * this.pageSize;
    return this.reviews.slice(start, start + this.pageSize);
  }

  get totalPages(): number {
    return Math.ceil(this.reviews.length / this.pageSize);
  }

  getStarArray(rating: number): number[] {
    const stars = [];
    for (let i = 1; i <= 5; i++) {
      if (rating >= i) {
        stars.push(2); // full star
      } else if (rating >= i - 0.5) {
        stars.push(1); // half star
      } else {
        stars.push(0); // empty star
      }
    }
    return stars;
  }

  calculateAverageRating(): void {
    if (this.reviews.length === 0) {
      this.averageRating = 0;
      return;
    }

    const totalStars = this.reviews.reduce(
      (sum, review) => sum + review.rating,
      0
    );
    this.averageRating = totalStars / this.reviews.length;
  }

  loadSimilarVehicles() {
    if (!this.vehicleData) return;

    this.vehicleService
      .getSimilarVehicleIds(
        this.vehicleData.id,
        this.vehicleData.chassis,
        this.vehicleData.transmission
      )
      .subscribe({
        next: (ids) => {
          console.log('Similar vehicle IDs received:', ids);
          this.similarVehicles = ids; // assign array of numbers here directly
        },
        error: (err) => console.error('Failed to get similar vehicle IDs', err),
      });
  }

  visibleCount = 3;
  visibleStartIndex = 0;

  get visibleVehicles(): number[] {
    return this.similarVehicles.slice(
      this.visibleStartIndex,
      this.visibleStartIndex + this.visibleCount
    );
  }

  nextSlide() {
    if (
      this.visibleStartIndex + this.visibleCount <
      this.similarVehicles.length
    ) {
      this.visibleStartIndex++;
    }
  }

  prevSlide() {
    if (this.visibleStartIndex > 0) {
      this.visibleStartIndex--;
    }
  }

  canContinue(): boolean {
    return this.totalDays > 0 && this.termsAccepted;
  }

  proceedToCheckout(): void {
    this.checkoutDataService.clearCheckoutData();

    this.checkoutDataService.setCheckoutData({
      startTime: this.startTime,
      endTime: this.endTime,
      totalCost: this.totalCost,
      makeModel: this.vehicleData?.make+ ' ' + this.vehicleData?.model,
      vehicleId: this.vehicleData!.id,
    });

    this.router.navigate(['/order-checkout']);
  }
}
