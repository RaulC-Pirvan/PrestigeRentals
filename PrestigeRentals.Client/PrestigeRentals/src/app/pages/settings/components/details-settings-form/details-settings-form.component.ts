import { Component } from '@angular/core';
import { ButtonComponent } from '../../../../shared/button/button.component';


@Component({
  selector: 'app-details-settings-form',
  imports: [ ButtonComponent],
  templateUrl: './details-settings-form.component.html',
  styleUrls: ['./details-settings-form.component.scss'],
})
export class DetailsSettingsFormComponent {}
