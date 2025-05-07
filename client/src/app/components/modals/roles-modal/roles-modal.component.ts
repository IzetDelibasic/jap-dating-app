// -Angular-
import { Component, inject } from '@angular/core';
// -NgxBootstrap-
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-roles-modal',
  imports: [],
  templateUrl: './roles-modal.component.html',
  styleUrl: './roles-modal.component.css',
})
export class RolesModalComponent {
  bsModalRef = inject(BsModalRef);
  title = '';
  username = '';
  selectedRoles: string[] = [];
  availableRoles: string[] = [];
  rolesUpdated = false;

  updateChecked(checkedValue: string) {
    if (this.selectedRoles.includes(checkedValue)) {
      this.selectedRoles = this.selectedRoles.filter((r) => r !== checkedValue);
    } else {
      this.selectedRoles.push(checkedValue);
    }
  }

  onSelectRoles() {
    this.rolesUpdated = true;
    this.bsModalRef.hide();
  }
}
