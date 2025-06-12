import {
  Component,
  inject,
  Input,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { PhotosFeedService } from '../../photos-feed.service';
import { Photo } from '../../../../core/models/photo';
import { CommonModule } from '@angular/common';
import {
  distinctUntilChanged,
  interval,
  map,
  Observable,
  shareReplay,
  startWith,
  switchMap,
} from 'rxjs';

@Component({
  selector: 'app-feed-approved-list',
  imports: [CommonModule],
  templateUrl: './feed-approved-list.component.html',
  styleUrl: './feed-approved-list.component.css',
})
export class FeedApprovedListComponent implements OnChanges {
  @Input() userId?: number;
  private photosFeedService = inject(PhotosFeedService);

  approvedPhotos$?: Observable<Photo[]>;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['userId'] && this.userId !== undefined) {
      this.approvedPhotos$ = interval(10000).pipe(
        startWith(0),
        switchMap(() => this.photosFeedService.getPhotosByUserId(this.userId!)),
        map((photos) => photos.filter((photo) => photo.isApproved)),
        distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b)),
        shareReplay(1)
      );
    }
  }
}
