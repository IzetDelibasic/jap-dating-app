// -Angular-
import { Component, inject, OnInit } from '@angular/core';
// -Models-
import { Photo } from '../../../../../shared/models/photo';
// -Service-
import { AdminService } from '../../admin.service';

@Component({
  selector: 'app-photo-managemet',
  imports: [],
  templateUrl: './photo-managemet.component.html',
  styleUrl: './photo-managemet.component.css',
})
export class PhotoManagemetComponent implements OnInit {
  photos: Photo[] = [];
  private adminService = inject(AdminService);

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval() {
    this.adminService.getPhotosForApproval().subscribe({
      next: (photos) => (this.photos = photos),
    });
  }

  approvePhoto(photoId: number) {
    this.adminService.approvePhoto(photoId).subscribe({
      next: () =>
        this.photos.splice(
          this.photos.findIndex((x) => x.id === photoId),
          1
        ),
    });
  }

  rejectPhoto(photoId: number) {
    this.adminService.rejectPhoto(photoId).subscribe({
      next: () =>
        this.photos.splice(
          this.photos.findIndex((x) => x.id === photoId),
          1
        ),
    });
  }
}
