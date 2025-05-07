// -Angular-
import { ResolveFn } from '@angular/router';
import { inject } from '@angular/core';
// -Models-
import { Member } from '../models/member';
// -Services-
import { MembersService } from '../services/members.service';

export const memberDetailResolver: ResolveFn<Member | null> = (route) => {
  const memberService = inject(MembersService);

  const username = route.paramMap.get('username');

  if (!username) return null;

  return memberService.getMember(username);
};
