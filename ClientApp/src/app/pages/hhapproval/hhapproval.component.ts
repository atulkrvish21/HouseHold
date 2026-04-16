import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Token } from '@angular/compiler';
import { AuthService } from 'src/app/services/auth.service';
import { NotificationService } from 'src/app/services/notification.service';
import { HouseholdReportService } from 'src/app/services/householdreport.service';
import { ClaimsService } from 'src/app/services/claims.service';


@Component({
  selector: 'app-hhapproval',
  templateUrl: './hhapproval.component.html',
  styleUrls: ['./hhapproval.component.css']
})
export class HHapprovalComponent implements OnInit {

  loading = false;
  errorMessage = '';
  claims: any;
  nameClaim: string | null = null;

  // Modal State
  householdStatus!: householdStatusChange;
  showRejectModal = false;
  selectedId: string | null = null;
  rejectionReason = '';

  // Data State
  reportList: HouseholdReport[] = []; // Holds the current page data
  
  // Pagination State
  totalRecords = 0;
  totalPage = 0;
  pageSize = 10;
  currentPage = 1;

  // Filters
  searchText = '';
  filter: HouseholdSearchFilter = {
    fromDate: '2025-01-01',
    toDate: new Date().toLocaleDateString('en-CA'),
    pageNumber: 1, // Initialize hardcoded, will update dynamically
    pageSize: 10
  };

  // View Modal State
  showViewModal = false;
  selectedHousehold: any = null;

  constructor(
    private http: HttpClient,
    private router: Router,
    private authService: AuthService,
    private notificationService: NotificationService,
    private reportService: HouseholdReportService,
    private claimsService: ClaimsService
  ) {
    const token = this.authService.getToken();
    if (token) {
      this.claims = this.claimsService.getClaims(token);
      this.nameClaim = this.claimsService.getNameClaim(token);
    }
  }

  ngOnInit(): void {
    this.loadReport();
  }
Edit(id: string) {
  if (!id) return;

  // 1. Close the modal so it's not stuck open if they come back
  this.closeViewModal(); 

  // 2. Navigate to the Entry Page with the ID
  // Make sure your route is defined as 'household-entry/:id'
  this.router.navigate(['/household-entry', id]); 
}
// Add this variable
previewImageUrl: string | null = null;

openImage(url: string) {
  this.previewImageUrl = url;
}

closeImage() {
  this.previewImageUrl = null;
}


