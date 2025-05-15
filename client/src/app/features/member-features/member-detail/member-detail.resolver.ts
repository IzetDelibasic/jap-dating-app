import { ResolveFn } from '@angular/router';
import { inject } from '@angular/core';
import { Member } from '../../../core/models/member';
import { MembersService } from '../members.service';

export const memberDetailResolver: ResolveFn<Member | null> = (route) => {
  const memberService = inject(MembersService);

  const username = route.paramMap.get('username');

  if (!username) return null;

  return memberService.getMember(username);
};
