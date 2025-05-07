import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../../services/members.service';
import { MemberCardComponent } from '../../../components/members/member-card/member-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-members-list',
  imports: [MemberCardComponent, PaginationModule, FormsModule, ButtonsModule, NgClass],
  templateUrl: './members-list.component.html',
  styleUrl: './members-list.component.css',
})
export class MembersListComponent implements OnInit {
  membersService = inject(MembersService);
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];

  ngOnInit(): void {
    if (!this.membersService.paginatedResult()) this.loadMembers();
  }

  loadMembers() {
    this.membersService.getMembers();
  }

  resetFilters() {
    this.membersService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    if (this.membersService.userParams().pageNumber !== event.page) {
      this.membersService.userParams().pageNumber = event.page;
      this.loadMembers();
    }
  }

  orderBy: string = 'lastActive'; 

  setActiveButton(button: string) {
    this.orderBy = button;
    this.loadMembers(); 
  }
}
