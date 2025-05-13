import { Routes } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { MemberDetailComponent } from './features/member-features/member-detail/pages/member-detail.component';
import { MembersListComponent } from './features/member-features/members-list/pages/members-list.component';
import { ListsComponent } from './features/lists/lists.component';
import { MessageComponent } from './features/message/pages/message.component';
import { authGuard } from './core/guards/auth.guard';
import { NotFoundComponent } from './pages/errors/not-found/not-found.component';
import { ServerErrorComponent } from './pages/errors/server-error/server-error.component';
import { MemberEditComponent } from './features/member-features/member-edit/pages/member-edit.component';
import { memberDetailResolver } from './features/member-features/member-detail/member-detail.resolver';
import { AdminPanelComponent } from './features/admin-features/admin-panel/pages/admin-panel.component';
import { adminGuard } from './core/guards/admin.guard';
import { preventUnsavedChangesGuard } from './core/guards/prevent-unsaved-changes.guard';
import { LoginFormComponent } from './auth/components/login-form/login-form.component';

export const routes: Routes = [
  { path: '', component: HomePageComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: 'members', component: MembersListComponent },
      {
        path: 'members/:username',
        component: MemberDetailComponent,
        resolve: { member: memberDetailResolver },
      },
      {
        path: 'member/edit',
        component: MemberEditComponent,
        canDeactivate: [preventUnsavedChangesGuard],
      },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessageComponent },
      {
        path: 'admin',
        component: AdminPanelComponent,
        canActivate: [adminGuard],
      },
    ],
  },
  { path: 'login', component: LoginFormComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: HomePageComponent, pathMatch: 'full' },
];
