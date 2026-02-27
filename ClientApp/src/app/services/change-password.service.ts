import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ChangePasswordModel } from '../change-password/change-password.component';

@Injectable({
  providedIn: 'root'
})
export class ChangePasswordService {

  private apiUrl = `${environment.apiUrl}/auth/change-password`; // Adjust API URL as needed

  constructor(private http: HttpClient) { }

  changePassword(data: ChangePasswordModel): Observable<any> {
    // Standard practice: retrieve token from storage and add to headers
    const token = localStorage.getItem('token');

    // IMPORTANT: Assuming the user is authenticated via a JWT or similar mechanism
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
    console.log("TOKEN " + token);
    // Send a POST request with the password data
    // Use an interface for the response if possible (e.g., Observable<ChangePasswordResponse>)
    return this.http.post(this.apiUrl, data, { headers });
  }
}
