import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavbarComponent } from './shared/components/navbar/navbar.component';
import { NgxSpinnerComponent } from 'ngx-spinner';
import { LikesService } from './core/services/likes.service';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NavbarComponent, NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent implements OnInit{
  private likesService = inject(LikesService);

  ngOnInit(): void {
    this.likesService.initializeLikeIds();
  }
}
