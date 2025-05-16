import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NOT_FOUND_IMAGE_URL } from '../../core/constants/contentConstants/imagesConstant';

@Component({
  selector: 'app-not-found',
  imports: [RouterLink],
  templateUrl: './not-found.component.html',
  styleUrl: './not-found.component.css',
})
export class NotFoundComponent {
  notFoundImageUrl = NOT_FOUND_IMAGE_URL;
}
