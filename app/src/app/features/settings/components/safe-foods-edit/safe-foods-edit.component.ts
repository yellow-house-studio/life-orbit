import { Component, input, output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { SafeFood } from '../../../../infrastructure/family/models/family.model';

@Component({
  selector: 'app-safe-foods-edit',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule
  ],
  template: `
    <div class="safe-foods-edit">
      <div class="safe-foods-list">
        @for (food of safeFoods(); track food.foodItem) {
          <div class="food-item">
            <span>{{ food.foodItem }}</span>
            <button mat-icon-button (click)="removeSafeFood(food)">
              <mat-icon>delete</mat-icon>
            </button>
          </div>
        }
      </div>

      <div class="add-safe-food">
        <mat-form-field>
          <mat-label>Food Item</mat-label>
          <input matInput [(ngModel)]="newFoodItem" placeholder="Enter food item">
        </mat-form-field>

        <button mat-button (click)="addSafeFood()" [disabled]="!newFoodItem">Add</button>
      </div>
    </div>
  `,
  styles: [`
    .safe-foods-edit {
      display: flex;
      flex-direction: column;
      gap: 16px;
    }

    .safe-foods-list {
      display: flex;
      flex-direction: column;
      gap: 8px;
    }

    .food-item {
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: 8px;
      background-color: var(--surface-color-light);
      border-radius: 4px;
    }

    .add-safe-food {
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
export class SafeFoodsEditComponent {
  safeFoods = input.required<SafeFood[]>();
  safeFoodsChange = output<SafeFood[]>();

  newFoodItem = '';

  addSafeFood() {
    if (!this.newFoodItem) return;

    const updatedSafeFoods = [
      ...this.safeFoods(),
      { foodItem: this.newFoodItem }
    ];

    this.safeFoodsChange.emit(updatedSafeFoods);
    this.newFoodItem = '';
  }

  removeSafeFood(food: SafeFood) {
    const updatedSafeFoods = this.safeFoods().filter(f => f.foodItem !== food.foodItem);
    this.safeFoodsChange.emit(updatedSafeFoods);
  }
} 