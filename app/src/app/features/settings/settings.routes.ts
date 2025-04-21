import { Routes } from "@angular/router";
import { SettingsPage } from "./pages/settings/settings.page";
import { FamilyMememberDashboardComponent } from "./pages/family-memember-dashboard/family-memember-dashboard.component";
import { AddFamilyMemberPage } from "./pages/add-family-member/add-family-member.page";

export const settingsRoutes: Routes = [
    {
        path: 'settings',
        component: SettingsPage
    },
    {
        path: 'settings/family/add',
        component: AddFamilyMemberPage
    },
    {
        path: 'settings/family/:id',
        component: FamilyMememberDashboardComponent
    }
];