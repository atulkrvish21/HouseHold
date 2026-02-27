import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

export interface ConnectionDetails {
  id: number;
  binderNumber: string;
  entryDate: string;
  aadhaarUAN: string;
  connectionsReceived: number;
  connectionType: string;
  meterNumber: string;
  pinNumber: string;
  siteImageFilePath: string;
  meterImageFilePath: string;
}

@Injectable({
  providedIn: 'root'
})
export class ConnectionService {
  constructor(private http: HttpClient) {}

  getFilteredConnections(): Observable<ConnectionDetails[]> {
    const params = {
      fromDate: this.filter.fromDate,
      toDate: this.filter.toDate,
      binderNumber: this.filter.binderNumber,
      meterNumber: this.filter.meterNumber
    };
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });

    return this.http.get<ConnectionDetails[]>(
      `${environment.apiUrl}/ConnectionDetails/filter`,
      { params, headers, observe: 'response' }
    ).pipe(
      map((response: HttpResponse<ConnectionDetails[]>) => {
        if (response.status === 200) {
          return response.body;
        } else {
          // Throw an error to be caught by catchError
          throw new Error(`Unexpected status code: ${response.status}`);
        }
      }),
      catchError(error => {
        // Log the error for debugging
        console.error('An error occurred:', error);

        // Return an empty array or handle the error as needed
        return throwError('An unexpected error occurred. Please try again later.');
      })
    );
  }
}
