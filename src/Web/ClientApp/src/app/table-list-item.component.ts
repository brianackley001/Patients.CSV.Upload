 import { Component, Input, inject, TemplateRef  } from '@angular/core';
 import { NgbHighlight, ModalDismissReasons, NgbModal  } from '@ng-bootstrap/ng-bootstrap';
 import { FormsModule } from '@angular/forms'; // Import FormsModule
 
import { MmDdYYYYDatePipe } from './pipes/mm--dd-yyyy-date.pipe';
import { Patient } from './dataModels/patient';

@Component({
  selector: 'tr[table-list-item]',
  standalone: true,
  imports: [ MmDdYYYYDatePipe, NgbHighlight, FormsModule],
  templateUrl: './table-list-item.component.html',
  styleUrl: './table-list-item.component.css'
})
export class TableListItem{
  @Input() patient!: Patient;
  @Input() ngbSearchTerm!: string;
	private modalService = inject(NgbModal);
	closeResult = '';
	open(content: TemplateRef<any>) {
		this.modalService.open(content, { ariaLabelledBy: 'modal-basic-title' }).result.then(
			(result) => {
				this.closeResult = `Closed with: ${result}`;
			},
			(reason) => {
				this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
			},
		);
	}

  openVerticallyCentered(content: TemplateRef<any>) {
		this.modalService.open(content, { centered: true }).result.then(
			(result) => {
				this.closeResult = `Closed with: ${result}`;
			},
			(reason) => {
				this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
			},
		);
	}

	private getDismissReason(reason: any): string {
		switch (reason) {
			case ModalDismissReasons.ESC:
				return 'by pressing ESC';
			case ModalDismissReasons.BACKDROP_CLICK:
				return 'by clicking on a backdrop';
			default:
				return `with: ${reason}`;
		}
	}
}
