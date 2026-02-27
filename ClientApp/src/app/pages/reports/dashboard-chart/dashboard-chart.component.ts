import { Component, OnInit, ViewChild } from '@angular/core';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-dashboard-chart',
  templateUrl: './dashboard-chart.component.html',
  styleUrls: ['./dashboard-chart.component.css']
})
export class DashboardChartComponent implements OnInit {

  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;

  // PIE CHART CONFIG (Social Category)
  public pieChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: { position: 'top' },
      title: { display: true, text: 'Social Category Distribution' }
    }
  };
  public pieChartType: ChartType = 'pie';
  public pieChartData: ChartData<'pie', number[], string | string[]> = {
    labels: [ 'SC', 'ST', 'OBC', 'General' ],
    datasets: [ { data: [ 0, 0, 0, 0 ] } ]
  };

  // BAR CHART CONFIG (Housing)
  public barChartOptions: ChartConfiguration['options'] = {
    responsive: true,
    plugins: {
      legend: { display: false }, // Hide legend since colors explain it
      title: { display: true, text: 'Rural Housing Coverage' }
    }
  };
  public barChartType: ChartType = 'bar';
  public barChartData: ChartData<'bar'> = {
    labels: [ 'Has Housing Scheme', 'No Housing Scheme' ],
    datasets: [
      { data: [ 0, 0 ], label: 'Households', backgroundColor: ['#22c55e', '#ef4444'] }
    ]
  };

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.loadData();
  }
get housingCoveragePercent(): number {
  // 1. Safely access the data array
  const dataset = this.barChartData?.datasets[0]?.data;

  if (!dataset || dataset.length < 2) return 0;

  // 2. Force conversion to Number (handles null/undefined safely)
  const hasHousing = Number(dataset[0]) || 0;
  const noHousing = Number(dataset[1]) || 0;
  const total = hasHousing + noHousing;

  // 3. Prevent divide by zero error
  if (total === 0) return 0;

  return (hasHousing / total) * 100;
}
  loadData() {
    this.http.get<any>(`${environment.apiUrl}/reports/dashboard-charts`).subscribe(res => {
      
      // Update Pie Chart Data
      this.pieChartData = {
        labels: [ 'SC', 'ST', 'OBC', 'General' ],
        datasets: [ {
          data: [ res.countSC, res.countST, res.countOBC, res.countGen ],
          backgroundColor: [ '#8b5cf6', '#f97316', '#3b82f6', '#9ca3af' ] // Purple, Orange, Blue, Gray
        } ]
      };

      // Update Bar Chart Data
      this.barChartData = {
        labels: [ 'Has Housing Scheme', 'No Housing Scheme' ],
        datasets: [
          { 
            data: [ res.hasHousing, res.noHousing ], 
            label: 'Households',
            backgroundColor: ['#22c55e', '#ef4444'] // Green, Red
          }
        ]
      };

      // Force chart re-render
      this.chart?.update();
    });
  }
}