import { Component, inject, OnInit, signal } from '@angular/core';
import { AdminService } from '../../../core/services/admin-service';
import { Photo } from '../../../types/member';

@Component({
  selector: 'app-photo-management',
  imports: [],
  templateUrl: './photo-management.html',
  styleUrl: './photo-management.css',
})
export class PhotoManagement implements OnInit {
  private adminService = inject(AdminService);
  photos = signal<Photo[]>([]);

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval() {
    this.adminService.getPhotosForApproval().subscribe({
      next: photos => this.photos.set(photos)
    })
  }

  approvePhoto(photoId: number) {
    this.adminService.approvePhoto(photoId).subscribe({
      next: () => this.photos.update(photos => {
        return photos.filter(photo => photo.id !== photoId);
      })
    })
  }

  rejectPhoto(photoId: number) {
    this.adminService.rejectPhoto(photoId).subscribe({
      next: () => this.photos.update(photos => {
        return photos.filter(photo => photo.id !== photoId);
      })
    })
  }
}
