import { Component } from '@angular/core';
import { ButtonComponent } from '../../shared/button/button.component';
import { TitleComponent } from '../../shared/title/title.component';
import { TicketService } from '../../services/ticket.service';
import { CreateTicketRequest } from '../../models/create-ticket-request.model';
import { NgForm, ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { NotificationService } from '../../services/notification.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-image-form',
  standalone: true,
  imports: [ButtonComponent, TitleComponent, ReactiveFormsModule, FormsModule, CommonModule],
  templateUrl: './image-form.component.html',
  styleUrl: './image-form.component.scss'
})
export class ImageFormComponent {
  request: CreateTicketRequest = {
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    description: ''
  };

  constructor(private ticketService: TicketService, private notificationService: NotificationService) {}

onSubmit(form: NgForm) {
  if (form.invalid) {
    this.notificationService.show('Please fill out all required fields correctly.', 'error');
    return;
  }

  this.ticketService.submitTicket(this.request).subscribe({
    next: () => {
      this.notificationService.show('Ticket submitted successfully!', 'success');
      form.resetForm(); 
    },
    error: () => {
      this.notificationService.show('Failed to submit ticket.', 'error');
    }
  });
}
}
