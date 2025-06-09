import { Component, inject,  } from '@angular/core';
import { Photo } from '../../../../../core/models/photo';
import { AdminService } from '../../admin.service';
import { BehaviorSubject, Observable, switchMap  } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-manage-photo',
  imports: [CommonModule],
  templateUrl: './manage-photo.component.html',
  styleUrls: ['./manage-photo.component.css'],
})
export class ManagePhotoComponent {
  private adminService = inject(AdminService);

  private refreshPhotos$ = new BehaviorSubject<void>(undefined);

  photos$: Observable<Photo[]> = this.refreshPhotos$.pipe(
    switchMap(() => this.adminService.getPhotosForApproval())
  );

  approvePhoto(photoId: number) {
    this.adminService.approvePhoto(photoId).subscribe({
      next: () => this.refreshPhotos$.next(),
    });
  }

  rejectPhoto(photoId: number) {
    this.adminService.rejectPhoto(photoId).subscribe({
      next: () => this.refreshPhotos$.next(),
    });
  }

  trackByPhotoId(index: number, photo: Photo): number {
    return photo.id;
  }
}
