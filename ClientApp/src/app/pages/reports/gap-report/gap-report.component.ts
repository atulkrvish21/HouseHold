import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-gap-report',
  templateUrl: './gap-report.component.html',
  styleUrls: ['./gap-report.component.css']
})
export class GapReportComponent implements OnInit {

  gapList: any[] = [];
  filteredList: any[] = [];
  searchText = '';
  loading = false;

  totals = {
    totalHH: 0,
    noRation: 0,
    noHousing: 0,
    noToilet: 0,
    womenExcluded: 0
  };

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.http.get<any[]>(`${environment.apiUrl}/reports/entitlement-gap`).subscribe({
      next: (data) => {
        this.gapList = data;
        this.filteredList = data;
        this.calculateTotals();
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  applyFilter() {
    const term = this.searchText.toLowerCase();
    this.filteredList = this.gapList.filter(item => 
      item.panchayat.toLowerCase().includes(term) ||
      item.block.toLowerCase().includes(term)
    );
    this.calculateTotals();
  }

  calculateTotals() {
    this.totals.totalHH = this.filteredList.reduce((sum, x) => sum + x.totalHouseholds, 0);
    this.totals.noRation = this.filteredList.reduce((sum, x) => sum + x.noRationCard, 0);
    this.totals.noHousing = this.filteredList.reduce((sum, x) => sum + x.noHousing, 0);
    this.totals.noToilet = this.filteredList.reduce((sum, x) => sum + x.noToilet, 0);
    this.totals.womenExcluded = this.filteredList.reduce((sum, x) => sum + x.womenNotInSHG, 0);
  }
}