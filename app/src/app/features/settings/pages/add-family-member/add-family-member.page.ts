import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MatStepperModule } from '@angular/material/stepper';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { Observable, filter, firstValueFrom, take } from 'rxjs';
import { FamilyMembersStore, CreateCompleteFamilyMember } from '../../../../infrastructure/family/stores/family-members.store';
import { AllergyListComponent } from '../../components/allergy-list/allergy-list.component';
import { SafeFoodsListComponent } from '../../components/safe-foods-list/safe-foods-list.component';
import { FoodPreferencesListComponent } from '../../components/food-preferences-list/food-preferences-list.component';
import { FamilyMember, Allergy, SafeFood, FoodPreference, AllergenSeverity, FoodPreferenceStatus } from '../../../../infrastructure/family/models/family.model';
import { toObservable } from '@angular/core/rxjs-interop';
import { dataErrorStatus, dataLoadedStatus } from '../../../../infrastructure/common/models/data-loading-status';

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
    MatProgressSpinnerModule,
    MatSnackBarModule,
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
  private snackBar = inject(MatSnackBar);
  private saveStatus = toObservable(this.familyStore.getCreateCompleteStatus);

  basicInfoForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    age: [null as number | null, [Validators.required, Validators.min(0), Validators.max(120)]]
  });

  allergies: Allergy[] = [];
  safeFoods: SafeFood[] = [];
  foodPreferences: FoodPreference[] = [];

  readonly createStatus = this.familyStore.getCreateCompleteStatus;

  async save() {
    if (this.basicInfoForm.valid) {
      const name = this.basicInfoForm.value.name;
      const age = this.basicInfoForm.value.age;

      if (!name || typeof age !== 'number') {
        return;
      }

        const request: CreateCompleteFamilyMember = {
          name,
          age,
          allergies: this.allergies,
          safeFoods: this.safeFoods,
          foodPreferences: this.foodPreferences
        };

        this.familyStore.createCompleteFamilyMember(request);

        this.saveStatus.pipe(filter(status => status === dataLoadedStatus || status.haveError), take(1)).subscribe(
          (status) => {
            if (status.haveError) {
              this.snackBar.open('Failed to add family member. Please try again.', 'Close', {
                duration: 5000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom'
              });

            } else {
              this.snackBar.open('Family member added successfully!', 'Close', {
                duration: 3000,
                horizontalPosition: 'center',
                verticalPosition: 'bottom'
              });
              this.router.navigate(['/settings']);
            }
          }
        );
    }
  }

  updateAllergies(allergies: Allergy[]) {
    this.allergies = allergies;
  }

  updateSafeFoods(safeFoods: SafeFood[]) {
    this.safeFoods = safeFoods;
  }

  updateFoodPreferences(preferences: FoodPreference[]) {
    this.foodPreferences = preferences;
  }
} 