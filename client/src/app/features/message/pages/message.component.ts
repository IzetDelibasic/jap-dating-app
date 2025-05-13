// -Angular-
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Component, inject, OnInit } from '@angular/core';
// -Service-
import { MessageService } from '../../../core/services/message.service';
// -Ngx-
import { TimeagoModule } from 'ngx-timeago';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';
// -Models-
import { Message } from '../../../shared/models/message';

@Component({
  selector: 'app-message',
  imports: [
    ButtonsModule,
    FormsModule,
    TimeagoModule,
    RouterLink,
    PaginationModule,
  ],
  templateUrl: './message.component.html',
  styleUrl: './message.component.css',
})
export class MessageComponent implements OnInit {
  messageService = inject(MessageService);
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;
  isOutbox = this.container === 'Outbox';

  ngOnInit(): void {
    this.loadMessages();
  }

  loadMessages() {
    this.messageService.getMessages(
      this.pageNumber,
      this.pageSize,
      this.container
    );
  }

  getRoute(message: Message) {
    if (this.container === 'Outbox')
      return `/members/${message.recipientUsername}`;
    else return `/members/${message.senderUsername}`;
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
