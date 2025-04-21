import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { Allergy, AllergenSeverity } from '../../../../infrastructure/family/models/family.model';

@Component({
  selector: 'app-allergy-edit',
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
    <div class="allergy-edit">
      <div class="allergies-list">
        @for (allergy of allergies(); track allergy.allergen) {
          <div class="allergy-item">
            <span>{{ allergy.allergen }}</span>
            <span>{{ allergy.severity }}</span>
            <button mat-icon-button (click)="removeAllergy(allergy)">
              <mat-icon>delete</mat-icon>
            </button>
          </div>
        }
      </div>

      <div class="add-allergy">
        <mat-form-field>
          <mat-label>Allergen</mat-label>
          <input matInput [(ngModel)]="newAllergen" placeholder="Enter allergen">
        </mat-form-field>

        <mat-form-field>
          <mat-label>Severity</mat-label>
          <mat-select [(ngModel)]="newSeverity">
            <mat-option [value]="AllergenSeverity.NotAllowed">Not Allowed</mat-option>
            <mat-option [value]="AllergenSeverity.AvailableForOthers">Available for Others</mat-option>
          </mat-select>
        </mat-form-field>

        <button mat-button (click)="addAllergy()" [disabled]="!newAllergen">Add</button>
      </div>
    </div>
  `,
  styles: [`
    .allergy-edit {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .allergies-list {
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .allergy-item {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 8px;
      background-color: var(--surface-color-light);
      border-radius: 4px;
    }

    .add-allergy {
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
export class AllergyEditComponent {
  allergies = input.required<Allergy[]>();
  allergiesChange = output<Allergy[]>();

  AllergenSeverity = AllergenSeverity;
  newAllergen = '';
  newSeverity = AllergenSeverity.NotAllowed;

  addAllergy() {
    if (!this.newAllergen) return;

    const updatedAllergies = [
      ...this.allergies(),
      { allergen: this.newAllergen, severity: this.newSeverity }
    ];

    this.allergiesChange.emit(updatedAllergies);
    this.newAllergen = '';
    this.newSeverity = AllergenSeverity.NotAllowed;
  }

  removeAllergy(allergy: Allergy) {
    const updatedAllergies = this.allergies().filter(a => a.allergen !== allergy.allergen);
    this.allergiesChange.emit(updatedAllergies);
  }
} 