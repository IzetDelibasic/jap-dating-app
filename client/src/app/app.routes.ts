import { Routes } from '@angular/router';
import { HomePageComponent } from './features/home-page/home-page.component';
import { MemberDetailComponent } from './features/members/member-detail/member-detail.component';
import { MembersListComponent } from './features/members/members-list/members-list.component';
import { ListsComponent } from './features/lists/lists.component';
import { MessagesComponent } from './features/messages/messages.component';
import { authGuard } from './guards/auth.guard';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { NotFoundComponent } from './features/errors/not-found/not-found.component';
import { ServerErrorComponent } from './features/errors/server-error/server-error.component';

export const routes: Routes = [
  { path: '', component: HomePageComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: 'members', component: MembersListComponent },
      { path: 'members/:username', component: MemberDetailComponent },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent },
    ],
  },
  { path: 'errors', component: TestErrorsComponent },
  { path: 'not-found', component: NotFoundComponent },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '**', component: HomePageComponent, pathMatch: 'full' },
];
