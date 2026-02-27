import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';
export interface Notification {
  message: string;
  type: 'success' | 'error' | 'info';
  duration?: number; // Optional duration in milliseconds
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private notificationSubject = new Subject<Notification>();
  notification$: Observable<Notification> = this.notificationSubject.asObservable();

  constructor() { }

  show(message: string, type: 'success' | 'error' | 'info' = 'info', duration: number = 3000) {

    this.notificationSubject.next({ message, type, duration });
  }
}
