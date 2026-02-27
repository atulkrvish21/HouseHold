import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HHReportComponent } from './hhreport.component';

describe('HHReportComponent', () => {
  let component: HHReportComponent;
  let fixture: ComponentFixture<HHReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HHReportComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HHReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
