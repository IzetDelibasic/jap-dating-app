import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../../services/members.service';
import { Member } from '../../../models/member';
import { MemberCardComponent } from '../../../components/member-card/member-card.component';

@Component({
  selector: 'app-members-list',
  imports: [MemberCardComponent],
  templateUrl: './members-list.component.html',
  styleUrl: './members-list.component.css',
})
export class MembersListComponent implements OnInit {
  private membersService = inject(MembersService);
  members: Member[] = [];

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.membersService.getMembers().subscribe({
      next: (members) => (this.members = members),
    });
  }
}
