import { Routes } from '@angular/router';
import { DashboardComponent } from './features/dashboard/pages/dashboard/dashboard.component';
import { settingsRoutes } from './features/settings/settings.routes';

export const routes: Routes = [
    {
        path: '',
        component: DashboardComponent
    },
    ...settingsRoutes
];
