import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());
  private token: string | null = null;

  get isLoggedIn() {
    return this.loggedIn.asObservable();
  }

  login(token: string) {
    this.token = token;
    this.loggedIn.next(true);
    localStorage.setItem('token', token);
  }

  logout() {
    this.token = null;
    this.loggedIn.next(false);
    localStorage.removeItem('token');
  }

  getToken(): string | null {
    return this.token || localStorage.getItem('token');
  }
   private hasToken(): boolean {
    return !!localStorage.getItem('token');
  }
}
