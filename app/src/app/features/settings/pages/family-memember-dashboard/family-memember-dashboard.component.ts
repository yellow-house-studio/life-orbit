import { Component, computed, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { FamilyMembersStore } from '../../../../infrastructure/family/stores/family-members.store';
import { Allergy, FoodPreference, SafeFood, AllergenSeverity, FoodPreferenceStatus } from '../../../../infrastructure/family/models/family.model';
import { AllergyEditComponent } from '../../components/allergy-edit/allergy-edit.component';
import { SafeFoodsEditComponent } from '../../components/safe-foods-edit/safe-foods-edit.component';
import { FoodPreferencesEditComponent } from '../../components/food-preferences-edit/food-preferences-edit.component';

@Component({
  selector: 'app-family-memember-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    MatButtonModule,
    MatIconModule,
    MatCardModule,
    MatProgressSpinnerModule,
    AllergyEditComponent,
    SafeFoodsEditComponent,
    FoodPreferencesEditComponent
  ],
  templateUrl: './family-memember-dashboard.component.html',
  styleUrl: './family-memember-dashboard.component.scss'
})
export class FamilyMememberDashboardComponent {
  private store = inject(FamilyMembersStore);
  name = input.required<string>();

  readonly loadStatus = this.store.getLoadStatus;
  readonly familyMember = computed(() => {
    return this.store.familyMembers().find(member => member.name === this.name());
  });

  // Track which section is being edited
  editingSection = signal<'allergies' | 'safeFoods' | 'preferences' | null>(null);

  // Temporary storage for edits
  editedAllergies = signal<Allergy[]>([]);
  editedSafeFoods = signal<SafeFood[]>([]);
  editedPreferences = signal<FoodPreference[]>([]);

  startEditing(section: 'allergies' | 'safeFoods' | 'preferences') {
    this.editingSection.set(section);
    // Initialize temporary storage with current values
    const member = this.familyMember();
    if (!member) return;

    switch (section) {
      case 'allergies':
        this.editedAllergies.set([...member.allergies]);
        break;
      case 'safeFoods':
        this.editedSafeFoods.set([...member.safeFoods]);
        break;
      case 'preferences':
        this.editedPreferences.set([...member.foodPreferences]);
        break;
    }
  }

  cancelEditing() {
    this.editingSection.set(null);
  }

  saveChanges() {
    const member = this.familyMember();
    if (!member) return;

    const section = this.editingSection();
    if (!section) return;

    switch (section) {
      case 'allergies':
        // Compare and update allergies
        const currentAllergies = new Set(member.allergies.map(a => a.allergen));
        const newAllergies = new Set(this.editedAllergies().map(a => a.allergen));
        
        // Remove allergies that are no longer present
        member.allergies
          .filter(a => !newAllergies.has(a.allergen))
          .forEach(a => {
            this.store.removeAllergy({ familyMemberId: member.id, allergen: a.allergen });
          });
        
        // Add new allergies
        this.editedAllergies()
          .filter(a => !currentAllergies.has(a.allergen))
          .forEach(a => {
            this.store.addAllergy({ 
              familyMemberId: member.id, 
              allergen: a.allergen, 
              severity: a.severity === AllergenSeverity.AvailableForOthers ? 'AvailableForOthers' : 'NotAllowed'
            });
          });
        break;

      case 'safeFoods':
        // Compare and update safe foods
        const currentSafeFoods = new Set(member.safeFoods.map(f => f.foodItem));
        const newSafeFoods = new Set(this.editedSafeFoods().map(f => f.foodItem));
        
        // Remove safe foods that are no longer present
        member.safeFoods
          .filter(f => !newSafeFoods.has(f.foodItem))
          .forEach(f => {
            this.store.removeSafeFood({ familyMemberId: member.id, foodItem: f.foodItem });
          });
        
        // Add new safe foods
        this.editedSafeFoods()
          .filter(f => !currentSafeFoods.has(f.foodItem))
          .forEach(f => {
            this.store.addSafeFood({ familyMemberId: member.id, foodItem: f.foodItem });
          });
        break;

      case 'preferences':
        // Compare and update preferences
        const currentPreferences = new Set(member.foodPreferences.map(p => p.preference));
        const newPreferences = new Set(this.editedPreferences().map(p => p.preference));
        
        // Remove preferences that are no longer present
        member.foodPreferences
          .filter(p => !newPreferences.has(p.preference))
          .forEach(p => {
            this.store.removeFoodPreference({ familyMemberId: member.id, foodItem: p.preference });
          });
        
        // Add new preferences
        this.editedPreferences()
          .filter(p => !currentPreferences.has(p.preference))
          .forEach(p => {
            this.store.addFoodPreference({ 
              familyMemberId: member.id, 
              foodItem: p.preference,
              status: p.status === FoodPreferenceStatus.Include ? 'Include' :
                     p.status === FoodPreferenceStatus.AvailableForOthers ? 'AvailableForOthers' : 'NotAllowed'
            });
          });
        break;
    }

    this.editingSection.set(null);
  }

  retry() {
    this.store.loadFamilyMembers();
  }
}
