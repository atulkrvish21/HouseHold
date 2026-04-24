import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { MigrationSurvey } from '../model/migration-survey.model';

@Injectable({
  providedIn: 'root'
})
export class MigrationSurveyService {
  private readonly migrationSurveyUrl = `${environment.apiUrl}/MigrationSurvey`;

  constructor(private http: HttpClient) {}

  getMigrationSurveys(): Observable<MigrationSurvey[]> {
    return this.http.get<MigrationSurvey[]>(this.migrationSurveyUrl);
  }

  getMigrationSurveyById(id: number): Observable<MigrationSurvey> {
    return this.http.get<MigrationSurvey>(`${this.migrationSurveyUrl}/${id}`);
  }
}
