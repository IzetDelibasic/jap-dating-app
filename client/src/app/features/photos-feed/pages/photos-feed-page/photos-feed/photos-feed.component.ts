import { Component, inject, OnInit } from '@angular/core';
import { FeedChartComponent } from '../../../components/feed-chart/feed-chart.component';
import { CommonModule } from '@angular/common';
import { MembersService } from '../../../../member-features/members.service';
import { AuthStoreService } from '../../../../../core/services/auth-store.service';
import { FeedApprovedListComponent } from '../../../components/feed-approved-list/feed-approved-list.component';
import { tap } from 'rxjs';

@Component({
  selector: 'app-photos-feed',
  imports: [FeedChartComponent, CommonModule, FeedApprovedListComponent],
  templateUrl: './photos-feed.component.html',
  styleUrl: './photos-feed.component.css',
})
export class PhotosFeedComponent implements OnInit {
  private membersService = inject(MembersService);
  private authStore = inject(AuthStoreService);

  userId?: number;

  ngOnInit(): void {
    const user = this.authStore.getCurrentUser();
    const username = user?.username;

    if (username) {
      this.membersService
        .getMember(username)
        .pipe(
          tap((member) => {
            this.userId = member.id;
          })
        )
        .subscribe();
    }
  }
}
