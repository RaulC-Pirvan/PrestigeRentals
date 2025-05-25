import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { TitleComponent } from '../../shared/title/title.component';
import { ButtonComponent } from '../../shared/button/button.component';
import { VehicleCardComponent } from '../../components/vehicle-card/vehicle-card.component';
import { VehiclesListComponent } from '../../components/vehicles-list/vehicles-list.component';

@Component({
  selector: 'app-inventory',
  imports: [NavbarComponent, FooterComponent, TitleComponent, ButtonComponent, VehiclesListComponent],
  templateUrl: './inventory.component.html',
  styleUrl: './inventory.component.scss'
})
export class InventoryComponent {
  
}
