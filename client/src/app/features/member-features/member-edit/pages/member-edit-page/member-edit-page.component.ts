import {
  Component,
  HostListener,
  inject,
  OnInit,
  ViewChild,
} from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { Member } from '../../../../../core/models/member';
import { AccountService } from '../../../../../core/services/account.service';
import { MembersService } from '../../../members.service';
import { PhotoEditorComponent } from '../../components/photo-editor/photo-editor.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { TimeagoModule } from 'ngx-timeago';
import { DEFAULT_PHOTO_URL } from '../../../../../core/constants/contentConstants/imagesConstant';
import { BehaviorSubject, catchError, of, switchMap, tap } from 'rxjs';

@Component({
  selector: 'app-member-edit-page',
  imports: [
    TabsModule,
    FormsModule,
    PhotoEditorComponent,
    TimeagoModule,
    DatePipe,
  ],
  templateUrl: './member-edit-page.component.html',
  styleUrl: './member-edit-page.component.css',
})
export class MemberEditPageComponent implements OnInit {
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }

  defaultPhoto = DEFAULT_PHOTO_URL;
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  private toastrService = inject(ToastrService);

  private memberSubject = new BehaviorSubject<Member | null>(null);
  member$ = this.memberSubject.asObservable();

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const user = this.accountService.currentUser();
    if (!user || !user.username) return;

    this.memberService
      .getMember(user.username)
      .pipe(
        tap((member) => this.memberSubject.next(member)),
        catchError((err) => {
          console.error('Failed to load member:', err);
          return of(null);
        })
      )
      .subscribe();
  }

  updateMember() {
    this.member$
      .pipe(
        switchMap((member) => {
          if (!member || !this.editForm?.value) return of(null);
          return this.memberService.updateMember(this.editForm.value).pipe(
            tap(() => {
              this.toastrService.success('Profile updated successfully');
              this.editForm?.reset(member);
            }),
            catchError((err) => {
              console.error('Failed to update member:', err);
              this.toastrService.error('Failed to update profile');
              return of(null);
            })
          );
        })
      )
      .subscribe();
  }

  onMemberChange(event: Member) {
    this.memberSubject.next(event);
  }
}
