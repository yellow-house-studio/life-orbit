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

  save() {
    if (this.basicInfoForm.valid) {
      const familyMember = {
        ...this.basicInfoForm.value,
        allergies: this.allergies,
        safeFoods: this.safeFoods,
        foodPreferences: this.foodPreferences
      };

      // TODO: Add store integration
      this.router.navigate(['/settings']);
    }
  }
} 