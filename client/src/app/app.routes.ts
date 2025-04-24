import { Routes } from '@angular/router';
import { HomePageComponent } from './features/home-page/home-page.component';
import { MemberDetailComponent } from './features/member-detail/member-detail.component';
import { MembersListComponent } from './features/members-list/members-list.component';
import { ListsComponent } from './features/lists/lists.component';
import { MessagesComponent } from './features/messages/messages.component';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: HomePageComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      { path: 'members', component: MembersListComponent },
      { path: 'members/:id', component: MemberDetailComponent },
      { path: 'lists', component: ListsComponent },
      { path: 'messages', component: MessagesComponent },
    ],
  },

  { path: '**', component: HomePageComponent, pathMatch: 'full' },
];
