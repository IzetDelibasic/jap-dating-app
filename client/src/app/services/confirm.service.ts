// -Angular-
import { inject, Injectable } from '@angular/core';
// -Ngx-
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
// -Components-
import { ConfirmDialogComponent } from '../components/modals/confirm-dialog/confirm-dialog.component';
// -Rxjs-
import { map } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ConfirmService {
  bsModalRef?: BsModalRef;
  private modalService = inject(BsModalService);

  confirm(
    title = 'Confirmation',
    message = 'Are you sure you want to do this?',
    btnOkText = 'Ok',
    btnCancelText = 'Cancel'
  ) {
    const config: ModalOptions = {
      initialState: {
        title,
        message,
        btnOkText,
        btnCancelText,
      },
    };
    this.bsModalRef = this.modalService.show(ConfirmDialogComponent, config);
    return this.bsModalRef.onHidden?.pipe(
      map(() => {
        if (this.bsModalRef?.content) {
          return this.bsModalRef.content.result;
        } else return false;
      })
    );
  }
}
