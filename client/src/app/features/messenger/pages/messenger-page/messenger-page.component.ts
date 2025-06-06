import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Component, inject, OnInit } from '@angular/core';
import { MessageService } from '../../../../core/services/message.service';
import { TimeagoModule } from 'ngx-timeago';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { Message } from '../../../../core/models/message';
import { catchError, EMPTY, tap } from 'rxjs';

@Component({
  selector: 'app-messenger-page',
  imports: [
    ButtonsModule,
    FormsModule,
    TimeagoModule,
    RouterLink,
    PaginationModule,
  ],
  templateUrl: './messenger-page.component.html',
  styleUrl: './messenger-page.component.css',
})
export class MessengerPageComponent implements OnInit {
  messageService = inject(MessageService);
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;
  isOutbox = this.container === 'Outbox';

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
  this.messageService
    .getMessages(this.pageNumber, this.pageSize, this.container)
    .pipe(
      tap((response) => console.log('Messages loaded:', response)),
      catchError((err) => {
        console.error('Error loading messages:', err);
        return EMPTY;
      })
    )
    .subscribe();
}

  getRoute(message: Message) {
    if (this.container === 'Outbox')
      return `/member-details/${message.recipientUsername}`;
    else return `/member-details/${message.senderUsername}`;
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadMessages();
    }
  }

  deleteMessage(id: number) {
    this.messageService.deleteMessage(id).subscribe({
      next: () => {
        this.messageService.paginatedResult.update((prev) => {
          if (prev && prev.items) {
            prev.items.splice(
              prev.items.findIndex((m) => m.id === id),
              1
            );
            return prev;
          }
          return prev;
        });
      },
    });
  }
}
