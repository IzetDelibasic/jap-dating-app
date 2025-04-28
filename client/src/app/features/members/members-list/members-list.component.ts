import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../../services/members.service';
import { Member } from '../../../models/member';
import { MemberCardComponent } from '../../../components/members/member-card/member-card.component';

@Component({
  selector: 'app-members-list',
  imports: [MemberCardComponent],
  templateUrl: './members-list.component.html',
  styleUrl: './members-list.component.css',
})
export class MembersListComponent implements OnInit {
  membersService = inject(MembersService);
  members: Member[] = [];

  ngOnInit(): void {
    if (this.membersService.members().length === 0) this.loadMembers();
  }

  loadMembers() {
    this.membersService.getMembers();
  }
}
