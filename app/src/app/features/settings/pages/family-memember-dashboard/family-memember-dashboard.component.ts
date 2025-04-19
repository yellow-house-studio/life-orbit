import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-family-memember-dashboard',
  imports: [RouterLink],
  templateUrl: './family-memember-dashboard.component.html',
  styleUrl: './family-memember-dashboard.component.scss'
})
export class FamilyMememberDashboardComponent {
  familyMemberName = input.required<string>();
}
