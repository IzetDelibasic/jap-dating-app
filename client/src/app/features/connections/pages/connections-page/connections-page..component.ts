import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikesService } from '../../../../core/services/likes.service';
import { MemberCardComponent } from './../../../../shared/components/member-card/member-card.component';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-lists',
  imports: [
    ButtonsModule,
    FormsModule,
    MemberCardComponent,
    PaginationModule,
    NgClass,
  ],
  templateUrl: './connections-page.component.html',
  styleUrl: './connections-page.component.css',
})
export class ConnectionsPage implements OnInit, OnDestroy {
  likesService = inject(LikesService);
  predicate = 'liked';
  pageSize = 5;
  pageNumber = 1;
  private subscriptions: Subscription = new Subscription();

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
    this.likesService
      .getLikes(this.predicate, this.pageNumber, this.pageSize)
      .subscribe({
        next: () => {
          console.log('Likes loaded successfully');
        },
        error: (err) => {
          console.error('Error fetching likes:', err);
        },
      });
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadLikes();
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
    this.likesService.paginatedResult.set(null);
  }
}
