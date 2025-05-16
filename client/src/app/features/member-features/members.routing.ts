import { Routes } from '@angular/router';
import { MembersBoardComponent } from './members-board/pages/members-board/members-board-page.component';
import { MemberDetailPageComponent } from './member-detail/pages/member-detail-page/member-detail-page.component';
import { memberDetailResolver } from './member-detail/member-detail.resolver';
import { MemberEditPageComponent } from './member-edit/pages/member-edit-page/member-edit-page.component';
import { preventUnsavedChangesGuard } from '../../core/guards/prevent-unsaved-changes.guard';

export const memberRoutes: Routes = [
  {
    path: 'board',
    component: MembersBoardComponent,
  },
  {
    path: 'member-details/:username',
    component: MemberDetailPageComponent,
    resolve: { member: memberDetailResolver },
  },
  {
    path: 'member/edit',
    component: MemberEditPageComponent,
    canDeactivate: [preventUnsavedChangesGuard],
  },
];
