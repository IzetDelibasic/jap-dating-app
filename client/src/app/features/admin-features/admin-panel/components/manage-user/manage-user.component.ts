import { Component, inject, OnInit } from '@angular/core';
import { AdminService } from '../../admin.service';
import { User } from '../../../../../core/models/user';
import { RolesModalComponent } from '../../../../../shared/components/modals/roles-modal/roles-modal.component';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-manage-user',
  imports: [CommonModule],
  templateUrl: './manage-user.component.html',
  styleUrl: './manage-user.component.css',
})
export class ManageUserComponent implements OnInit {
  private adminService = inject(AdminService);
  private modalService = inject(BsModalService);
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef();

  users$ = this.adminService.getUserWithRoles();

  ngOnInit(): void {}

  openRolesModal(user: User) {
    this.users$.subscribe((users) => {
      const initialState: ModalOptions = {
        class: 'modal-lg',
        initialState: {
          title: 'User roles',
          username: user.username,
          selectedRoles: [...user.roles],
          availableRoles: ['Admin', 'Moderator', 'Member'],
          users: users,
          rolesUpdated: false,
        },
      };

      this.bsModalRef = this.modalService.show(
        RolesModalComponent,
        initialState
      );

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
    });
  }
}
