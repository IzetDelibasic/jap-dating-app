import { Component } from '@angular/core';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ManageUserComponent } from '../../components/manage-user/manage-user.component';
import { ManagePhotoComponent } from '../../components/manage-photo/manage-photo.component';
import { HasRoleDirective } from '../../../../../shared/directives/has-role.directive';

@Component({
  selector: 'app-admin-panel',
  imports: [
    TabsModule,
    HasRoleDirective,
    ManagePhotoComponent,
    ManageUserComponent,
  ],
  templateUrl: './admin-panel-page.component.html',
  styleUrl: './admin-panel-page.component.css',
})
export class AdminPanelPageComponent {}
