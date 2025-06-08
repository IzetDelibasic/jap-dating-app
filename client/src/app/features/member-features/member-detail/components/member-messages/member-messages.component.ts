import {
  AfterViewChecked,
  Component,
  inject,
  input,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { MessageService } from '../../../../../core/services/message.service';
import { TimeagoModule } from 'ngx-timeago';
import { DEFAULT_PHOTO_URL } from '../../../../../core/constants/contentConstants/imagesConstant';
import { ActivatedRoute } from '@angular/router';
import { catchError, EMPTY, tap } from 'rxjs';

@Component({
  selector: 'app-member-messages',
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css',
})
export class MemberMessagesComponent implements AfterViewChecked, OnInit {
  @ViewChild('messageForm') messageForm?: NgForm;
  @ViewChild('scrollMe') scrollContainer?: any;
  messageService = inject(MessageService);
  username = input.required<string>();
  messageContent = '';
  userInterests = '';
  userLookingFor = '';

  defaultPhoto = DEFAULT_PHOTO_URL;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      const member = data['member'];
      if (member) {
        this.userInterests = member.interests;
        this.userLookingFor = member.lookingFor;
      }
    });
  }

  sendMessage() {
    this.messageService
      .sendMessage(this.username(), this.messageContent)
      .then(() => {
        this.messageForm?.reset();
        this.scrollToBottom();
      });
  }

  generateMessage(): void {
    this.messageService
      .generateMessage(this.userInterests, this.userLookingFor)
      .pipe(
        tap((response) => {
          this.messageContent = response.message;
        }),
        catchError((err) => {
          console.error('Error generating message:', err);
          return EMPTY;
        })
      )
      .subscribe();
  }

  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  private scrollToBottom() {
    if (this.scrollContainer) {
      this.scrollContainer.nativeElement.scrollTop =
        this.scrollContainer.nativeElement.scrollHeight;
    }
  }
}
