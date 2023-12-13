 import { Component, Input, inject, TemplateRef, Output, EventEmitter  } from '@angular/core';
 import { NgbHighlight, NgbModal  } from '@ng-bootstrap/ng-bootstrap';
 import { FormsModule } from '@angular/forms'; 
 
import { MmDdYYYYDatePipe } from './pipes/mm--dd-yyyy-date.pipe';
import { Patient } from './dataModels/patient';

@Component({
  selector: 'tr[table-list-item]',
  standalone: true,
  imports: [ NgbHighlight, FormsModule ],
  providers: [MmDdYYYYDatePipe],
  templateUrl: './table-list-item.component.html',
  styleUrl: './table-list-item.component.css'
})
export class TableListItem{
  @Input() patient!: Patient;
  @Input() ngbSearchTerm!: string;
  @Output() savePatientEvent = new EventEmitter<Patient>();
	private modalService = inject(NgbModal);

  constructor(private mmDdYYYYDatePipe: MmDdYYYYDatePipe) {}

  ngOnInit(): void {
    // Convert birthDate to MM/DD/YYYY format, trim time provided from the API/DB for form presnetation:
    this.patient.birthDate = this.mmDdYYYYDatePipe.transform(this.patient.birthDate);
  }
  
  openVerticallyCentered(content: TemplateRef<any>) {
		this.modalService.open(content, { centered: true });
	}
  onSubmitForm() { 
    console.table(this.patient);
    // toggle the date back to ISO format for the API post:
    this.patient.birthDate = new Date(this.patient.birthDate); 
    this.savePatient();
    this.modalService.dismissAll();
  }

  savePatient() {
    this.savePatientEvent.emit(this.patient);
  }
}
