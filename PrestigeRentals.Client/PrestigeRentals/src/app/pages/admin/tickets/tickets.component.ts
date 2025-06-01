import { Component } from '@angular/core';
import { Ticket } from '../../../models/ticket.model';
import { TicketService } from '../../../services/ticket.service';
import { TitleComponent } from "../../../shared/title/title.component";
import { AdminTicketOverviewComponent } from "../components/admin-ticket-overview/admin-ticket-overview.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-tickets',
  imports: [TitleComponent, AdminTicketOverviewComponent, CommonModule],
  templateUrl: './tickets.component.html',
  styleUrl: './tickets.component.scss',
})
export class TicketsComponent {
  allTickets: Ticket[] = [];
  displayedTickets: Ticket[] = [];

  currentPage: number = 1;
  itemsPerPage: number = 5;

  constructor(private ticketService: TicketService) {}

  ngOnInit(): void {
    this.loadTickets();
  }

  loadTickets(): void {
    this.ticketService.getAllTickets().subscribe({
      next: (tickets) => {
        this.allTickets = tickets;
        this.updateDisplayedTickets();
      },
      error: (err) => console.error('Error loading tickets', err),
    });
  }

  updateDisplayedTickets(): void {
    const start = (this.currentPage - 1) * this.itemsPerPage;
    const end = start + this.itemsPerPage;
    this.displayedTickets = this.allTickets.slice(start, end);
  }

  changePage(offset: number): void {
    const newPage = this.currentPage + offset;
    if (newPage > 0 && newPage <= this.totalPages()) {
      this.currentPage = newPage;
      this.updateDisplayedTickets();
    }
  }

  totalPages(): number {
    return Math.ceil(this.allTickets.length / this.itemsPerPage);
  }
}
