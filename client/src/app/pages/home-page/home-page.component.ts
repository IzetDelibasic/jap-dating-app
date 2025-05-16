import { Component } from '@angular/core';
import { RegisterFormComponent } from '../../features/auth/components/register-form/register-form.component';
import { HERO_VIDEO_URL } from '../../core/constants/contentConstants/videosConstant';

@Component({
  selector: 'app-home-page',
  imports: [RegisterFormComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css',
})
export class HomePageComponent {
  registerMode = true;
  heroVideo = HERO_VIDEO_URL;

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = !event;
  }
}
