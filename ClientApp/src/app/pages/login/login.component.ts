import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

import { environment } from 'src/environments/environment';
import { Token } from '@angular/compiler';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html'
})
export class LoginComponent {

  model = {
    username: '',
    password: ''
  };

  loading = false;
  errorMessage = '';

  constructor(
    private http: HttpClient,
    private router: Router,
    private authService: AuthService, private notificationService: NotificationService
  ) {

    if(authService.isLoggedIn){
       this.router.navigate(['/dashboard']);
    }
  }

  login() {
    this.loading = true;
    this.errorMessage = '';

    this.http.post<any>(`${environment.apiUrl}/auth/login`, this.model)
      .subscribe({
        next: (res) => {
          localStorage.setItem('token', res.token);
         this.authService.login(res.token);
          this.loading = false;
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.notificationService.show('Login failed. Please check your credentials.', 'error');
          this.errorMessage = err.error?.message || 'Login failed';
          this.loading = false;
        }
      });
  }
}
