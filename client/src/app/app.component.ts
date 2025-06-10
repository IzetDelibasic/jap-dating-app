import { LoadingService } from './core/services/loading.service';
import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { LikesService } from './core/services/likes.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit {
  private likesService = inject(LikesService);

  constructor(public loadingService: LoadingService) {}

  ngOnInit(): void {
    this.likesService.initializeLikeIds();
  }
}
