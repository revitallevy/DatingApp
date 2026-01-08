import { Component, ElementRef, inject, OnInit, AfterViewInit, signal, ViewChild } from '@angular/core';
import { AdminService } from '../../../core/services/admin-service';
import { User } from '../../../types/user';

@Component({
  selector: 'app-user-management',
  imports: [],
  templateUrl: './user-management.html',
  styleUrl: './user-management.css',
})
export class UserManagement implements OnInit, AfterViewInit {
  @ViewChild('rolesModal') rolesModal!: ElementRef<HTMLDialogElement>;
  private adminService = inject(AdminService);
  protected users = signal<User[]>([]);
  protected availableRoles = ['Member', 'Moderator', 'Admin'];
  protected selectedUser: User | null = null;

  ngOnInit(): void {
    this.getUserWithRoles();
  }

  getUserWithRoles() {
    this.adminService.getUserWithRoles().subscribe({
      next: users => this.users.set(users)
    });
  }

  openRolesModal(user: User) {
    this.selectedUser = { ...user, roles: [...(user.roles ?? [])] } as User;
    this.rolesModal.nativeElement.showModal();
  }

  ngAfterViewInit(): void {
    this.rolesModal.nativeElement.addEventListener('close', () => {
      this.selectedUser = null;
    });
  }

  toggleRole(event: Event, role: string) {
    if (!this.selectedUser) return;

    const isChecked = (event.target as HTMLInputElement).checked;

    if (isChecked) {
      this.selectedUser.roles.push(role);
    } else {
      this.selectedUser.roles = this.selectedUser.roles.filter(r => r !== role);
    }
  }

  updateRoles() {
    if (!this.selectedUser) return;

    this.adminService.updateUserRoles(this.selectedUser.id, this.selectedUser.roles).subscribe({
      next: updatedRoles => {
        this.users.update(users => users.map(u => {
          if (u.id === this.selectedUser?.id) u.roles = updatedRoles;
          return u;
        }));
        this.rolesModal.nativeElement.close();
      },
      error: error => console.error('Failed to update roles', error)
    })
  }
}
