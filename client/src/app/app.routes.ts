import { Routes } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { authGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { ServerErrorComponent } from './pages/server-error/server-error.component';
import { LoginFormComponent } from './auth/components/login-form/login-form.component';
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
  { path: 'login', component: LoginFormComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: HomePageComponent, pathMatch: 'full' },
];
