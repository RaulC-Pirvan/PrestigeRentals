import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { ImageFormComponent } from '../../components/image-form/image-form.component';

@Component({
  selector: 'app-contact',
  imports: [NavbarComponent, FooterComponent, ImageFormComponent],
  templateUrl: './contact.component.html',
  styleUrl: './contact.component.scss'
})
export class ContactComponent {

}
