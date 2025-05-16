import {
  Directive,
  inject,
  Input,
  OnInit,
  TemplateRef,
  ViewContainerRef,
} from '@angular/core';
import { AccountService } from '../../core/services/account.service';

@Directive({
  selector: '[appHasRole]',
})
export class HasRoleDirective implements OnInit {
  @Input() appHasRole: string[] = [];
  private accountService = inject(AccountService);
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
    const roles = this.accountService.roles();
    return roles?.some((r: string) => this.appHasRole.includes(r)) ?? false;
  }
}
