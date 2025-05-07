// -Angular-
import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
// -Service-
import { LikesService } from '../../services/likes.service';
// -Components-
import { MemberCardComponent } from '../../components/members/member-card/member-card.component';
// -NgxBootstrap-
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-lists',
  imports: [ButtonsModule, FormsModule, MemberCardComponent, PaginationModule, NgClass],
  templateUrl: './lists.component.html',
  styleUrl: './lists.component.css',
})
export class ListsComponent implements OnInit, OnDestroy {
  likesService = inject(LikesService);
  predicate = 'liked';
  pageSize = 5;
  pageNumber = 1;

  ngOnInit(): void {
    this.loadLikes();
  }

  getTitle() {
    switch (this.predicate) {
      case 'liked':
        return 'Members you like';
      case 'likedBy':
        return 'Members who like you';
      default:
        return 'Mutual';
    }
  }

  loadLikes() {
    this.likesService.getLikes(this.predicate, this.pageNumber, this.pageSize);
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadLikes();
    }
  }

  ngOnDestroy(): void {
    this.likesService.paginatedResult.set(null);
  }
}
