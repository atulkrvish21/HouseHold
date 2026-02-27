import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { DashboardService, DashboardStats } from 'src/app/services/dashboard.service';
import { ClaimsService } from 'src/app/services/claims.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  stats: DashboardStats | null = null;
  claims: any;
  nameClaim: string | null = null;
  roleClaim: string | null = null;
  givenNameClaim: string | null = null;
  isAdmin: boolean = false;
  loading = true;
  error = '';

  constructor(
    private dashboardService: DashboardService,
    private router: Router
  ) {
 
}

  ngOnInit(): void {
    this.loadStats();
    console.log("total" 
      + this.stats);
  }

  loadStats() {
    this.dashboardService.getStats().subscribe({
      next: (res) => {
        console.log(res);
        this.stats = res;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load dashboard data';
        this.loading = false;
      }
    });
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}