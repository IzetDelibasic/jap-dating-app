import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  inject,
  Input,
  OnInit,
  Output,
} from '@angular/core';
import { DecimalPipe, NgClass, NgFor, NgIf, NgStyle } from '@angular/common';
import { Member } from '../../../../../core/models/member';
import { Photo } from '../../../../../core/models/photo';
import { FileUploadModule, FileUploader } from 'ng2-file-upload';
import { AccountService } from '../../../../../core/services/account.service';
import { MembersService } from '../../../members.service';
import { environment } from '../../../../../../environments/environment';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-photo-editor',
  imports: [
    NgStyle,
    NgClass,
    FileUploadModule,
    DecimalPipe,
    NgSelectModule,
    FormsModule,
  ],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css',
})
export class PhotoEditorComponent implements OnInit {
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  private cdr = inject(ChangeDetectorRef);

  @Input() member!: Member;
  @Output() memberChange = new EventEmitter<Member>();

  uploader?: FileUploader;
  hasBaseDropZoneOver = false;

  tags: { id: number; name: string }[] = [];
  photoTags: { [photoId: number]: string[] } = {};
  selectedTag: string = '';
  selectedTags: string[] = [];
  searchTags: string[] = [];
  filteredPhotos: Photo[] = [];
  searchedPhotos: Photo[] = [];

  ngOnInit(): void {
    this.initializeUploader();
    this.loadTags();
    this.filteredPhotos = [...this.member.photos];
    this.member.photos.forEach((photo) => this.loadTagsForPhoto(photo.id));
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader(): void {
    this.uploader = new FileUploader({
      url: `${environment.apiBaseUrl}photo/add-photo`,
      authToken: `Bearer ${this.accountService.currentUser()?.token}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onBuildItemForm = (fileItem, form) => {
      this.selectedTags.forEach((tag) => {
        form.append('Tags', tag);
      });
    };

    this.uploader.onSuccessItem = (item, response) => {
      const photo = JSON.parse(response);
      this.member.photos.push(photo);
      this.filteredPhotos = [...this.member.photos];
      this.memberChange.emit(this.member);

      this.loadTagsForPhoto(photo.id);

      this.selectedTags = [];
      this.cdr.detectChanges();
    };
  }

  loadTags(): void {
    this.memberService.getTags().subscribe({
      next: (response) => (this.tags = response),
      error: (err) => console.error('Error fetching tags:', err),
    });
  }

  loadTagsForPhoto(photoId: number): void {
    this.memberService.getTagsForPhoto(photoId).subscribe({
      next: (tags) => (this.photoTags[photoId] = tags || []),
      error: () => (this.photoTags[photoId] = []),
    });
  }

  searchPhotosByTag(): void {
    if (!this.searchTags || this.searchTags.length === 0) {
      alert('Please select at least one tag to search.');
      return;
    }

    this.filteredPhotos = this.member.photos.filter((photo) =>
      this.searchTags.some((tag) => this.photoTags[photo.id]?.includes(tag))
    );
  }

  resetFilter(): void {
    this.filteredPhotos = [...this.member.photos];
  }

  deletePhoto(photo: Photo): void {
    this.memberService.deletePhoto(photo).subscribe({
      next: () => {
        this.member.photos = this.member.photos.filter(
          (x) => x.id !== photo.id
        );
        this.filteredPhotos = [...this.member.photos];
        this.memberChange.emit(this.member);
      },
    });
  }

  setMainPhoto(photo: Photo): void {
    this.memberService.setMainPhoto(photo).subscribe({
      next: () => {
        const user = this.accountService.currentUser();
        if (user) {
          user.photoUrl = photo.url;
          this.accountService.setCurrentUser(user);
        }
        this.member.photos.forEach((p) => (p.isMain = p.id === photo.id));
        this.memberChange.emit(this.member);
      },
    });
  }
}
