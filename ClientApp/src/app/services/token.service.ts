import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { catchError, retry, tap, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private refreshTokenEndpoint = `${environment.apiUrl}/refresh-token`;
  private tokenSubject: BehaviorSubject<string> = new BehaviorSubject<string>('');
  public token$: Observable<string> = this.tokenSubject.asObservable();

  constructor(private http: HttpClient) {
    const token = localStorage.getItem('token');
    if (token) {
      this.tokenSubject.next(token);
    }
  }

  setToken(token: string) {
    localStorage.setItem('token', token);
    this.tokenSubject.next(token);
  }

  getToken(): string {
    return this.tokenSubject.value;
  }

  clearToken() {
    localStorage.removeItem('token');
    this.tokenSubject.next('');
  }

  refreshToken(): Observable<string> {
    const refreshToken = localStorage.getItem('refreshToken');
    if (!refreshToken) {
      return throwError('No refresh token available');
    }

    return this.http.post<string>(this.refreshTokenEndpoint, { refreshToken }).pipe(
      tap((newToken) => {
        this.setToken(newToken);
      }),
      catchError((error) => {
        this.clearToken();
        return throwError(error);
      })
    );
  }

  checkTokenExpiration(): Observable<boolean> {
    const token = this.getToken();
    if (!token) {
      return throwError('No token available');
    }

    // Assuming the token has an expiration time in the payload
    const payload = JSON.parse(atob(token.split('.')[1]));
    const expirationTime = payload.exp * 1000; // Convert to milliseconds
    const currentTime = new Date().getTime();

    if (expirationTime - currentTime < 60000) { // Refresh token if it expires in less than 1 minute
      return this.refreshToken().pipe(
        map(() => {
          return true;
        })
      );
    }

    return throwError('Token is still valid');
  }
}
