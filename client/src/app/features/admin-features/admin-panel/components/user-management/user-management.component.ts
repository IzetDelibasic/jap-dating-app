// -Angular-
import { Component, inject, OnInit } from '@angular/core';
// -Service-
import { AdminService } from '../../admin.service';
// -Models-
import { User } from '../../../../../shared/models/user';
// -Modals-
import { RolesModalComponent } from '../../../../../shared/components/modals/roles-modal/roles-modal.component';
// -NgxBootstrap-
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-user-management',
  imports: [],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css',
})
export class UserManagementComponent implements OnInit {
  private adminService = inject(AdminService);
  private modalService = inject(BsModalService);
  users: User[] = [];
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef();

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  openRolesModal(user: User) {
    const initialState: ModalOptions = {
      class: 'modal-lg',
      initialState: {
        title: 'User roles',
        username: user.username,
        selectedRoles: [...user.roles],
        availableRoles: ['Admin', 'Moderator', 'Member'],
        users: this.users,
        rolesUpdated: false,
      },
    };

    this.bsModalRef = this.modalService.show(RolesModalComponent, initialState);
    this.bsModalRef.onHide?.subscribe({
      next: () => {
        if (this.bsModalRef.content && this.bsModalRef.content.rolesUpdated) {
          const selectedRoles = this.bsModalRef.content.selectedRoles;
          if (user.username) {
            this.adminService
              .updateUserRoles(user.username, selectedRoles)
              .subscribe({
                next: (roles) => (user.roles = roles),
              });
          } else {
            console.error('Username is undefined');
          }
        }
      },
    });
  }

  getUsersWithRoles() {
    this.adminService.getUserWithRoles().subscribe({
      next: (users) => (this.users = users),
    });
  }
}
