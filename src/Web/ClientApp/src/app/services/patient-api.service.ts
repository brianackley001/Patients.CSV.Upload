import { Injectable } from '@angular/core';
import axios from 'axios';

@Injectable({
  providedIn: 'root'
})
export class PatientApiService {

  constructor() { }

  getPatients() {
    return axios.get('https://localhost:7171/Patient?pageNumber=1&pageSize=10'); //
  }
}
