// -Angular-
import { Component } from '@angular/core';
// -NgxBootstrap-
import { TabsModule } from 'ngx-bootstrap/tabs';
// -Components-
import { UserManagementComponent } from '../../../components/admin/user-management/user-management.component';
import { PhotoManagemetComponent } from '../../../components/admin/photo-managemet/photo-managemet.component';
// -Directives-
import { HasRoleDirective } from '../../../directives/has-role.directive';

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
