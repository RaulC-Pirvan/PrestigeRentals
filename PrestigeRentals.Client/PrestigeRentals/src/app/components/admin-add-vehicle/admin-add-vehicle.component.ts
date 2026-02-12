import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, Output, QueryList, ViewChildren } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { VehicleService } from '../../services/vehicle.service';
import { HttpClient } from '@angular/common/http';
import { ButtonComponent } from "../../shared/button/button.component";

@Component({
  selector: 'app-admin-add-vehicle',
  imports: [CommonModule, ReactiveFormsModule, ButtonComponent],
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
  emptySlots = Array(9);
  totalSlots = Array(9);

  @ViewChildren('secondaryImageInput') secondaryImageInputs!: QueryList<ElementRef>;
  
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

onSecondaryImageSelected(event: any, index: number) {
  const file = event.target.files[0];
  if (file) {
    this.secondaryImageFiles[index] = file;

    const reader = new FileReader();
    reader.onload = () => {
      this.secondaryImagesPreview[index] = reader.result;
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
