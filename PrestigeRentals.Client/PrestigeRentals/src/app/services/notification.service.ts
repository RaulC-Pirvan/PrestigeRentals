import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

export type NotificationType = 'success' | 'error';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private messageSubject = new BehaviorSubject<string | null>(null);
  private typeSubject = new BehaviorSubject<NotificationType>('success');

  get message$(): Observable<string | null> {
    return this.messageSubject.asObservable();
  }

  get type$(): Observable<NotificationType> {
    return this.typeSubject.asObservable();
  }

  show(message: string, type: NotificationType = 'success', duration = 1000) {
    this.messageSubject.next(message);
    this.typeSubject.next(type);

    setTimeout(() => {
      this.messageSubject.next(null);
    }, duration);
  }
}
