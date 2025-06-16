import { Component, Input } from '@angular/core';
import { Photo } from '../../../../core/models/photo';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-feed-approved-list',
  imports: [CommonModule],
  templateUrl: './feed-approved-list.component.html',
  styleUrl: './feed-approved-list.component.css',
})
export class FeedApprovedListComponent {
  @Input() photos$?: Observable<Photo[]>;
}
