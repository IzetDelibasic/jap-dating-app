import { inject } from '@angular/core';
import { CanDeactivateFn } from '@angular/router';
import { MemberEditPageComponent } from '../../features/member-features/member-edit/pages/member-edit-page/member-edit-page.component';
import { ConfirmService } from '../services/confirm.service';

export const preventUnsavedChangesGuard: CanDeactivateFn<
  MemberEditPageComponent
> = (component) => {
  const confirmService = inject(ConfirmService);

  if (component.editForm?.dirty) {
    return confirmService.confirm() ?? false;
  }
  return true;
};
