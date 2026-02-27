import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SurveyProgressComponent } from './survey-progress.component';

describe('SurveyProgressComponent', () => {
  let component: SurveyProgressComponent;
  let fixture: ComponentFixture<SurveyProgressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SurveyProgressComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SurveyProgressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
