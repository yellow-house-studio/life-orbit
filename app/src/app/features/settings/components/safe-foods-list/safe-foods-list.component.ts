import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { SafeFood } from '../../../../infrastructure/family/models/family.model';

@Component({
  selector: 'app-safe-foods-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatChipsModule,
    MatIconModule
  ],
  templateUrl: './safe-foods-list.component.html',
  styleUrls: ['./safe-foods-list.component.scss']
})
export class SafeFoodsListComponent {
  @Input() safeFoods: SafeFood[] = [];
  @Output() safeFoodsChange = new EventEmitter<SafeFood[]>();

  private fb = new FormBuilder();

  foodForm: FormGroup = this.fb.group({
    foodItem: ['', [Validators.required]]
  });

  addFood() {
    if (this.foodForm.valid) {
      const newFood: SafeFood = { foodItem: this.foodForm.value.foodItem };
      
      // Check for duplicates
      if (this.safeFoods.some(f => f.foodItem.toLowerCase() === newFood.foodItem.toLowerCase())) {
        this.foodForm.get('foodItem')?.setErrors({ duplicate: true });
        return;
      }

      this.safeFoods = [...this.safeFoods, newFood];
      this.safeFoodsChange.emit(this.safeFoods);
      this.foodForm.reset();
    }
  }

  removeFood(food: SafeFood) {
    this.safeFoods = this.safeFoods.filter(f => f.foodItem !== food.foodItem);
    this.safeFoodsChange.emit(this.safeFoods);
  }
} 