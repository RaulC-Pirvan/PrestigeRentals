import { Component, Input } from '@angular/core';
import { Ticket } from '../../../../models/ticket.model';

@Component({
  selector: 'app-admin-ticket-overview',
  imports: [],
  templateUrl: './admin-ticket-overview.component.html',
  styleUrl: './admin-ticket-overview.component.scss',
})
export class AdminTicketOverviewComponent {
  @Input() ticket!: Ticket;
}
