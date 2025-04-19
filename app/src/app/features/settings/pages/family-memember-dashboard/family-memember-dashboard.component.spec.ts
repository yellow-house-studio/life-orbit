import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FamilyMememberDashboardComponent } from './family-memember-dashboard.component';

describe('FamilyMememberDashboardComponent', () => {
  let component: FamilyMememberDashboardComponent;
  let fixture: ComponentFixture<FamilyMememberDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FamilyMememberDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FamilyMememberDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
