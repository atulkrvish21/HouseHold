import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HHapprovalComponent } from './hhapproval.component';

describe('HHapprovalComponent', () => {
  let component: HHapprovalComponent;
  let fixture: ComponentFixture<HHapprovalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HHapprovalComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HHapprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
