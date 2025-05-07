// -Angular-
import { Component } from '@angular/core';
// -Components-
import { RegisterFormComponent } from '../../components/forms/register-form/register-form.component';

@Component({
  selector: 'app-home-page',
  imports: [RegisterFormComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css',
})
export class HomePageComponent {
  registerMode = true;

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = !event;
  }
}
