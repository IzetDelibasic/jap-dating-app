import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, inject, OnInit } from '@angular/core';
import { MembersService } from '../../../members.service';
import { MemberCardComponent } from '../../../../../shared/components/member-card/member-card.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { BehaviorSubject, catchError, of, tap } from 'rxjs';
import { Member } from '../../../../../core/models/member';

@Component({
  selector: 'app-members-board-page',
  imports: [
    MemberCardComponent,
    PaginationModule,
    FormsModule,
    ButtonsModule,
    NgClass,
  ],
  templateUrl: './members-board-page.component.html',
  styleUrl: './members-board-page.component.css',
})
export class MembersBoardComponent implements OnInit {
  membersService = inject(MembersService);
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];

  members$ = new BehaviorSubject<Member[]>([]);
  errorMessage = '';

  ngOnInit(): void {
    if (!this.membersService.paginatedResult()) this.loadMembers();
  }

  loadMembers() {
    this.errorMessage = '';
    this.membersService
      .getMembers()
      .pipe(
        tap((members) => {
          this.members$.next(members);
        }),
        catchError((err) => {
          this.handleError(err);
          return of([]);
        })
      )
      .subscribe();
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

  private handleError(error: any) {
    this.errorMessage = 'Failed to load members. Please try again.';
    console.error(error);
  }
}
