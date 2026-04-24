import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { of } from 'rxjs';
import { HouseholdSurveyDetailsComponent } from './household-survey-details.component';
import { HouseholdSurveyService } from 'src/app/services/household-survey.service';

class HouseholdSurveyServiceStub {
  getHouseholds() {
    return of([]);
  }
}

describe('HouseholdSurveyDetailsComponent', () => {
  let component: HouseholdSurveyDetailsComponent;
  let fixture: ComponentFixture<HouseholdSurveyDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HouseholdSurveyDetailsComponent],
      imports: [
        BrowserAnimationsModule,
        MatButtonModule,
        MatCardModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        MatPaginatorModule,
        MatProgressSpinnerModule,
        MatTableModule
      ],
      providers: [
        { provide: HouseholdSurveyService, useClass: HouseholdSurveyServiceStub }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(HouseholdSurveyDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
