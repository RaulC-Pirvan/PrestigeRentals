import { HttpClient } from '@angular/common/http';
import {
  Component,
  ElementRef,
  OnInit,
  QueryList,
  ViewChildren,
} from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ButtonComponent } from '../../shared/button/button.component';
import { CommonModule } from '@angular/common';
import { TitleComponent } from '../../shared/title/title.component';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-admin-edit-vehicle',
  standalone: true,
  imports: [ButtonComponent, CommonModule, ReactiveFormsModule, TitleComponent],
  templateUrl: './admin-edit-vehicle.component.html',
  styleUrls: ['./admin-edit-vehicle.component.scss'],
})
export class AdminEditVehicleComponent implements OnInit {
  modifyForm!: FormGroup;
  mainImagePreview: string | null = null;
  secondaryImagesPreview: string[] = [];
  vehicleId!: number;
  mainImageFile: File | null = null;
  secondaryImageFiles: File[] = [];
  totalSlots = Array(9);

  @ViewChildren('secondaryImageInput')
  secondaryImageInputs!: QueryList<ElementRef>;

  constructor(
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private notificationService: NotificationService
  ) {}

  ngOnInit(): void {
    this.vehicleId = +this.route.snapshot.paramMap.get('id')!;
    this.initForm();
    this.loadVehicleData();
  }

  initForm(): void {
    this.modifyForm = this.fb.group({
      make: ['', Validators.required],
      model: ['', Validators.required],
      chassis: [''],
      horsepower: [''],
      engineSize: [''],
      fuelType: [''],
      transmission: [''],
      pricePerDay: ['', Validators.required],
      navigation: [false],
      headsUpDisplay: [false],
      hillAssist: [false],
      cruiseControl: [false],
    });
  }

  loadVehicleData(): void {
    this.http
      .get<any>(`https://localhost:7093/api/vehicle/${this.vehicleId}`)
      .subscribe((vehicle) => {
        this.modifyForm.patchValue(vehicle);
      });

    this.http
      .get<any>(`https://localhost:7093/api/vehicle/${this.vehicleId}/options`)
      .subscribe((options) => {
        this.modifyForm.patchValue(options);
      });
    this.http
      .get(`https://localhost:7093/api/image/vehicle/${this.vehicleId}/main`, {
        responseType: 'blob',
      })
      .subscribe(
        (blob) => {
          const reader = new FileReader();
          reader.onload = () => {
            this.mainImagePreview = reader.result as string;
          };
          reader.readAsDataURL(blob);
        },
        (err) => {
          console.error('Nu s-a putut încărca imaginea principală', err);
          this.mainImagePreview = null;
        }
      );
  }

  submit(): void {
    if (this.modifyForm.invalid) {
      this.notificationService.show('The form contains errors', 'error');
      return;
    }

    this.http
      .patch(
        `https://localhost:7093/api/vehicle/${this.vehicleId}`,
        this.modifyForm.value
      )
      .subscribe({
        next: () => {
          console.log('Vehicul modificat cu succes!');
          this.notificationService.show(
            'Successfully modified the vehicle',
            'success'
          );
          this.router.navigate(['/admin-dashboard/vehicles']);
        },
        error: (err) => {
          this.notificationService.show(
            'Failed to modify the vehicle',
            'error'
          );
          console.error('Eroare la update:', err);
        },
      });
  }

  onCancel(): void {
    this.router.navigate(['/admin-dashboard/vehicles']);
  }
}
