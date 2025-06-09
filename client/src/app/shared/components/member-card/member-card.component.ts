import { RouterLink } from '@angular/router';
import { Component, computed, inject, input } from '@angular/core';
import { Member } from '../../../core/models/member';
import { LikesService } from '../../../core/services/likes.service';
import { PresenceService } from '../../../core/services/presence.service';
import { ToastrService } from 'ngx-toastr';
import { DEFAULT_PHOTO_URL } from '../../../core/constants/contentConstants/imagesConstant';
import { catchError, of, tap } from 'rxjs';

@Component({
  selector: 'app-member-card',
  imports: [RouterLink],
  templateUrl: './member-card.component.html',
  styleUrl: './member-card.component.css',
})
export class MemberCardComponent {
  private likeService = inject(LikesService);
  private presenceService = inject(PresenceService);
  private toastr = inject(ToastrService);

  member = input.required<Member>();
  defaultPhoto = DEFAULT_PHOTO_URL;

  hasLiked = computed(
    () => this.member() && this.likeService.likeIds().includes(this.member().id)
  );

  isOnline = computed(
    () =>
      this.member() &&
      this.presenceService.onlineUsers().includes(this.member().userName)
  );

  toggleLike() {
    this.likeService
      .toggleLike(this.member().id)
      .pipe(
        tap(() => {
          this.likeService.updateLikeIds(this.member().id, this.hasLiked());
        }),
        catchError(() => {
          this.toastr.error('Failed to toggle like');
          return of(null);
        })
      )
      .subscribe();
  }
}
