import { Component, signal } from '@angular/core';
import { ApiError } from '../../../types/error';

@Component({
  selector: 'app-server-error',
  imports: [],
  templateUrl: './server-error.html',
  styleUrl: './server-error.css',
})
export class ServerError {
  protected error = signal<ApiError | undefined>(history.state?.error);
  protected showDetails = false;

  // constructor() {
  //   const navigation = this.router.getCurrentNavigation();
  //   this.error = navigation?.extras?.state?.['error'];
  // }

  detailsToggle() {
    this.showDetails = !this.showDetails;
  }
}
