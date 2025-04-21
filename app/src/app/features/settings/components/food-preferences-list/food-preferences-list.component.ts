import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { FoodPreference, FoodPreferenceStatus } from '../../../../infrastructure/family/models/family.model';

@Component({
  selector: 'app-food-preferences-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatChipsModule,
    MatIconModule
  ],
  templateUrl: './food-preferences-list.component.html',
  styleUrls: ['./food-preferences-list.component.scss']
})
export class FoodPreferencesListComponent {
  @Input() preferences: FoodPreference[] = [];
  @Output() preferencesChange = new EventEmitter<FoodPreference[]>();

  private fb = new FormBuilder();
  readonly FoodPreferenceStatus = FoodPreferenceStatus;

  // Common dietary preferences
  readonly DIETARY_PREFERENCES = [
    'Ketogenic',
    'Vegetarian',
    'Vegan',
    'Pescatarian',
    'Gluten-Free',
    'Dairy-Free',
    'Low-Carb',
    'Mediterranean',
    'Paleo',
    'Whole30'
  ];

  preferenceForm: FormGroup = this.fb.group({
    foodItem: ['', [Validators.required]],
    status: [FoodPreferenceStatus.Include, [Validators.required]]
  });

  isDietaryPreferenceSelected(diet: string): boolean {
    return this.preferences.some(p => p.preference.toLowerCase() === diet.toLowerCase());
  }

  addPreference(): void {
    if (this.preferenceForm.valid) {
      const { foodItem, status } = this.preferenceForm.value;
      
      // Check for duplicates
      if (this.preferences.some(p => p.preference.toLowerCase() === foodItem.toLowerCase())) {
        this.preferenceForm.get('foodItem')?.setErrors({ duplicate: true });
        return;
      }

      const newPreference: FoodPreference = {
        preference: foodItem,
        status: status
      };

      this.preferences = [...this.preferences, newPreference];
      this.preferencesChange.emit(this.preferences);
      this.preferenceForm.reset({ status: FoodPreferenceStatus.Include });
    }
  }

  removePreference(preference: FoodPreference): void {
    this.preferences = this.preferences.filter(p => p.preference !== preference.preference);
    this.preferencesChange.emit(this.preferences);
  }

  addDietaryPreference(preference: string): void {
    if (!this.isDietaryPreferenceSelected(preference)) {
      const newPreference: FoodPreference = {
        preference: preference,
        status: FoodPreferenceStatus.Include
      };
      this.preferences = [...this.preferences, newPreference];
      this.preferencesChange.emit(this.preferences);
    }
  }
} 