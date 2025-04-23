import { Component } from '@angular/core';

@Component({
  selector: 'app-home-page',
  imports: [],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css',
})
export class HomePageComponent {
  registerMode = false;

  registerToggle() {
    console.log(this.registerMode);
    this.registerMode = !this.registerMode;
  }
}
