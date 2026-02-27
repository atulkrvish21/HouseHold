import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface DashboardStats {
  totalHousehold: number;
  approved: number;
  pending: number;
  totalHouseholdFTM: number;
  approvedFTM: number;
  pendingFTM: number;
}

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  private apiUrl = `${environment.apiUrl}/dashboard`;

  constructor(private http: HttpClient) {}

  getStats(): Observable<DashboardStats> {
    return this.http.get<DashboardStats[]>(`${this.apiUrl}/stat`).pipe(
      map(res => res[0])   // 👈 array ka first object
    );;
  }
}
