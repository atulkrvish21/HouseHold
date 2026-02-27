import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { HouseholdReportService } from 'src/app/services/householdreport.service';
import { environment } from 'src/environments/environment';
@Component({
  selector: 'app-hhreport',
  templateUrl: './hhreport.component.html',
  styleUrls: ['./hhreport.component.css']
})
export class HHReportComponent {

reportList: any[] = [];
  totalRecords = 0;
  totalPages = 0;
  loading = false;

  // Filter State
  filter = {
    pageNumber: 1,
    pageSize: 20, // Audit screens usually show more rows
    sortColumn: 'EntryDate',
    sortDirection: 'DESC',
    
    // Audit Specifics
    fromDate: new Date(new Date().setDate(new Date().getDate() - 30)).toISOString().split('T')[0], // Last 30 Days
    toDate: new Date().toISOString().split('T')[0],
    entryBy: '',
    status: null, // null = All
    district: '',
    searchText: ''
  };
// Add this variable
previewImageUrl: string | null = null;

openImage(url: string) {
  this.previewImageUrl = url;
}

closeImage() {
  this.previewImageUrl = null;
}
  // Search Debouncer (Prevents API spamming)
  private searchSubject = new Subject<string>();

  constructor(private reportService: HouseholdReportService,private http: HttpClient) {}

  ngOnInit() {
    this.loadData();

    // Setup Debounce: Wait 500ms after user stops typing before calling API
    this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged()
    ).subscribe(() => {
      this.filter.pageNumber = 1; // Reset to page 1 on search
      this.loadData();
    });
  }

  // Called by HTML Inputs
  onFilterChange() {
    this.searchSubject.next(this.filter.searchText); // Trigger debounce
  }

  // Immediate update for Dropdowns/Dates (No debounce needed)
  onDirectFilterChange() {
    this.filter.pageNumber = 1;
    this.loadData();
  }

  onSort(column: string) {
    // Toggle Direction if clicking same column
    if (this.filter.sortColumn === column) {
      this.filter.sortDirection = this.filter.sortDirection === 'ASC' ? 'DESC' : 'ASC';
    } else {
      this.filter.sortColumn = column;
      this.filter.sortDirection = 'ASC';
    }
    this.loadData();
  }

  onPageChange(newPage: number) {
    if(newPage < 1 || newPage > this.totalPages) return;
    this.filter.pageNumber = newPage;
    this.loadData();
  }
// Add this property to your class
expandedId: string | null = null;
detailsCache: { [key: string]: any } = {}; // Caches loaded details to prevent re-fetching

// Toggle Logic
toggleExpand(uniqueId: string) {
  if (this.expandedId === uniqueId) {
    // Collapse if already open
    this.expandedId = null;
    return;
  }

  this.expandedId = uniqueId;

  // Lazy Load: Only fetch if we haven't fetched this ID before
  if (!this.detailsCache[uniqueId]) {
    this.loading = true; // Optional: Local loading state could be better
    this.http.get<any>(`${environment.apiUrl}/Household/${uniqueId}`).subscribe({
      next: (res) => {
        this.detailsCache[uniqueId] = res;
        console.log('Loaded details for', uniqueId, res);
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load details', err);
        this.loading = false;
      }
    });
  }
}
  loadData() {
    this.loading = true;
    this.reportService.getReport(this.filter).subscribe({
      next: (res: any) => {
        this.reportList = res.data;
        this.totalRecords = res.totalRecords;
        this.totalPages = res.totalPages;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }
  
  // Helper for Status Badge
  getStatusColor(status: number) {
    switch(status) {
        case 0: return 'bg-yellow-100 text-yellow-800'; // Pending
        case 1: return 'bg-green-100 text-green-800';   // Approved
        case 10: return 'bg-red-100 text-red-800';      // Rejected
        default: return 'bg-gray-100 text-gray-800';
    }
  }
  
  getStatusLabel(status: number) {
      if(status === 0) return 'Pending';
      if(status === 1) return 'Approved';
      if(status === 10) return 'Rejected';
      return 'Unknown';
  }
}
