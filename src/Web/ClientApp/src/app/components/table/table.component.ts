import { AsyncPipe, DecimalPipe } from '@angular/common';
import { Component, QueryList, ViewChildren } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbHighlight, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

import { Patient } from '../../dataModels/patient';
import { PatientApiService } from '../../services/patient-api.service';
import { NgbdSortableHeader, SortEvent } from '../../directives/sortable.directive';
import { MmDdYYYYDatePipe } from '../../pipes/mm--dd-yyyy-date.pipe';
import { TableListItem } from '../table.list-item/table-list-item.component';	

interface State {
  filteringSearchSubmitted: boolean;
  page: number;
  pageSize: number;
  searchTerm: string;
  sortAsc: boolean;
  sortBy: string;
}
interface OnInit {
  ngOnInit(): void;
}

@Component({
  selector: 'ngbd-table-complete',
  standalone: true,
  imports: [
    DecimalPipe,
    FormsModule,
    AsyncPipe,
    NgbHighlight,
    NgbdSortableHeader,
    NgbPaginationModule,
    MmDdYYYYDatePipe,
    CommonModule,
		TableListItem,
  ],
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css'],
  providers: [DecimalPipe, PatientApiService],
})
export class NgbdTableComplete implements OnInit {
  @ViewChildren(NgbdSortableHeader)
  headers!: QueryList<NgbdSortableHeader>;
  patients: Patient[] = [];
  patientCollectionSize: number = 0;
	zeroSearchResults: boolean = false;

  constructor(private patientApiService: PatientApiService) {}

  // Internal State tracking
  private _state: State = {
    filteringSearchSubmitted: false,
    page: 1,
    pageSize: 5,
    searchTerm: '',
    sortAsc: true,
    sortBy: '',
  };
  get filteringSearchSubmitted() {
    return this._state.filteringSearchSubmitted;
  }
  get page() {
    return this._state.page;
  }
  get pageSize() {
    return this._state.pageSize;
  }
  get searchTerm() {
    return this._state.searchTerm;
  }
  get sortAsc() {
    return this._state.sortAsc;
  }
  get sortBy() {
    return this._state.sortBy;
  }
  set filteringSearchSubmitted(filteringSearchSubmitted: boolean) {
    this._set({ filteringSearchSubmitted });
  }
  set page(page: number) {
    this._set({ page });
  }
  set pageSize(pageSize: number) {
    this._set({ pageSize });
  }
  set searchTerm(searchTerm: string) {
    this._set({ searchTerm });
  }
  set sortBy(sortBy: string) {
    this._set({ sortBy });
  }
  set sortAsc(sortAsc: boolean) {
    this._set({ sortAsc });
  }
  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
  }

  // methods
  ngOnInit(): void {
    this.patientApiService
      .getPatients(
        this.page,
        this.pageSize,
        this.searchTerm,
        this.sortBy,
        this.sortAsc
      )
      .then((response: any) => {
        this.patients = response.data.patients;
        this.patientCollectionSize = response.data.collectionTotal;
      });
  }
	onClearSearchTerm() {
    this.filteringSearchSubmitted = false;
		this.searchTerm = '';
		this.onSearch();
	}
  onPageNavigate(event: number) {
    this.page = event;
    this.patientApiService
      .getPatients(
        this.page,
        this.pageSize,
        this.searchTerm,
        this.sortBy,
        this.sortAsc
      )
      .then((response: any) => {
        this.patients = response.data.patients;
        this.patientCollectionSize = response.data.collectionTotal;
      });
  }
  onSearch() {
    this.patientApiService
      .getPatients(
        this.page,
        this.pageSize,
        this.searchTerm,
        this.sortBy,
        this.sortAsc
      )
      .then((response: any) => {
        this.patients = response.data.patients;
        this.patientCollectionSize = response.data.collectionTotal;
        this.page = 1;
				this.zeroSearchResults = this.patients.length === 0 && this.searchTerm.length > 0 ? true : false;
        this.filteringSearchSubmitted = true;
      });
  }
  onSort({ column, direction }: SortEvent) {
    // resetting other headers
    this.headers.forEach((header) => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });

    this.sortBy = column;
    this.sortAsc = direction.toLowerCase() === 'asc' ? true : false;
    this.patientApiService
      .getPatients(
        this.page,
        this.pageSize,
        this.searchTerm,
        this.sortBy,
        this.sortAsc
      )
      .then((response: any) => {
        this.patients = response.data.patients;
        this.patientCollectionSize = response.data.collectionTotal;
      });
  }
	onUpdatePatient(patient: Patient) {
		console.log("onUpdatePatient() called, patient: ", patient);
			this.patientApiService.upsertPatient(patient).then((response: any) => {
				// refresh the table's Patient collection in case it affects the current sort/pagination/search
				this.patientApiService
					.getPatients(
						this.page,
						this.pageSize,
						this.searchTerm,
						this.sortBy,
						this.sortAsc
					)
					.then((response: any) => {
						this.patients = response.data.patients;
						this.patientCollectionSize = response.data.collectionTotal;
					});
		}); 
	}
 } 
