import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HouseholdSearchFilter } from '../pages/hhapproval/hhapproval.component';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class HouseholdReportService {

  private approvalUrl = `${environment.apiUrl}/household/search-approval`;
  private searchUrl = `${environment.apiUrl}/household/search`;
  constructor(private http: HttpClient) {}

getPendingReport(filter: HouseholdSearchFilter): Observable<any[]> {
    return this.http.post<any[]>(this.approvalUrl, filter);
  }

  getReport(filter: HouseholdSearchFilter): Observable<any[]> {
    return this.http.post<any[]>(this.searchUrl, filter);
  }
}
