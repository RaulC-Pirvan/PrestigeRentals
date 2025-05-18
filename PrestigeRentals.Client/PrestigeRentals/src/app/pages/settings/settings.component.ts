import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { DetailsSettingsFormComponent } from '../../components/details-settings-form/details-settings-form.component';

@Component({
  selector: 'app-settings',
  imports: [NavbarComponent, FooterComponent, DetailsSettingsFormComponent],
  templateUrl: './settings.component.html',
  styleUrl: './settings.component.scss'
})
export class SettingsComponent {

}
