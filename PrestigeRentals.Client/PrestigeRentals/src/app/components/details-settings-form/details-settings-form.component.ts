import { Component } from '@angular/core';
import { TitleComponent } from '../../shared/title/title.component';
import { ButtonComponent } from '../../shared/button/button.component';

@Component({
  selector: 'app-details-settings-form',
  imports: [TitleComponent, ButtonComponent],
  templateUrl: './details-settings-form.component.html',
  styleUrl: './details-settings-form.component.scss',
})
export class DetailsSettingsFormComponent {}
