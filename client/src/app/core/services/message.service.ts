// -Angular-
import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
// -Models-
import { PaginatedResult } from '../../shared/models/pagination';
import { Message } from '../../shared/models/message';
import { Group } from '../../shared/models/group';
import { User } from '../../shared/models/user';
// -PaginationHelper-
import { setPaginationHeaders } from './paginationHelper';
// -Environment-
import { environment } from '../../../environments/environment';
// -SignalR-
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private http = inject(HttpClient);
  hubConnection?: HubConnection;
  messageThread = signal<Message[]>([]);
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);

  createHubConnection(user: User, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubsUrl + 'messages?user=' + otherUsername, {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => this.handleError(error));

    this.hubConnection.on('ReceiveMessageThread', (messages) =>
      this.onReceiveMessageThread(messages)
    );

    this.hubConnection.on('NewMessage', (message) =>
      this.onNewMessage(message)
    );

    this.hubConnection.on('UpdatedGroup', (group) =>
      this.onUpdatedGroup(group, otherUsername)
    );
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch((error) => console.log(error));
    }
  }

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = setPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);

    return this.http.get<PaginatedResult<Message[]>>(
      environment.apiBaseUrl + 'messages',
      { observe: 'response', params }
    );
  }

  getMessageThread(username: string) {
    return this.http.get<Message[]>(
      environment.apiBaseUrl + `messages/thread/${username}`
    );
  }

  async sendMessage(username: string, content: string) {
    return this.hubConnection?.invoke('SendMessage', {
      recipientUsername: username,
      content,
    });
  }

  deleteMessage(id: number) {
    return this.http.delete(environment.apiBaseUrl + `messages/${id}`);
  }

  private handleError(error: any): void {
    console.error('MessageService Error:', error);
  }

  private onReceiveMessageThread(messages: Message[]): void {
    this.messageThread.set(messages);
  }

  private onNewMessage(message: Message): void {
    this.messageThread.update((messages) => [...messages, message]);
  }

  private onUpdatedGroup(group: Group, otherUsername: string): void {
    if (group.connections.some((x) => x.username === otherUsername)) {
      this.messageThread.update((messages) => {
        messages.forEach((message) => {
          if (!message.dateRead) {
            message.dateRead = new Date(Date.now());
          }
        });
        return messages;
      });
    }
  }
}
