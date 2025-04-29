import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../../services/members.service';
import { MemberCardComponent } from '../../../components/members/member-card/member-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-members-list',
  imports: [MemberCardComponent, PaginationModule],
  templateUrl: './members-list.component.html',
  styleUrl: './members-list.component.css',
})
export class MembersListComponent implements OnInit {
  membersService = inject(MembersService);
  pageNumber = 1;
  pageSize = 5;

  ngOnInit(): void {
    if (!this.membersService.paginatedResult()) this.loadMembers();
  }

  loadMembers() {
    this.membersService.getMembers(this.pageNumber, this.pageSize);
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMembers();
    }
  }
}
