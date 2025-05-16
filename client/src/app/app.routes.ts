import { Routes } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { authGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { LoginFormPageComponent } from './features/auth/pages/login-form-page/login-form-page.component';
import { adminRoutes } from './features/admin-features/admin.routing';
import { memberRoutes } from './features/member-features/members.routing';
import { connectionsRoutes } from './features/connections/connections.routing';
import { messengerRoutes } from './features/messenger/messenger.routing';

export const routes: Routes = [
  // /
  // /board
  // /auth
  // /admin/
  // /members/
  // /messenger/

  { path: '', component: HomePageComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      ...adminRoutes,
      ...memberRoutes,
      ...connectionsRoutes,
      ...messengerRoutes,
    ],
  },
  { path: 'auth/login', component: LoginFormPageComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: '**', component: HomePageComponent, pathMatch: 'full' },
];
