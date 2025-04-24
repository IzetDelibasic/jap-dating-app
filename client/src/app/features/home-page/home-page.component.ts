import { Component, inject, OnInit } from '@angular/core';
import { RegisterFormComponent } from '../../components/register-form/register-form.component';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home-page',
  imports: [RegisterFormComponent],
  templateUrl: './home-page.component.html',
  styleUrl: './home-page.component.css',
})
export class HomePageComponent implements OnInit {
  registerMode = false;
  users: any;
  http = inject(HttpClient);

  ngOnInit(): void {
    this.getUsers();
  }

  registerToggle() {
    console.log(this.registerMode);
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = !event;
  }

  getUsers() {
    this.http.get(environment.apiBaseUrl + 'user').subscribe({
      next: (response) => (this.users = response),
      error: (error) => console.log(error),
      complete: () => console.log('Request has completed'),
    });
  }
}
