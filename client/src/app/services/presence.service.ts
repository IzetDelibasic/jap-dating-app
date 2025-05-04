import { inject, Injectable, signal } from '@angular/core';
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { User } from '../models/user';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  private hubConnection?: HubConnection;
  private toastr = inject(ToastrService);
  onlineUsers = signal<string[]>([]);

  crateHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(environment.hubsUrl + 'presence', {
        accessTokenFactory: () => user.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().catch((error) => console.log(error));

    this.hubConnection.on('UserIsOnline', (username) => {
      this.toastr.info(username + ' has connected');
    });

    this.hubConnection.on('UserIsOffline', (username) => {
      this.toastr.warning(username + ' has disconnected');
    });

    this.hubConnection.on('GetOnlineUsers', (usernames) => {
      this.onlineUsers.set(usernames);
    });
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch((error) => console.log(error));
    }
  }
}
