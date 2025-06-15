import {
  ChangeDetectorRef,
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  inject,
  OnChanges,
  SimpleChanges,
} from '@angular/core';
import { CommonModule, NgClass, NgStyle } from '@angular/common';
import { Member } from '../../../../../core/models/member';
import { Photo } from '../../../../../core/models/photo';
import { FileUploadModule, FileUploader } from 'ng2-file-upload';
import { MembersService } from '../../../members.service';
import { environment } from '../../../../../../environments/environment';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';
import { AuthStoreService } from '../../../../../core/services/auth-store.service';
import {
  BehaviorSubject,
  catchError,
  combineLatest,
  map,
  of,
  take,
  tap,
} from 'rxjs';
import { PHOTOS_API } from '../../../../../core/constants/servicesConstants/photoServiceConstant';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ConfirmDialogComponent } from '../../../../../shared/components/modals/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-photo-editor',
  imports: [
    NgStyle,
    NgClass,
    FileUploadModule,
    NgSelectModule,
    FormsModule,
    CommonModule,
  ],
  templateUrl: './photo-editor.component.html',
  styleUrl: './photo-editor.component.css',
})
export class PhotoEditorComponent implements OnInit, OnChanges {
  private authStore = inject(AuthStoreService);
  private memberService = inject(MembersService);
  private cdr = inject(ChangeDetectorRef);
  private modalService = inject(BsModalService);

  @Input() member!: Member;
  @Output() memberChange = new EventEmitter<Member>();

  uploader?: FileUploader;
  hasBaseDropZoneOver = false;
  bsModalRef?: BsModalRef;

  tags: { id: number; name: string }[] = [];
  photoTags: { [photoId: number]: string[] } = {};
  currentTags: string[] = [];
  searchTags: string[] = [];

  private selectedTags$ = new BehaviorSubject<string[]>([]);
  private approvalFilter$ = new BehaviorSubject<boolean | null>(null);
  private photos$ = new BehaviorSubject<Photo[]>([]);

  filteredPhotos$ = combineLatest([
    this.photos$,
    this.selectedTags$,
    this.approvalFilter$,
  ]).pipe(
    map(([photos, selectedTags, approvalFilter]) =>
      this.filterPhotos(photos, selectedTags, approvalFilter)
    )
  );

  ngOnInit(): void {
    this.initializeUploader();
    this.loadTags();
    this.photos$.next(this.member.photos);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['member'] && this.member?.photos) {
      this.photos$.next(this.member.photos);
      this.member.photos.forEach((photo) => this.loadTagsForPhoto(photo.id));
    }
  }

  getApprovalValue(event: Event): boolean | null {
    const value = (event.target as HTMLSelectElement).value;
    return value === 'all' ? null : value === 'true';
  }

  fileOverBase(e: any): void {
    this.hasBaseDropZoneOver = e;
  }

  initializeUploader(): void {
    this.uploader = new FileUploader({
      url: `${environment.apiBaseUrl}` + PHOTOS_API.ADD_PHOTO,
      authToken: `Bearer ${this.authStore.getCurrentUser()?.token}`,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024,
    });

    this.uploader.onBuildItemForm = (fileItem, form) => {
      this.currentTags.forEach((tag) => form.append('Tags', tag));
    };

    this.uploader.onSuccessItem = (item, response) => {
      const photo = JSON.parse(response) as Photo;
      const updatedPhotos = [...this.photos$.value, photo];
      this.photos$.next(updatedPhotos);
      this.loadTagsForPhoto(photo.id);
      this.currentTags = [];
      this.emitMemberChange(updatedPhotos);
      this.cdr.detectChanges();
    };
  }

  onTagsChanged(tags: string[]): void {
    this.searchTags = tags;
    this.selectedTags$.next(tags);
  }

  onApprovalFilterChanged(status: boolean | null): void {
    this.approvalFilter$.next(status);
  }

  loadTags(): void {
    this.memberService
      .getTags()
      .pipe(
        tap((response) => (this.tags = response)),
        catchError((err) => {
          console.error('Error fetching tags:', err);
          return of([]);
        })
      )
      .subscribe();
  }

  loadTagsForPhoto(photoId: number): void {
    this.memberService
      .getTagsForPhoto(photoId)
      .pipe(
        tap((tags) => (this.photoTags[photoId] = tags || [])),
        catchError(() => {
          this.photoTags[photoId] = [];
          return of([]);
        })
      )
      .subscribe();
  }

  deletePhoto(photo: Photo): void {
    this.memberService
      .deletePhoto(photo)
      .pipe(
        tap(() => {
          const updated = this.photos$.value.filter((p) => p.id !== photo.id);
          this.photos$.next(updated);
          this.emitMemberChange(updated);
        })
      )
      .subscribe();
  }

  openDeleteModal(photo: Photo) {
    this.bsModalRef = this.modalService.show(ConfirmDialogComponent, {
      initialState: {
        title: 'Delete Photo',
        message: 'Are you sure you want to delete this photo?',
        btnOkText: 'Delete',
        btnCancelText: 'Cancel',
      },
    });

    this.bsModalRef.onHidden?.pipe(take(1)).subscribe(() => {
      if (this.bsModalRef?.content?.result === true) {
        this.deletePhoto(photo);
      }
    });
  }

  setMainPhoto(photo: Photo): void {
    this.memberService
      .setMainPhoto(photo)
      .pipe(
        tap(() => {
          const user = this.authStore.getCurrentUser();
          if (user) {
            user.photoUrl = photo.url;
            this.authStore.updateCurrentUser(user);
          }
          const updated = this.photos$.value.map((p) => ({
            ...p,
            isMain: p.id === photo.id,
          }));
          this.photos$.next(updated);
          this.emitMemberChange(updated);
        })
      )
      .subscribe();
  }

  resetFilter(): void {
    this.currentTags = [];
    this.selectedTags$.next([]);
    this.approvalFilter$.next(null);
  }

  private emitMemberChange(updatedPhotos: Photo[]): void {
    this.memberChange.emit({
      ...this.member,
      photos: updatedPhotos,
    });
  }

  private filterPhotos(
    photos: Photo[],
    selectedTags: string[],
    approvalFilter: boolean | null
  ): Photo[] {
    const isTagFilterActive = selectedTags.length > 0;
    const isApprovalFilterActive = approvalFilter !== null;

    return photos.filter((photo) => {
      const hasTags =
        !isTagFilterActive ||
        selectedTags.every((tag) => this.photoTags[photo.id]?.includes(tag));
      const approved =
        !isApprovalFilterActive || photo.isApproved === approvalFilter;
      return hasTags && approved;
    });
  }
}
