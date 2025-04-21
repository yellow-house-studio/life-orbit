import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { FoodPreference, FoodPreferenceStatus } from '../../../../infrastructure/family/models/family.model';

@Component({
  selector: 'app-food-preferences-edit',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule
  ],
  template: `
    <div class="preferences-edit">
      <div class="preferences-list">
        @for (pref of preferences(); track pref.preference) {
          <div class="preference-item">
            <span>{{ pref.preference }}</span>
            <span class="status">{{ pref.status }}</span>
            <button mat-icon-button (click)="removePreference(pref)">
              <mat-icon>delete</mat-icon>
            </button>
          </div>
        }
      </div>

      <div class="add-preference">
        <mat-form-field>
          <mat-label>Food Item</mat-label>
          <input matInput [(ngModel)]="newFoodItem" placeholder="Enter food item">
        </mat-form-field>

        <mat-form-field>
          <mat-label>Status</mat-label>
          <mat-select [(ngModel)]="newStatus">
            <mat-option [value]="FoodPreferenceStatus.Include">Include</mat-option>
            <mat-option [value]="FoodPreferenceStatus.AvailableForOthers">Available for Others</mat-option>
            <mat-option [value]="FoodPreferenceStatus.NotAllowed">Not Allowed</mat-option>
          </mat-select>
        </mat-form-field>

        <button mat-button (click)="addPreference()" [disabled]="!newFoodItem">Add</button>
      </div>
    </div>
  `,
  styles: [`
    .preferences-edit {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .preferences-list {
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .preference-item {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 8px;
      background-color: var(--surface-color-light);
      border-radius: 4px;

      .status {
        font-size: 0.9rem;
        color: var(--text-secondary);
        padding: 4px 8px;
        border-radius: 4px;
        background-color: var(--surface-color);
      }
    }

    .add-preference {
      display: flex;
      gap: 8px;
      align-items: flex-start;

      mat-form-field {
        flex: 1;
      }

      button {
        margin-top: 4px;
      }
    }
  `]
})
export class FoodPreferencesEditComponent {
  preferences = input.required<FoodPreference[]>();
  preferencesChange = output<FoodPreference[]>();

  FoodPreferenceStatus = FoodPreferenceStatus;
  newFoodItem = '';
  newStatus = FoodPreferenceStatus.Include;

  addPreference() {
    if (!this.newFoodItem) return;

    const updatedPreferences = [
      ...this.preferences(),
      { preference: this.newFoodItem, status: this.newStatus }
    ];

    this.preferencesChange.emit(updatedPreferences);
    this.newFoodItem = '';
    this.newStatus = FoodPreferenceStatus.Include;
  }

  removePreference(pref: FoodPreference) {
    const updatedPreferences = this.preferences().filter(p => p.preference !== pref.preference);
    this.preferencesChange.emit(updatedPreferences);
  }
} 