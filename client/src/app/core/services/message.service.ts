import { MESSAGES_API } from '../constants/servicesConstants/messagesServiceConstant';
import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { PaginatedResult } from '../models/pagination';
import { Message } from '../models/message';
import { Group } from '../models/group';
import { User } from '../models/user';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { environment } from '../../../environments/environment';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { Observable, tap } from 'rxjs';

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

    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('ReceiveMessageThread', (messages) => {
      this.messageThread.set(messages);
    });

    this.hubConnection.on('NewMessage', (message) => {
      this.messageThread.update((messages) => [...messages, message]);
    });

    this.hubConnection.on('UpdatedGroup', (group: Group) => {
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
      .get<Message[]>(environment.apiBaseUrl + MESSAGES_API.BASE, {
        observe: 'response',
        params,
      })
      .pipe(
        tap((response) => setPaginatedResponse(response, this.paginatedResult))
      );
  }

  generateMessage(
    interests: string,
    lookingFor: string
  ): Observable<{ message: string }> {
    return this.http.post<{ message: string }>(
      environment.apiBaseUrl + MESSAGES_API.GENERATE_MESSAGE,
      {
        interests,
        lookingFor,
      }
    );
  }

  getMessageThread(username: string) {
    return this.http
      .get<Message[]>(environment.apiBaseUrl + MESSAGES_API.THREAD(username))
      .pipe(tap((messages) => this.messageThread.set(messages)));
  }

  async sendMessage(username: string, content: string) {
    if (!this.hubConnection) {
      throw new Error('Hub connection not initialized');
    }

    return this.hubConnection.invoke('SendMessage', {
      recipientUsername: username,
      content,
    });
  }

  deleteMessage(id: number) {
    return this.http.delete(environment.apiBaseUrl + MESSAGES_API.DELETE(id));
  }
}
