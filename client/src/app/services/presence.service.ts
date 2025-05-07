// -Angular-
import { inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
// -Signalr-
import {
  HubConnection,
  HubConnectionBuilder,
  HubConnectionState,
} from '@microsoft/signalr';
// -Ngx-
import { ToastrService } from 'ngx-toastr';
// -Models-
import { User } from '../models/user';
// -Environment-
import { environment } from '../environments/environment';
// -Rxjs-
import { take } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PresenceService {
  private hubConnection?: HubConnection;
  private toastr = inject(ToastrService);
  private router = inject(Router);
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
      this.onlineUsers.update((users) => [...users, username]);
    });

    this.hubConnection.on('UserIsOffline', (username) => {
      this.onlineUsers.update((users) => users.filter((x) => x !== username));
    });

    this.hubConnection.on('GetOnlineUsers', (usernames) => {
      this.onlineUsers.set(usernames);
    });

    this.hubConnection.on('NewMessageReceived', ({ username, knownAs }) => {
      this.toastr
        .info(knownAs + ' has sent you a new message! Click me to see it')
        .onTap.pipe(take(1))
        .subscribe(() =>
          this.router.navigateByUrl('/members/' + username + '?tab=Messages')
        );
    });
  }

  stopHubConnection() {
    if (this.hubConnection?.state === HubConnectionState.Connected) {
      this.hubConnection.stop().catch((error) => console.log(error));
    }
  }
}
