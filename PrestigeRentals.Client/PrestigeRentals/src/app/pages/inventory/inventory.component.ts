import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { TitleComponent } from '../../shared/title/title.component';
import { ButtonComponent } from '../../shared/button/button.component';
import { VehiclesListComponent } from '../../components/vehicles-list/vehicles-list.component';
import {
  VehicleFilterOptions,
  VehicleService,
} from '../../services/vehicle.service';

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    NavbarComponent,
    FooterComponent,
    TitleComponent,
    ButtonComponent,
    VehiclesListComponent,
  ],
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.scss'],
})
export class InventoryComponent implements OnInit {
  filterForm: FormGroup;

  filters: any = {};
  searchTerm: string = '';
  sortBy: string = '';

  makes: string[] = [];
  models: string[] = [];
  fuelTypes: string[] = [];
  transmissions: string[] = [];
  chassis: string[] = [];

  constructor(private fb: FormBuilder, private vehicleService: VehicleService) {
    this.filterForm = this.fb.group({
      make: [''],
      model: [''],
      fuel: [''],
      transmission: [''],
      chassis: [''],
    });
  }

  ngOnInit(): void {
    this.vehicleService
      .getFilterOptions()
      .subscribe((options: VehicleFilterOptions) => {
        this.makes = options.makes;
        this.models = options.models;
        this.fuelTypes = options.fuelTypes;
        this.transmissions = options.transmissions;
        this.chassis = options.chassis;
      });
  }

  applyFilters(): void {
    this.filters = this.filterForm.value;
    console.log('Applying filters:', this.filters);
    this.filters = this.filters; // Make sure you update filters property so vehicle list updates
  }

  resetFilters(): void {
    this.filterForm.reset();
    this.filters = {};
  }

  // ✅ These two methods must exist to avoid template errors
  updateSearch(event: Event): void {
    const input = event.target as HTMLInputElement | null;
    this.searchTerm = input?.value ?? '';
  }

  updateSort(event: Event): void {
    const select = event.target as HTMLSelectElement | null;
    this.sortBy = select?.value ?? '';
  }
}
