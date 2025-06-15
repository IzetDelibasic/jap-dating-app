import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { FeedChartComponent } from '../../../components/feed-chart/feed-chart.component';
import { CommonModule } from '@angular/common';
import { MembersService } from '../../../../member-features/members.service';
import { AuthStoreService } from '../../../../../core/services/auth-store.service';
import { FeedApprovedListComponent } from '../../../components/feed-approved-list/feed-approved-list.component';
import {
  interval,
  Observable,
  shareReplay,
  startWith,
  Subject,
  switchMap,
  takeUntil,
  tap,
} from 'rxjs';
import { Photo } from '../../../../../core/models/photo';
import { PhotosFeedService } from '../../../photos-feed.service';

@Component({
  selector: 'app-photos-feed',
  imports: [FeedChartComponent, CommonModule, FeedApprovedListComponent],
  templateUrl: './photos-feed.component.html',
  styleUrl: './photos-feed.component.css',
})
export class PhotosFeedComponent implements OnInit, OnDestroy {
  private membersService = inject(MembersService);
  private authStore = inject(AuthStoreService);
  private photosFeedService = inject(PhotosFeedService);

  userId?: number;
  photos$?: Observable<Photo[]>;
  private destroy$ = new Subject<void>();

  ngOnInit(): void {
    const user = this.authStore.getCurrentUser();
    const username = user?.username;

    if (username) {
      this.membersService.getMember(username).subscribe((member) => {
        this.userId = member.id;
        this.photos$ = interval(10000).pipe(
          startWith(0),
          takeUntil(this.destroy$),
          switchMap(() => this.photosFeedService.getPhotosByUserId(member.id)),
          shareReplay(1)
        );
      });
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
