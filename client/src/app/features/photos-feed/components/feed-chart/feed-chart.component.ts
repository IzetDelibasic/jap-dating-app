import {
  Component,
  inject,
  Input,
  OnChanges,
  OnDestroy,
  OnInit,
  SimpleChanges,
} from '@angular/core';
import { NgxChartsModule } from '@swimlane/ngx-charts';
import { PhotosFeedService } from '../../photos-feed.service';
import { Photo } from '../../../../core/models/photo';

@Component({
  selector: 'app-feed-chart',
  imports: [NgxChartsModule],
  templateUrl: './feed-chart.component.html',
  styleUrl: './feed-chart.component.css',
})
export class FeedChartComponent implements OnChanges, OnInit, OnDestroy {
  @Input() userId?: number;
  private photosFeedService = inject(PhotosFeedService);

  pieChartData = [
    { name: 'Approved', value: 0 },
    { name: 'Unapproved', value: 0 },
  ];
  chartView: [number, number] = [450, 450];

  totalPhotos = 0;
  approvedPhotos = 0;
  unapprovedPhotos = 0;

  private resizeListener = () => this.setResponsiveChartView();

  ngOnInit(): void {
    this.setResponsiveChartView();
    window.addEventListener('resize', this.resizeListener);
  }

  ngOnDestroy(): void {
    window.removeEventListener('resize', this.resizeListener);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['userId'] && this.userId !== undefined) {
      this.photosFeedService
        .getPhotosByUserId(this.userId)
        .subscribe((photos: Photo[]) => {
          this.totalPhotos = photos.length;
          this.approvedPhotos = photos.filter((p) => p.isApproved).length;
          this.unapprovedPhotos = photos.filter((p) => !p.isApproved).length;
          this.pieChartData = [
            { name: 'Approved', value: this.approvedPhotos },
            { name: 'Unapproved', value: this.unapprovedPhotos },
          ];
        });
    }
  }

  setResponsiveChartView() {
    if (window.innerWidth < 600) {
      this.chartView = [300, 250];
    } else {
      this.chartView = [850, 350];
    }
  }
}
