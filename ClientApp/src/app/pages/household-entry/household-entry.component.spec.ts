import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HouseholdEntryComponent } from './household-entry.component';

describe('HouseholdEntryComponent', () => {
  let component: HouseholdEntryComponent;
  let fixture: ComponentFixture<HouseholdEntryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HouseholdEntryComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HouseholdEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
