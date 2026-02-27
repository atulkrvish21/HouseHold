import { Component } from '@angular/core';
import { AuthService } from './services/auth.service';
import { SidebarComponent } from './sidebar/sidebar.component';
import { HeaderComponent } from './header/header.component';
import { Router, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {
  title = 'app';
   sidebarOpen = false;
  isAdmin = true;
  isLoggedIn: boolean = false;

   constructor(private authService: AuthService, public router: Router) {
    
    this.authService.isLoggedIn.subscribe(state => this.isLoggedIn = state);
    console.log("isLoggin" + this.isLoggedIn);
  }
  isNotLoginPage(): boolean {
    return this.router.url !== '/login';
  }
}
