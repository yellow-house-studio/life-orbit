import { Routes } from "@angular/router";
import { SettingsPage } from "./pages/settings/settings.page";
import { FamilyMememberDashboardComponent } from "./pages/family-memember-dashboard/family-memember-dashboard.component";
import { AddFamilyMemberComponent } from "./pages/add-family-member/add-family-member.component";

export const settingsRoutes: Routes = [
    {
        path: 'settings',
        component: SettingsPage
    },
    {
        path: 'settings/:family-member-name',
        component: FamilyMememberDashboardComponent
    },
    {
        path: 'settings/family-member/add',
        component: AddFamilyMemberComponent
    }
];