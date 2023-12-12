import { AsyncPipe, DecimalPipe } from '@angular/common';
import { Component, QueryList, ViewChildren } from '@angular/core';
import { Observable } from 'rxjs';

import { Patient } from './dataModels/patient';
import { PatientService } from './services/patient.service';
import { NgbdSortableHeader, SortEvent } from './directives/sortable.directive';
import { FormsModule } from '@angular/forms';
import { NgbHighlight, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
	selector: 'ngbd-table-complete',
	standalone: true,
	imports: [DecimalPipe, FormsModule, AsyncPipe, NgbHighlight, NgbdSortableHeader, NgbPaginationModule],
	templateUrl: './table.component.html',
	providers: [PatientService, DecimalPipe],
})
export class NgbdTableComplete {
	patients$: Observable<Patient[]>;
	total$: Observable<number>;
	@ViewChildren(NgbdSortableHeader)
	headers!: QueryList<NgbdSortableHeader>;

	constructor(public service: PatientService) {
		this.patients$ = service.patients$;
		this.total$ = service.total$;
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
