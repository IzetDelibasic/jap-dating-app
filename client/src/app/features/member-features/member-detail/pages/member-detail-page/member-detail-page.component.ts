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
import { AccountService } from '../../../../../core/services/account.service';
import { LikesService } from '../../../../../core/services/likes.service';

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
  private accountService = inject(AccountService);
  private likeService = inject(LikesService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;

  ngOnInit(): void {
    this.route.data.subscribe({
      next: (data) => {
        console.log(data);
        this.member = data['member'];
        this.member &&
          this.member.photos.map((p) => {
            this.images.push(new ImageItem({ src: p.url, thumb: p.url }));
          });
      },
    });

    this.route.paramMap.subscribe({
      next: () => this.onRouteParamsChange(),
    });

    this.route.queryParams.subscribe({
      next: (params) => {
        params['tab'] && this.selectTab(params['tab']);
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
    const user = this.accountService.currentUser();
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
      const user = this.accountService.currentUser();
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
