import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddFamilyMemberComponent } from './add-family-member.component';

describe('AddFamilyMemberComponent', () => {
  let component: AddFamilyMemberComponent;
  let fixture: ComponentFixture<AddFamilyMemberComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddFamilyMemberComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddFamilyMemberComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
