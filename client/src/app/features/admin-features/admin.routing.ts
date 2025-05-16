import { Routes } from '@angular/router';
import { AdminPanelPageComponent } from './admin-panel/pages/admin-panel-page/admin-panel-page.component';
import { adminGuard } from '../../core/guards/admin.guard';

export const adminRoutes: Routes = [
  {
    path: 'admin',
    component: AdminPanelPageComponent,
    canActivate: [adminGuard],
  },
];
