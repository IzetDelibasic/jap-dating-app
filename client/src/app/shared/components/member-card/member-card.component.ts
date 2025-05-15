import { RouterLink } from '@angular/router';
import { Component, computed, inject, input } from '@angular/core';
import { Member } from '../../../core/models/member';
import { LikesService } from '../../../core/services/likes.service';
import { PresenceService } from '../../../core/services/presence.service';
import { ToastrService } from 'ngx-toastr';

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

  hasLiked = computed(
    () => this.member() && this.likeService.likeIds().includes(this.member().id)
  );

  isOnline = computed(
    () =>
      this.member() &&
      this.presenceService.onlineUsers().includes(this.member().userName)
  );

  toggleLike() {
    this.likeService.toggleLike(this.member().id).subscribe({
      next: () => {
        this.likeService.updateLikeIds(this.member().id, this.hasLiked());
      },
      error: () => {
        this.toastr.error('Failed to toggle like');
      },
    });
  }
}
