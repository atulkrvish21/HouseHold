import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

import { AuthGuard } from './services/auth.guard';
import { AuthService } from './services/auth.service';




import { NgxPaginationModule } from 'ngx-pagination';

import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NotificationComponent } from './notification/notification.component';
import { NgxImageCompressService } from 'ngx-image-compress';


import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { HeaderComponent } from './header/header.component';
import { LucideAngularModule, Gauge, FolderPlus, ChevronDown } from 'lucide-angular';
import { HHapprovalComponent } from './pages/hhapproval/hhapproval.component';
import { HHReportComponent } from './pages/hhreport/hhreport.component';
import { HouseholdDetailsComponent } from './pages/household-details/household-details.component';
import { HouseholdEntryComponent } from './pages/household-entry/household-entry.component';
import { SurveyProgressComponent } from './pages/survey-progress/survey-progress.component';
import { PanchayatProgressComponent } from './pages/reports/panchayat-progress/panchayat-progress.component';
import { GapReportComponent } from './pages/reports/gap-report/gap-report.component';
import { NgChartsModule } from 'ng2-charts';
import { DashboardChartComponent } from './pages/reports/dashboard-chart/dashboard-chart.component';
@NgModule({
  declarations: [
  AppComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    NotificationComponent,
    DashboardComponent,
    HeaderComponent,
    SidebarComponent,
    HHapprovalComponent,
    HHReportComponent,
    HouseholdDetailsComponent,
    HouseholdEntryComponent,
    SurveyProgressComponent,
    PanchayatProgressComponent,
    GapReportComponent,
    DashboardChartComponent,
    
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    NgxPaginationModule,
    FormsModule,
    NgChartsModule,
    ReactiveFormsModule,
    LucideAngularModule.pick({ Gauge, FolderPlus, ChevronDown }),
    RouterModule.forRoot([
        { path: '', component: LoginComponent, pathMatch: 'full' },
        { path: 'login', component: LoginComponent },
        { path: 'dashboard', component: DashboardComponent, canActivate: [AuthGuard] },
        { path: 'household-approval', component: HHapprovalComponent, canActivate: [AuthGuard] },
        { path: 'household-entry', component: HouseholdEntryComponent, canActivate: [AuthGuard] },
        { path: 'household-entry/:id', component: HouseholdEntryComponent, canActivate: [AuthGuard] },
        { path: 'household-report', component: HHReportComponent, canActivate: [AuthGuard] },
        { path: 'survey-progress', component: SurveyProgressComponent, canActivate: [AuthGuard] },
        { path: 'panchayat-progress', component: PanchayatProgressComponent, canActivate: [AuthGuard] },
        { path: 'gap-report', component: GapReportComponent, canActivate: [AuthGuard] },
        { path: 'dashboard-chart', component: DashboardChartComponent, canActivate: [AuthGuard] },

    ])
],
  providers: [NgxImageCompressService, AuthService,
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }],
  bootstrap: [AppComponent]
})
export class AppModule { }
