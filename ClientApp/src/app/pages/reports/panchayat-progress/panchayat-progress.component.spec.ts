import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanchayatProgressComponent } from './panchayat-progress.component';

describe('PanchayatProgressComponent', () => {
  let component: PanchayatProgressComponent;
  let fixture: ComponentFixture<PanchayatProgressComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PanchayatProgressComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanchayatProgressComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
