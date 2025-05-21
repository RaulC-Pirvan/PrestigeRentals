import { Component, Input } from '@angular/core';
import { Order } from '../../models/order.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-order-preview',
  imports: [CommonModule],
  templateUrl: './order-preview.component.html',
  styleUrl: './order-preview.component.scss',
  standalone: true,
})
export class OrderPreviewComponent {
  @Input() order: any;
  @Input() orderNumber!: number;
}
