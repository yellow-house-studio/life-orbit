import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { Router, RouterModule } from '@angular/router';
import { AllergyListComponent } from '../../components/allergy-list/allergy-list.component';
import { SafeFoodsListComponent } from '../../components/safe-foods-list/safe-foods-list.component';
import { FoodPreferencesListComponent } from '../../components/food-preferences-list/food-preferences-list.component';
import { Allergy, SafeFood, FoodPreference } from '../../../../infrastructure/family/models/family.model';
import { MatIconModule } from '@angular/material/icon';
import { FamilyMembersStore } from '../../../../infrastructure/family/stores/family-members.store';
import { firstValueFrom, Observable } from 'rxjs';
import { AllergenSeverity, FoodPreferenceStatus } from '../../../../infrastructure/family/models/family.model';

@Component({
  selector: 'app-add-family-member',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterModule,
    MatStepperModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule,
    AllergyListComponent,
    SafeFoodsListComponent,
    FoodPreferencesListComponent
  ],
  templateUrl: './add-family-member.page.html',
  styleUrls: ['./add-family-member.page.scss']
})
export class AddFamilyMemberPage {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private familyStore = inject(FamilyMembersStore);

  basicInfoForm: FormGroup = this.fb.group({
    name: ['', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(100),
      Validators.pattern(/^[a-zA-Z\s-]+$/)
    ]],
    age: ['', [
      Validators.required,
      Validators.min(0),
      Validators.max(120)
    ]]
  });

  allergies: Allergy[] = [];
  safeFoods: SafeFood[] = [];
  foodPreferences: FoodPreference[] = [];

  updateAllergies(allergies: Allergy[]) {
    this.allergies = allergies;
  }

  updateSafeFoods(safeFoods: SafeFood[]) {
    this.safeFoods = safeFoods;
  }

  updateFoodPreferences(preferences: FoodPreference[]) {
    this.foodPreferences = preferences;
  }

  async save() {
    if (this.basicInfoForm.valid) {
      try {
        // Create the family member first
        const createObs = this.familyStore.createFamilyMember(this.basicInfoForm.value);
        const familyMemberId = await firstValueFrom(createObs as unknown as Observable<string>);

        // Add allergies
        await Promise.all(this.allergies.map(allergy => {
          const addAllergyObs = this.familyStore.addAllergy({
            familyMemberId,
            allergen: allergy.allergen,
            severity: this.convertAllergenSeverity(allergy.severity)
          });
          return firstValueFrom(addAllergyObs as unknown as Observable<void>);
        }));

        // Add safe foods
        await Promise.all(this.safeFoods.map(safeFood => {
          const addSafeFoodObs = this.familyStore.addSafeFood({
            familyMemberId,
            foodItem: safeFood.foodItem
          });
          return firstValueFrom(addSafeFoodObs as unknown as Observable<void>);
        }));

        // Add food preferences
        await Promise.all(this.foodPreferences.map(preference => {
          const addPreferenceObs = this.familyStore.addFoodPreference({
            familyMemberId,
            foodItem: preference.preference,
            status: this.convertFoodPreferenceStatus(preference.status)
          });
          return firstValueFrom(addPreferenceObs as unknown as Observable<void>);
        }));

        this.router.navigate(['/settings']);
      } catch (error) {
        // TODO: Handle error (show error message to user)
        console.error('Failed to save family member:', error);
      }
    }
  }

  private convertAllergenSeverity(severity: AllergenSeverity): 'AvailableForOthers' | 'NotAllowed' {
    return severity === AllergenSeverity.AvailableForOthers ? 'AvailableForOthers' : 'NotAllowed';
  }

  private convertFoodPreferenceStatus(status: FoodPreferenceStatus): 'Include' | 'AvailableForOthers' | 'NotAllowed' {
    switch (status) {
      case FoodPreferenceStatus.Include:
        return 'Include';
      case FoodPreferenceStatus.AvailableForOthers:
        return 'AvailableForOthers';
      case FoodPreferenceStatus.NotAllowed:
        return 'NotAllowed';
    }
  }
} 