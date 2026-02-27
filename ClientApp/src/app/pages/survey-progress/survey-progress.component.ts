import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-survey-progress',
  templateUrl: './survey-progress.component.html',
  styleUrls: ['./survey-progress.component.css']
})
export class SurveyProgressComponent implements OnInit {

  summaryList: any[] = [];
  loading = false;

  // Aggregated Totals for the Footer
  totalTarget = 0;
  totalCompleted = 0;
  totalPending = 0;

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.http.get<any[]>(`${environment.apiUrl}/household/survey-progress`).subscribe({
      next: (data) => {
        this.summaryList = data;
        this.calculateTotals();
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  calculateTotals() {
    this.totalTarget = this.summaryList.reduce((sum, item) => sum + item.targetHouseholds, 0);
    this.totalCompleted = this.summaryList.reduce((sum, item) => sum + item.completedSurveys, 0);
    this.totalPending = this.summaryList.reduce((sum, item) => sum + item.pendingApproval, 0);
  }

  // Color helper for progress bar
  getProgressColor(percentage: number): string {
    if (percentage < 30) return 'bg-red-500';
    if (percentage < 70) return 'bg-yellow-500';
    return 'bg-green-500';
  }
}