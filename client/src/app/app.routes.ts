import { Routes } from '@angular/router';
import { HomePageComponent } from './features/home-page/home-page.component';
import { MemberDetailComponent } from './features/members/member-detail/member-detail.component';
import { MembersListComponent } from './features/members/members-list/members-list.component';
import { ListsComponent } from './features/lists/lists.component';
import { MessageComponent } from './features/message/message.component';
import { authGuard } from './guards/auth.guard';
import { NotFoundComponent } from './features/errors/not-found/not-found.component';
import { ServerErrorComponent } from './features/errors/server-error/server-error.component';
import { MemberEditComponent } from './components/members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './guards/prevent-unsaved-changes.guard';
import { memberDetailResolver } from './resolvers/member-detail.resolver';
import { AdminPanelComponent } from './features/admin/admin-panel/admin-panel.component';
import { adminGuard } from './guards/admin.guard';
import { LoginFormComponent } from './components/forms/login-form/login-form.component';

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
