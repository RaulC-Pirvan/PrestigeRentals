import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { BannerWithButtonComponent } from '../../components/banner-with-button/banner-with-button.component';

@Component({
  selector: 'app-landing-page',
  imports: [NavbarComponent, FooterComponent, BannerWithButtonComponent],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.scss'
})
export class LandingPageComponent {

}
