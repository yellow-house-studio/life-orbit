import { Component, inject } from '@angular/core';
import { RouterLink, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FamilyMembersStore } from '../../../../infrastructure/family/stores/family-members.store';

@Component({
  selector: 'app-manage-family',
  standalone: true,
  imports: [RouterLink, RouterModule, CommonModule],
  templateUrl: './manage-family.component.html',
  styleUrl: './manage-family.component.scss'
})
export class ManageFamilyComponent {
  private store = inject(FamilyMembersStore);

  readonly familyMembers = this.store.familyMembers;
  readonly loadStatus = this.store.getLoadStatus;

  retry() {
    this.store.loadFamilyMembers();
  }
} 
