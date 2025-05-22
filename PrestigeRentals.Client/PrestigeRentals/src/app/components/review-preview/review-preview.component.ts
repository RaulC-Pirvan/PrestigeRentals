import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-review-preview',
  imports: [],
  templateUrl: './review-preview.component.html',
  styleUrl: './review-preview.component.scss',
  standalone: true,
})
export class ReviewPreviewComponent {
  @Input() review: any;
  @Input() reviewNumber!: number;
}
