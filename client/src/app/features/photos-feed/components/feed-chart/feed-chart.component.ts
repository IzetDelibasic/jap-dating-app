import { Component, inject, Input, OnDestroy, OnInit } from '@angular/core';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { PhotosFeedService } from '../../photos-feed.service';
import { Photo } from '../../../../core/models/photo';
import { Observable, Subscription } from 'rxjs';

@Component({
  selector: 'app-feed-chart',
  imports: [NgxChartsModule],
  templateUrl: './feed-chart.component.html',
  styleUrl: './feed-chart.component.css',
})
export class FeedChartComponent implements OnInit, OnDestroy {
  @Input() userId?: number;
  @Input() photos$?: Observable<Photo[]>;

  private photosFeedService = inject(PhotosFeedService);

  pieChartData = [
    { name: 'Approved', value: 0 },
    { name: 'Unapproved', value: 0 },
  ];
  chartView: [number, number] = [450, 450];

  totalPhotos = 0;
  approvedPhotos = 0;
  unapprovedPhotos = 0;
  private photosSub?: Subscription;

  private resizeListener = () => this.setResponsiveChartView();

  ngOnInit(): void {
    this.setResponsiveChartView();
    window.addEventListener('resize', this.resizeListener);

    if (this.photos$) {
      this.photosSub = this.photos$.subscribe((photos) => {
        this.updateChartData(photos);
      });
    }
  }

  ngOnDestroy(): void {
    window.removeEventListener('resize', this.resizeListener);
    this.photosSub?.unsubscribe();
  }

  setResponsiveChartView() {
    if (window.innerWidth < 600) {
      this.chartView = [300, 250];
    } else {
      this.chartView = [850, 350];
    }
  }

  private updateChartData(photos: Photo[]) {
    this.totalPhotos = photos.length;
    this.approvedPhotos = photos.filter((p) => p.isApproved).length;
    this.unapprovedPhotos = photos.filter((p) => !p.isApproved).length;
    this.pieChartData = [
      { name: 'Approved', value: this.approvedPhotos },
      { name: 'Unapproved', value: this.unapprovedPhotos },
    ];
  }
}
