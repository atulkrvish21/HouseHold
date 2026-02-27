import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { ClaimsService } from '../services/claims.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html'
})
export class HeaderComponent {
  @Output() toggleSidebar = new EventEmitter<void>();

  claims: any;
  nameClaim: string | null = null;
  roleClaim: string | null = null;
  givenNameClaim: string | null = null;
  isAdmin: boolean = false;
  constructor(private authService: AuthService, private router: Router, private claimsService: ClaimsService) {
    const token = this.authService.getToken();
    if (token) {
      this.claims = this.claimsService.getClaims(token);
      if (token) {
        this.nameClaim = this.claimsService.getNameClaim(token);
        this.roleClaim = this.claimsService.getRoleClaim(token);
        this.givenNameClaim = this.claimsService.getGivenClaim(token);
        if (this.roleClaim == "2") {
          this.isAdmin = true;
        }
        else {
          this.isAdmin = false;
        }
      }

    }
  }
}
