// -Angular-
import { ResolveFn } from '@angular/router';
import { inject } from '@angular/core';
// -Models-
import { Member } from '../../../shared/models/member';
// -Services-
import { MembersService } from '../members.service';

export const memberDetailResolver: ResolveFn<Member | null> = (route) => {
  const memberService = inject(MembersService);

  const username = route.paramMap.get('username');

  if (!username) return null;

  return memberService.getMember(username);
};
