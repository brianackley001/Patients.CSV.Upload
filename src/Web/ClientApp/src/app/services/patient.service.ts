/* eslint-disable @typescript-eslint/adjacent-overload-signatures */
import { Injectable, PipeTransform } from '@angular/core';

import { BehaviorSubject, Observable, of, Subject } from 'rxjs';

import { Patient } from '../dataModels/patient';
import { PATIENTS } from '../dataModels/patients';
import { DecimalPipe } from '@angular/common';
import { debounceTime, delay, switchMap, tap } from 'rxjs/operators';
import { SortColumn, SortDirection } from '../directives/sortable.directive';

interface SearchResult {
	patients: Patient[];
	total: number;
}

interface State {
	page: number;
	pageSize: number;
	searchTerm: string;
	sortColumn: SortColumn;
	sortDirection: SortDirection;
}

const compare = (v1: string | number | Date, v2: string | number | Date) => (v1 < v2 ? -1 : v1 > v2 ? 1 : 0);

function sort(patients: Patient[], column: SortColumn, direction: string): Patient[] {
	if (direction === '' || column === '') {
		return patients;
	} else {
		return [...patients].sort((a, b) => {
			const res = compare(a[column], b[column]);
			return direction === 'asc' ? res : -res;
		});
	}
}

function matches(patient: Patient, term: string, pipe: PipeTransform) {
	return (
		patient.firstName.toLowerCase().includes(term.toLowerCase()) ||
		pipe.transform(patient.lastName.toLowerCase()).includes(term.toLowerCase()) 
	);
}

@Injectable({ providedIn: 'root' })
export class PatientService {
	private _loading$ = new BehaviorSubject<boolean>(true);
	private _search$ = new Subject<void>();
	private _patients$ = new BehaviorSubject<Patient[]>([]);
	private _total$ = new BehaviorSubject<number>(0);

	private _state: State = {
		page: 1,
		pageSize: 4,
		searchTerm: '',
		sortColumn: '',
		sortDirection: '',
	};

	constructor(private pipe: DecimalPipe) {
		this._search$
			.pipe(
				tap(() => this._loading$.next(true)),
				debounceTime(200),
				switchMap(() => this._search()),
				delay(200),
				tap(() => this._loading$.next(false)),
			)
			.subscribe((result) => {
				this._patients$.next(result.patients);
				this._total$.next(result.total);
			});

		this._search$.next();
	}

	get patients$() {
		return this._patients$.asObservable();
	}
	get total$() {
		return this._total$.asObservable();
	}
	get loading$() {
		return this._loading$.asObservable();
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

	set page(page: number) {
		this._set({ page });
	}
	set pageSize(pageSize: number) {
		this._set({ pageSize });
	}
	set searchTerm(searchTerm: string) {
		this._set({ searchTerm });
	}
	set sortColumn(sortColumn: SortColumn) {
		this._set({ sortColumn });
	}
	set sortDirection(sortDirection: SortDirection) {
		this._set({ sortDirection });
	}

	private _set(patch: Partial<State>) {
		Object.assign(this._state, patch);
		this._search$.next();
	}

	private _search(): Observable<SearchResult> {
		const { sortColumn, sortDirection, pageSize, page, searchTerm } = this._state;

		// 1. sort
		let patients = sort(PATIENTS, sortColumn, sortDirection);

		// 2. filter
		patients = patients.filter((patient) => matches(patient, searchTerm, this.pipe));
		const total = patients.length;

		// 3. paginate
		patients = patients.slice((page - 1) * pageSize, (page - 1) * pageSize + pageSize);
		return of({ patients, total });
	}
}