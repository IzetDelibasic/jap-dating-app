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
import { BehaviorSubject, catchError, of, tap } from 'rxjs';

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

  tags$ = new BehaviorSubject<{ id: number; name: string }[]>([]);
  photoTags$ = new BehaviorSubject<{ [photoId: number]: string[] }>({});
  filteredPhotos$ = new BehaviorSubject<Photo[]>([]);
  searchTags$ = new BehaviorSubject<string[]>([]);
  selectedTags$ = new BehaviorSubject<string[]>([]);
  errorMessage = '';

  ngOnInit(): void {
    this.initializeUploader();
    this.loadTags();
    this.filteredPhotos$.next([...this.member.photos]);
    this.member.photos.forEach((photo) => this.loadTagsForPhoto(photo.id));
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
      this.selectedTags$.value.forEach((tag) => {
        form.append('Tags', tag);
      });
    };

    this.uploader.onSuccessItem = (item, response) => {
      const photo = JSON.parse(response);
      this.member.photos.push(photo);
      this.filteredPhotos$.next([...this.member.photos]);
      this.memberChange.emit(this.member);

      this.loadTagsForPhoto(photo.id);

      this.selectedTags$.next([]);
      this.cdr.detectChanges();
    };
  }

  loadTags(): void {
    this.memberService
      .getTags()
      .pipe(
        tap((response) => this.tags$.next(response)),
        catchError((err) => {
          console.error('Error fetching tags:', err);
          this.errorMessage = 'Failed to load tags.';
          return of([]);
        })
      )
      .subscribe();
  }

  loadTagsForPhoto(photoId: number): void {
    this.memberService
      .getTagsForPhoto(photoId)
      .pipe(
        tap((tags) => {
          const currentTags = this.photoTags$.value;
          currentTags[photoId] = tags || [];
          this.photoTags$.next(currentTags);
        }),
        catchError(() => {
          const currentTags = this.photoTags$.value;
          currentTags[photoId] = [];
          this.photoTags$.next(currentTags);
          return of([]);
        })
      )
      .subscribe();
  }

  searchPhotosByTag(): void {
    const searchTags = this.searchTags$.value;
    if (!searchTags || searchTags.length === 0) {
      this.filteredPhotos$.next([...this.member.photos]);
      return;
    }

    const filtered = this.member.photos.filter((photo) =>
      searchTags.every((tag) => this.photoTags$.value[photo.id]?.includes(tag))
    );
    this.filteredPhotos$.next(filtered);
  }

  resetFilter(): void {
    this.filteredPhotos$.next([...this.member.photos]);
    this.searchTags$.next([]);
  }

  deletePhoto(photo: Photo): void {
    this.memberService
      .deletePhoto(photo)
      .pipe(
        tap(() => {
          this.member.photos = this.member.photos.filter(
            (x) => x.id !== photo.id
          );
          this.filteredPhotos$.next([...this.member.photos]);
          this.memberChange.emit(this.member);
        }),
        catchError((err) => {
          console.error('Failed to delete photo:', err);
          this.errorMessage = 'Failed to delete photo.';
          return of(null);
        })
      )
      .subscribe();
  }

  setMainPhoto(photo: Photo): void {
    this.memberService
      .setMainPhoto(photo)
      .pipe(
        tap(() => {
          const user = this.accountService.currentUser();
          if (user) {
            user.photoUrl = photo.url;
            this.accountService.setCurrentUser(user);
          }
          this.member.photos.forEach((p) => (p.isMain = p.id === photo.id));
          this.memberChange.emit(this.member);
        }),
        catchError((err) => {
          console.error('Failed to set main photo:', err);
          this.errorMessage = 'Failed to set main photo.';
          return of(null);
        })
      )
      .subscribe();
  }
}
