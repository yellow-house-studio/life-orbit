import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatRadioModule } from '@angular/material/radio';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { Allergy, AllergenSeverity } from '../../../../infrastructure/family/models/family.model';

@Component({
  selector: 'app-allergy-list',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatRadioModule,
    MatChipsModule,
    MatIconModule
  ],
  templateUrl: './allergy-list.component.html',
  styleUrls: ['./allergy-list.component.scss']
})
export class AllergyListComponent {
  @Input() allergies: Allergy[] = [];
  @Output() allergiesChange = new EventEmitter<Allergy[]>();

  private fb = new FormBuilder();
  readonly AllergenSeverity = AllergenSeverity;

  allergyForm: FormGroup = this.fb.group({
    allergen: ['', [Validators.required]],
    severity: [AllergenSeverity.NotAllowed, [Validators.required]]
  });

  addAllergy() {
    if (this.allergyForm.valid) {
      const newAllergy: Allergy = this.allergyForm.value;
      
      // Check for duplicates
      if (this.allergies.some(a => a.allergen.toLowerCase() === newAllergy.allergen.toLowerCase())) {
        this.allergyForm.get('allergen')?.setErrors({ duplicate: true });
        return;
      }

      this.allergies = [...this.allergies, newAllergy];
      this.allergiesChange.emit(this.allergies);
      this.allergyForm.reset({ severity: AllergenSeverity.NotAllowed });
    }
  }

  removeAllergy(allergy: Allergy) {
    this.allergies = this.allergies.filter(a => a.allergen !== allergy.allergen);
    this.allergiesChange.emit(this.allergies);
  }
} 