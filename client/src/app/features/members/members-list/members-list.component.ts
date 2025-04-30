import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../../services/members.service';
import { MemberCardComponent } from '../../../components/members/member-card/member-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { AccountService } from '../../../services/account.service';
import { UserParams } from '../../../models/userParams';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-members-list',
  imports: [MemberCardComponent, PaginationModule, FormsModule],
  templateUrl: './members-list.component.html',
  styleUrl: './members-list.component.css',
})
export class MembersListComponent implements OnInit {
  private accountService = inject(AccountService);
  membersService = inject(MembersService);
  userParams = new UserParams(this.accountService.currentUser());
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];

  ngOnInit(): void {
    if (!this.membersService.paginatedResult()) this.loadMembers();
  }

  loadMembers() {
    this.membersService.getMembers(this.userParams);
  }

  resetFilters() {
    this.userParams = new UserParams(this.accountService.currentUser());
    this.loadMembers();
  }

  pageChanged(event: any) {
    if (this.userParams.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    }
  }
}
