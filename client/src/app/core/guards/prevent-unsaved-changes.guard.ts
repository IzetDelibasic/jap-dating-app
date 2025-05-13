// -Angular-
import { inject } from '@angular/core';
import { CanDeactivateFn } from '@angular/router';
// -Components-
import { MemberEditComponent } from '../../features/member-features/member-edit/pages/member-edit.component';
// -Service-
import { ConfirmService } from '../services/confirm.service';

export const preventUnsavedChangesGuard: CanDeactivateFn<
  MemberEditComponent
> = (component) => {
  const confirmService = inject(ConfirmService);

  if (component.editForm?.dirty) {
    return confirmService.confirm() ?? false;
  }
  return true;
};
