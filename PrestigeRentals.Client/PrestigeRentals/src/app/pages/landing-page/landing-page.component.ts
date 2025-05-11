import { Component } from '@angular/core';
import { NavbarComponent } from '../../components/navbar/navbar.component';
import { FooterComponent } from '../../components/footer/footer.component';
import { BannerWithButtonComponent } from '../../components/banner-with-button/banner-with-button.component';
import { ThreeItemShowcaseComponent } from '../../components/three-item-showcase/three-item-showcase.component';
import { HalfPhotoShowcaseComponent } from '../../components/half-photo-showcase/half-photo-showcase.component';
import { TextShowcaseComponent } from '../../components/text-showcase/text-showcase.component';

@Component({
  selector: 'app-landing-page',
  imports: [NavbarComponent, FooterComponent, BannerWithButtonComponent, ThreeItemShowcaseComponent, HalfPhotoShowcaseComponent, TextShowcaseComponent],
  templateUrl: './landing-page.component.html',
  styleUrl: './landing-page.component.scss'
})
export class LandingPageComponent {

}
