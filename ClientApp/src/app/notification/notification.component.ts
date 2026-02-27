
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Notification, NotificationService } from '../services/notification.service' // Adjust path as needed
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-notification',
  template: `
    <div *ngIf="showNotification" [ngClass]="['notification-popup', notification.type]">
      {{ notification.message }}
    </div>
  `,
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit, OnDestroy {
  notification: Notification = { message: '', type: 'info' };
  showNotification = false;
  isExiting = false; 
  private destroy$ = new Subject<void>();
  private timeoutId: any;
  private exitTimeoutId: any;

  constructor(private notificationService: NotificationService) { }

  ngOnInit(): void {
    this.notificationService.notification$
      .pipe(takeUntil(this.destroy$))
      .subscribe(notification => {
    
       

        if (this.timeoutId) {
          clearTimeout(this.timeoutId);
        }
        if (this.exitTimeoutId) {
          clearTimeout(this.exitTimeoutId);
        }
        this.notification = notification;
        this.showNotification = true;
        this.isExiting = false;

        this.timeoutId = setTimeout(() => {
          this.isExiting = true; // Start the fade-out animation
          // Set another timeout for the fade-out animation duration
          this.exitTimeoutId = setTimeout(() => {
            this.showNotification = false; // Finally remove from DOM after fade-out
          }, 500); // This should match your CSS fade-out animation duration (0.5s)
        }, notification.duration || 3000);

      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    if (this.timeoutId) {
      clearTimeout(this.timeoutId);
    }
    if (this.exitTimeoutId) {
      clearTimeout(this.exitTimeoutId);
    }
  }
}
