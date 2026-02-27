import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-panchayat-progress',
  templateUrl: './panchayat-progress.component.html',
  styleUrls: ['./panchayat-progress.component.css']
})
export class PanchayatProgressComponent implements OnInit {

  // Data State
  fullList: any[] = []; // Stores raw API data
  filteredList: any[] = []; // Stores data after search
  loading = false;
  searchText = '';

  // Footer Totals
 totals = {
  totalSubmitted: 0, // Changed from 'target'
  completed: 0,
  pending: 0,
  rejected: 0        // Added rejected
};

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.loading = true;
    this.http.get<any[]>(`${environment.apiUrl}/reports/survey-progress-panchayat`)
      .subscribe({
        next: (data) => {
          this.fullList = data;
          this.filteredList = data; // Initially, filtered list is same as full
          this.calculateTotals();
          this.loading = false;
        },
        error: (err) => {
          console.error('API Error:', err);
          this.loading = false;
        }
      });
  }

  // Client-side Filter Logic
  applyFilter() {
    const term = this.searchText.toLowerCase();
    
    this.filteredList = this.fullList.filter(item => 
      item.panchayat.toLowerCase().includes(term) || 
      item.block.toLowerCase().includes(term) ||
      item.district.toLowerCase().includes(term)
    );
    
    // Recalculate totals based on filtered view
    this.calculateTotals();
  }

calculateTotals() {
  this.totals.totalSubmitted = this.filteredList.reduce((sum, x) => sum + (x.TotalSubmitted || x.totalSubmitted || 0), 0);
  this.totals.completed = this.filteredList.reduce((sum, x) => sum + (x.Completed || x.completed || 0), 0);
  this.totals.pending = this.filteredList.reduce((sum, x) => sum + (x.Pending || x.pending || 0), 0);
  this.totals.rejected = this.filteredList.reduce((sum, x) => sum + (x.Rejected || x.rejected || 0), 0);
  
  console.log('Calculated Totals:', this.totals); // Debugging
}
  // Helper for Row Coloring
  getRowClass(percentage: number): string {
    if (percentage === 0) return 'bg-red-50'; // Highlight non-starters
    return '';
  }
}