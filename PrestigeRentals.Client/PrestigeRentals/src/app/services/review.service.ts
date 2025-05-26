import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Review } from '../models/review.model';

@Injectable({
  providedIn: 'root',
})
export class ReviewService {
  private baseUrl = 'https://localhost:7093/api/review/user';

  constructor(private http: HttpClient) {}

  getReviewsForCurrentUser(): Observable<Review[]> {
    const token =
      localStorage.getItem('authToken') || sessionStorage.getItem('authToken');

    if (!token) {
      throw new Error('No auth token found');
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http.get<Review[]>(this.baseUrl, { headers });
  }


}
