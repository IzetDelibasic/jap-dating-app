import { Component } from '@angular/core';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { UserManagementComponent } from '../../../components/admin/user-management/user-management.component';
import { HasRoleDirective } from '../../../directives/has-role.directive';
import { PhotoManagemetComponent } from '../../../components/admin/photo-managemet/photo-managemet.component';

@Component({
  selector: 'app-admin-panel',
  imports: [
    TabsModule,
    UserManagementComponent,
    PhotoManagemetComponent,
    HasRoleDirective,
  ],
  templateUrl: './admin-panel.component.html',
  styleUrl: './admin-panel.component.css',
})
export class AdminPanelComponent {}