  loadReport() {
    this.loading = true;

    // ✅ Crucial: Ensure the filter object has the current page before sending
    this.filter.pageNumber = this.currentPage;
    this.filter.pageSize = this.pageSize;

    this.reportService.getPendingReport(this.filter).subscribe({
      next: (res: any) => {
        // Handle API variations (Response wrapper vs Direct Array)
        const dataArray = Array.isArray(res) ? res : res.data;

        if (!dataArray) {
          console.error("API Error: No data found", res);
          this.reportList = [];
          this.loading = false;
          return;
        }

        // Map Data
        this.reportList = dataArray.map((x: any) => ({
          uniqueId: x.householdBasicProfile?.uniqueId,
          district: x.householdBasicProfile?.district,
          block: x.householdBasicProfile?.block,
          gp: x.householdBasicProfile?.gramPanchayat,
          village: x.householdBasicProfile?.revenueVillage,
          headName: x.householdBasicProfile?.headOfTheHouseholdNameAsPerAadhar,
          category: x.householdBasicProfile?.socialCategory,
          totalMembers: x.householdBasicProfile?.totalFamilyMembers,
          hasRationCard: x.householdEntitlement.hasRationCard,
          hasLPG: x.householdEntitlement.hasUjjwalaLPGConnection,
          hasElectricity: x.householdEntitlement.hasElectricityConnection,
          entryDate: x.householdBasicProfile.entryDate?.split('T')[0]
        }));

        // Update Pagination Stats from Server
        this.totalRecords = res.totalRecords || 0;
        this.totalPage = res.totalPages || 0;
        this.currentPage = res.pageNumber || this.currentPage; // Use server page or fallback

        // If user was searching locally, re-apply filter (Optional UX choice)
        if(this.searchText) {
           this.applyFilter(); 
        }

        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  // ✅ Fixed Pagination Logic
  onPageChange(newPage: number) {
    // 1. Check Bounds
    if (newPage < 1 || newPage > this.totalPage) return;

    // 2. Update State
    this.currentPage = newPage;
    
    // 3. Update Filter Object
    this.filter.pageNumber = newPage;

    // 4. Fetch Data from Server
    this.loadReport();
  }

  // ⚠️ Note: This filters ONLY the current page data (10 records). 
  // For full search, you need backend support (sending searchText in API).
  applyFilter() {
    if (!this.searchText) {
      // If search cleared, reload original page data logic? 
      // For now, we just don't filter the current view.
      return; 
    }
    
    const val = this.searchText.toLowerCase();
    
    // We filter the visible list locally
    // If you reload page, this gets overwritten by loadReport
    this.reportList = this.reportList.filter(h =>
      (h.uniqueId && h.uniqueId.toLowerCase().includes(val)) ||
      (h.headName && h.headName.toLowerCase().includes(val)) ||
      (h.block && h.block.toLowerCase().includes(val))
    );
  }

  // ... [Keep exportCSV, viewDetails, approve, reject, modal logic exactly as they were] ...
printPage() {
  window.print();
}
  // 1. Triggered by the Reject button in the table
  openRejectConfirm(id: string) {
    this.selectedId = id;
    this.showRejectModal = true;
  }

  closeModal() {
    this.showRejectModal = false;
    this.selectedId = null;
    this.rejectionReason = '';
  }

  confirmReject() {
    if (this.selectedId) {
      this.reject(this.selectedId);
      this.closeModal();
    }
  }

  // Export CSV
  exportCSV() {
    if(!this.reportList || this.reportList.length === 0) return;
    const header = Object.keys(this.reportList[0]).join(',');
    const rows = this.reportList.map(r => Object.values(r).join(','));
    const csv = [header, ...rows].join('\n');
    const blob = new Blob([csv], { type: 'text/csv' });
    const a = document.createElement('a');
    a.href = URL.createObjectURL(blob);
    a.download = 'household-report.csv';
    a.click();
  }

  // ... View, Approve, Reject logic (Keep as is) ...
  viewDetails(id: string) {
    this.loading = true;
    this.http.get<any>(`${environment.apiUrl}/household/${id}`).subscribe({
      next: (data) => {
        this.selectedHousehold = data;
        this.showViewModal = true;
        this.loading = false;
        console.log('Loaded household details', data);
      },
      error: (err) => {
        this.notificationService.show('Failed to load full details', 'error');
        this.loading = false;
      }
    });
  }

  closeViewModal() {
    this.showViewModal = false;
    this.selectedHousehold = null;
  }

  approve(id: string) {
    this.loading = true;
    const payload: householdStatusChange = {
      sstatus: 1,
      approvedBy: this.nameClaim || '',
      reason: "",
      uniqueId: id
    };

    this.http.post<any>(`${environment.apiUrl}/household/update-status`, payload)
      .subscribe({
        next: (res) => {
          if(res.success) {
             this.notificationService.show('Household approved successfully', 'success');
             this.loadReport(); // ✅ Reload data to refresh table/pagination
             this.closeViewModal();
          } else { 
             this.notificationService.show(res.message, 'info');
             this.loading = false;
          }
        },
        error: (err) => {
          this.notificationService.show('Approval failed.', 'error');
          this.loading = false;
        }
      });
  }

  reject(id: string) {
    this.loading = true;
    const payload: householdStatusChange = {
      sstatus: 10,
      approvedBy: this.nameClaim || '',
      reason: this.rejectionReason,
      uniqueId: id
    };

    this.http.post<any>(`${environment.apiUrl}/household/update-status`, payload)
      .subscribe({
        next: (res) => {
          if(res.success) {
            this.notificationService.show('Household rejected', 'success');
            this.loadReport(); // ✅ Reload data
            this.closeViewModal()
          } else { 
            this.notificationService.show(res.message, 'info');
             this.loading = false;
          }
        },
        error: (err) => {
          this.notificationService.show('Rejection failed.', 'error');
          this.loading = false;
        }
      });
  }
}




// household-report.model.ts
export interface HouseholdReport {
  uniqueId: string;
  district: string;
  block: string;
  gp: string;
  village:string;
  headName: string;
  category: string;
  totalMembers: number;
  hasRationCard: boolean;
  hasLPG: boolean;
  hasElectricity: boolean;
  entryDate: string;
}



export interface householdStatusChange {
  uniqueId: string;
  reason: string;
  sstatus: number;
  approvedBy: string;
}


export interface HouseholdSearchFilter {
  fromDate: string;        // mandatory
  toDate: string;          // mandatory

  district?: string;
  block?: string;
  gramPanchayat?: string;
  revenueVillage?: string;

  socialCategory?: string;

  hasRationCard?: boolean;
  isCoveredUnderSHG?: boolean;

  entryBy?: string;

  pageNumber?: number;
  pageSize?: number;
}