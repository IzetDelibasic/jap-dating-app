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
import { MembersService } from '../../../members.service';
import { PhotoEditorComponent } from '../../components/photo-editor/photo-editor.component';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ToastrService } from 'ngx-toastr';
import { TimeagoModule } from 'ngx-timeago';
import { DEFAULT_PHOTO_URL } from '../../../../../core/constants/contentConstants/imagesConstant';
import { AuthStoreService } from '../../../../../core/services/auth-store.service';

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

  member?: Member;
  defaultPhoto = DEFAULT_PHOTO_URL;

  private authStore = inject(AuthStoreService);
  private memberService = inject(MembersService);
  private toastrService = inject(ToastrService);

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const user = this.authStore.getCurrentUser();
    if (!user || !user.username) return;
    this.memberService.getMember(user.username).subscribe({
      next: (member) => (this.member = member),
    });
  }

  updateMember() {
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: () => {
        this.toastrService.success('Profile updated successfuly');
        this.editForm?.reset(this.member);
      },
    });
  }

  onMemberChange(event: Member) {
    this.member = event;
  }
}
