import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  @Input() isOpen = true;
  @Input() isAdmin = false;

  openMenu: string | null = null;

    constructor(
      private router: Router,private authService: AuthService
    ) {}

  toggleMenu(menu: string) {
    this.openMenu = this.openMenu === menu ? null : menu;
  }
    logout() {
    localStorage.removeItem('token');
    this.authService.logout();

    this.router.navigate(['/login']);
  }
}
