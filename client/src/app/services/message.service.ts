import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { PaginatedResult } from '../models/pagination';
import { Message } from '../models/message';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  private http = inject(HttpClient);
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);

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

  sendMessage(username: string, content: string) {
    return this.http.post<Message>(environment.apiBaseUrl + 'messages', {
      recipientUsername: username,
      content: content,
    });
  }

  deleteMessage(id: number) {
    return this.http.delete(environment.apiBaseUrl + `messages/${id}`);
  }
}
