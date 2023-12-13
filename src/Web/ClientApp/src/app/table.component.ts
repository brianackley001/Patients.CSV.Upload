import { AsyncPipe, DecimalPipe } from '@angular/common';
import { Component, QueryList, ViewChildren } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgbHighlight, NgbPaginationModule } from '@ng-bootstrap/ng-bootstrap';

import { Patient } from './dataModels/patient';
import { PatientApiService } from './services/patient-api.service';
import { NgbdSortableHeader, SortEvent } from './directives/sortable.directive';
import { MmDdYYYYDatePipe } from './mm--dd-yyyy-date.pipe';

interface State {
  page: number;
  pageSize: number;
  searchTerm: string;
  sortBy: string;
  sortAsc: boolean;
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

  constructor(private patientApiService: PatientApiService) {}

  // Internal State tracking
  private _state: State = {
    page: 1,
    pageSize: 5,
    searchTerm: '',
    sortBy: '',
    sortAsc: true,
  };
  get page() {
    return this._state.page;
  }
  get pageSize() {
    return this._state.pageSize;
  }
  get searchTerm() {
    return this._state.searchTerm;
  }
  get sortBy() {
    return this._state.sortBy;
  }
  get sortAsc() {
    return this._state.sortAsc;
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
      });
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
}
