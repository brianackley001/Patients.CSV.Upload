import { Injectable } from '@angular/core';
import { environment } from './../../environments/environment';
import axios from 'axios';
import { Patient } from '../dataModels/patient';

@Injectable({
  providedIn: 'root'
})
export class PatientApiService {

  constructor() { }

  getPatients(pageNumber: number, pageSize: number, searchTerm: string, sortBy : string, sortAsc : boolean) {
    // nullable parameters:
    let optionalParameters = searchTerm.length > 0 ? `&searchTerm=${searchTerm}` : '';
    optionalParameters += sortBy.length > 0  ? `&sortBy=${sortBy}` : '';
    optionalParameters += !sortAsc ? `&sortAsc=${sortAsc}` : '';
    
    return axios.get(`${environment.apiUrl}/Patient?pageNumber=${pageNumber}&pageSize=${pageSize}${optionalParameters}`); //
  }
  upsertPatient(patient: Patient) {
    return axios.post(`${environment.apiUrl}/Patient`, patient);
  }
}
