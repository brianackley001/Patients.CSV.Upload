import { AsyncPipe, DecimalPipe } from '@angular/common';
import { Component, QueryList, ViewChildren } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { NgbHighlight, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

import { Patient } from './dataModels/patient';
import { PatientService } from './services/patient.service';
import { NgbdSortableHeader, SortEvent } from './directives/sortable.directive';
import { MmDdYYYYDatePipe } from './mm--dd-yyyy-date.pipe';

@Component({
	selector: 'ngbd-table-complete',
	standalone: true,
	imports: [DecimalPipe, FormsModule, AsyncPipe, NgbHighlight, 
		NgbdSortableHeader, NgbPaginationModule, MmDdYYYYDatePipe, CommonModule],
	templateUrl: './table.component.html',
	styleUrls: ['./table.component.css'],
	providers: [PatientService, DecimalPipe],
})
export class NgbdTableComplete {
	patients$: Observable<Patient[]>;
	total$: Observable<number>;
	@ViewChildren(NgbdSortableHeader)
	headers!: QueryList<NgbdSortableHeader>;
	hasItems: boolean = false;

	constructor(public service: PatientService) {
		this.patients$ = service.patients$;
		this.total$ = service.total$;
		this.total$.subscribe(val => {this.hasItems = val > 0});
	}

	onSort({ column, direction }: SortEvent) {
		// resetting other headers
		this.headers.forEach((header) => {
			if (header.sortable !== column) {
				header.direction = '';
			}
		});

		this.service.sortColumn = column;
		this.service.sortDirection = direction;
	}
}
