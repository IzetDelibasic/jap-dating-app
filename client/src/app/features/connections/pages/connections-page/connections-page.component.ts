import { NgClass } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { LikesService } from '../../../../core/services/likes.service';
import { MemberCardComponent } from '../../../../shared/components/member-card/member-card.component';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { catchError, of, Subscription, tap } from 'rxjs';

@Component({
  selector: 'app-connections-page',
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
    this.loadLikes().subscribe();
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
    return this.likesService
      .getLikes(this.predicate, this.pageNumber, this.pageSize)
      .pipe(
        tap(() => {
          console.log('Likes loaded successfully');
        }),
        catchError((err) => {
          console.error('Error fetching likes:', err);
          return of(null);
        })
      );
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadLikes().subscribe();
    }
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
    this.likesService.paginatedResult.set(null);
  }
}
