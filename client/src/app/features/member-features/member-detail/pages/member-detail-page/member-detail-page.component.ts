import { Component, inject, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { HubConnectionState } from '@microsoft/signalr';
import { Member } from '../../../../../core/models/member';
import { TabDirective, TabsetComponent, TabsModule } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from 'ngx-timeago';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { MemberMessagesComponent } from '../../components/member-messages/member-messages.component';
import { MessageService } from '../../../../../core/services/message.service';
import { PresenceService } from '../../../../../core/services/presence.service';
import { catchError, combineLatest, of, tap } from 'rxjs';
import { AuthStoreService } from '../../../../../core/services/auth-store.service';

@Component({
  selector: 'app-member-detail',
  imports: [
    TabsModule,
    CommonModule,
    GalleryModule,
    TimeagoModule,
    DatePipe,
    MemberMessagesComponent,
  ],
  templateUrl: './member-detail-page.component.html',
  styleUrl: './member-detail-page.component.css',
})
export class MemberDetailPageComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', { static: true }) memberTabs?: TabsetComponent;
  presenceService = inject(PresenceService);
  private messageService = inject(MessageService);
  private authStore = inject(AuthStoreService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;

  ngOnInit(): void {
    combineLatest([
      this.route.data.pipe(
        tap((data) => {
          this.member = data['member'];
          this.member &&
            this.member.photos.map((p) => {
              this.images.push(new ImageItem({ src: p.url, thumb: p.url }));
            });
        }),
        catchError((err) => {
          console.error('Failed to load route data:', err);
          return of(null);
        })
      ),
      this.route.paramMap.pipe(tap(() => this.onRouteParamsChange())),
      this.route.queryParams.pipe(
        tap((params) => {
          params['tab'] && this.selectTab(params['tab']);
        })
      ),
    ]).subscribe({
      next: () => {
        console.log('All route observables processed successfully.');
      },
      error: (err) => {
        console.error('Error processing route observables:', err);
      },
    });
  }

  selectTab(heading: string) {
    if (this.memberTabs) {
      const messageTab = this.memberTabs.tabs.find(
        (x) => x.heading === heading
      );
      if (messageTab) messageTab.active = true;
    }
  }

  onRouteParamsChange() {
    const user = this.authStore.getCurrentUser();
    if (!user) return;
    if (
      this.messageService.hubConnection?.state ===
        HubConnectionState.Connected &&
      this.activeTab?.heading === 'Messages'
    ) {
      this.messageService.hubConnection.stop().then(() => {
        this.messageService.createHubConnection(user, this.member.userName);
      });
    }
  }

  onTabActivated(data: TabDirective) {
    this.activeTab = data;
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { tab: this.activeTab.heading },
      queryParamsHandling: 'merge',
    });
    if (this.activeTab.heading === 'Messages' && this.member) {
      const user = this.authStore.getCurrentUser();
      if (!user) return;
      this.messageService.createHubConnection(user, this.member.userName);
    } else {
      this.messageService.stopHubConnection();
    }
  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }
}
