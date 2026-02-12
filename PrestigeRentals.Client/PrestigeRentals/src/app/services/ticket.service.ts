import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateTicketRequest } from '../models/create-ticket-request.model';
import { Observable } from 'rxjs';
import { Ticket } from '../models/ticket.model';

@Injectable({
  providedIn: 'root',
})
export class TicketService {
  private readonly baseUrl = 'https://localhost:7093/api/ticket';

  constructor(private http: HttpClient) {}

  submitTicket(request: CreateTicketRequest): Observable<any> {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');

    if (!token) {
      throw new Error('No auth token found');
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http.post(this.baseUrl, request, { headers });
  }

  getAllTickets(): Observable<Ticket[]> {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');

    if (!token) {
      throw new Error('No auth token found');
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http.get<Ticket[]>(this.baseUrl, { headers });
  }
}
