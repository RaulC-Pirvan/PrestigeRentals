import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { VehicleService } from '../../services/vehicle.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-admin-add-vehicle',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './admin-add-vehicle.component.html',
  styleUrls: ['./admin-add-vehicle.component.scss'],
})
export class AdminAddVehicleComponent {
  @Output() vehicleAdded = new EventEmitter<{
    vehicleData: any;
    mainImageFile?: File;
    secondaryImageFiles: File[];
  }>();
  @Output() cancel = new EventEmitter<void>();

  addVehicleForm: FormGroup;

  mainImageFile?: File;
  mainImagePreview: string | ArrayBuffer | null = null;

  secondaryImageFiles: File[] = [];
  secondaryImagesPreview: (string | ArrayBuffer | null)[] = [];

  constructor(
    private fb: FormBuilder,
    private vehicleService: VehicleService,
    private http: HttpClient
  ) {
    this.addVehicleForm = this.fb.group({
      make: ['', Validators.required],
      model: ['', Validators.required],
      chassis: ['', Validators.required],
      pricePerDay: [0, Validators.required],
      horsepower: [0, Validators.required],
      engineSize: [0, Validators.required],
      fuelType: ['', Validators.required],
      transmission: ['', Validators.required],
      navigation: [false],
      headsUpDisplay: [false],
      hillAssist: [false],
      cruiseControl: [false],
    });
  }

  ngOnInit(): void {
    console.log('AdminAddVehicleComponent loaded');
  }

  onMainImageSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.mainImageFile = file;

      const reader = new FileReader();
      reader.onload = () => (this.mainImagePreview = reader.result);
      reader.readAsDataURL(file);
    }
  }

  onSecondaryImagesSelected(event: any) {
    const files: FileList = event.target.files;
    this.secondaryImageFiles = [];
    this.secondaryImagesPreview = [];

    for (let i = 0; i < files.length; i++) {
      const file = files[i];
      this.secondaryImageFiles.push(file);

      const reader = new FileReader();
      reader.onload = () => {
        this.secondaryImagesPreview.push(reader.result);
      };
      reader.readAsDataURL(file);
    }
  }

  submit() {
    if (!this.addVehicleForm.valid) return;

    this.vehicleAdded.emit({
      vehicleData: this.addVehicleForm.value,
      mainImageFile: this.mainImageFile,
      secondaryImageFiles: this.secondaryImageFiles,
    });

    this.addVehicleForm.reset();
    this.mainImageFile = undefined;
    this.secondaryImageFiles = [];
    this.mainImagePreview = null;
    this.secondaryImagesPreview = [];
  }

  onCancel() {
    this.cancel.emit();
  }
}
