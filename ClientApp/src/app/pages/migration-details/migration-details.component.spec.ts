import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MigrationDetailsComponent } from './migration-details.component';

describe('MigrationDetailsComponent', () => {
  let component: MigrationDetailsComponent;
  let fixture: ComponentFixture<MigrationDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MigrationDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MigrationDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
