import {
  Directive,
  inject,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { AuthStoreService } from '../../core/services/auth-store.service';

@Directive({
  selector: '[appHasRole]',
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[] = [];
  private authStore = inject(AuthStoreService);
  private viewContainerRef = inject(ViewContainerRef);
  private templateRef = inject(TemplateRef);

  ngOnInit(): void {
    if (!this.appHasRole || this.appHasRole.length === 0) {
      this.viewContainerRef.clear();
      return;
    }

    if (this.hasRequiredRole()) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }

  private hasRequiredRole(): boolean {
    return this.authStore.hasRole(this.appHasRole);
  }
}
