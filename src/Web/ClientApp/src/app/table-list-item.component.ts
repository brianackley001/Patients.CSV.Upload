 import { Component, Input } from '@angular/core';
 import { NgbHighlight, NgbModal } from '@ng-bootstrap/ng-bootstrap';
 
import { MmDdYYYYDatePipe } from './pipes/mm--dd-yyyy-date.pipe';
import { Patient } from './dataModels/patient';

@Component({
  selector: 'tr[table-list-item]',
  standalone: true,
  imports: [ MmDdYYYYDatePipe,],
  templateUrl: './table-list-item.component.html',
  styleUrl: './table-list-item.component.css'
})
export class TableListItem{
  @Input() patient!: Patient;
  @Input() ngbResult!: string;
  @Input() ngbSearchTerm!: string;
}
