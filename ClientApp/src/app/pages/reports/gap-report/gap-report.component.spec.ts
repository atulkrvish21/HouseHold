import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GapReportComponent } from './gap-report.component';

describe('GapReportComponent', () => {
  let component: GapReportComponent;
  let fixture: ComponentFixture<GapReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GapReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GapReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
