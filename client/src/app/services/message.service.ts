import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { PaginatedResult } from '../models/pagination';
import { Message } from '../models/message';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { environment } from '../environments/environment';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { User } from '../models/user';
import { Group } from '../models/group';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private http = inject(HttpClient);
  hubConnection?: HubConnection;
  messageTrehad = signal<Message[]>([]);
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);

  createHubConnection(user: User, otherUsername: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubsUrl + 'messages?user=' + otherUsername, {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('ReceiveMessageThread', (messages) => {
      this.messageTrehad.set(messages);
    });

    this.hubConnection.on('NewMessage', (message) => {
      this.messageTrehad.update((messages) => [...messages, message]);
    });

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if (group.connections.some((x) => x.username === otherUsername)) {
        this.messageTrehad.update((messages) => {
          messages.forEach((message) => {
            if (!message.dateRead) {
              message.dateRead = new Date(Date.now());
            }
          });
          return messages;
        });
      }
    });
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch((error) => console.log(error));
    }
  }

  getMessages(pageNumber: number, pageSize: number, container: string) {
    let params = setPaginationHeaders(pageNumber, pageSize);

    params = params.append('Container', container);

    return this.http
      .get<Message[]>(environment.apiBaseUrl + 'messages', {
        observe: 'response',
        params,
      })
      .subscribe({
        next: (response) =>
          setPaginatedResponse(response, this.paginatedResult),
      });
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
}
