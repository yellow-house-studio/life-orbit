import { Component } from '@angular/core';
import { FamilyMememberDashboardComponent } from '../family-memember-dashboard/family-memember-dashboard.component';
import { ManageFamilyComponent } from '../../containers/manage-family/manage-family.component';

@Component({
  selector: 'app-settings',
  imports: [ManageFamilyComponent],
  templateUrl: './settings.page.html',
  styleUrl: './settings.page.scss'
})
export class SettingsPage {

}
