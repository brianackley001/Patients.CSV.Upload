import { Component, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PatientApiService } from '../../services/patient-api.service';
import { PatientCsvItem } from '../../dataModels/patientCsvItem';
import { NgbAlertModule } from '@ng-bootstrap/ng-bootstrap';

interface State {
  fileProcessingComplete: boolean;
  successMessage: string;
  uploadErrors: { isvalid: boolean; errors: string[] };
}
interface uploadError {
  isvalid: boolean;
  errors: string[];
}

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [NgbAlertModule, CommonModule],
  providers: [PatientApiService],
  templateUrl: './upload.component.html',
  styleUrl: './upload.component.css',
})
export class UploadComponent {
  constructor(private patientApiService: PatientApiService) {}

  csvFileName = '';
  records: PatientCsvItem[] = [];
  @ViewChild('fileUpload') csvReader: any;

  // Internal State tracking
  private _state: State = {
    fileProcessingComplete: false,
    successMessage: '',
    uploadErrors: {
      isvalid: true,
      errors: [],
    },
  };
  get fileProcessingComplete() {
    return this._state.fileProcessingComplete;
  }
  get successMessage() {
    return this._state.successMessage;
  }
  get uploadErrors() {
    return this._state.uploadErrors;
  }
  set fileProcessingComplete(fileProcessingComplete: boolean) {
    this._set({ fileProcessingComplete });
  }
  set successMessage(successMessage: string) {
    this._set({ successMessage });
  }
  set uploadErrors(uploadErrors: uploadError) {
    this._set({ uploadErrors });
  }
  private _set(patch: Partial<State>) {
    Object.assign(this._state, patch);
  }

  onFileSelected($event: any): void {
    this.fileProcessingComplete = false;

    let files = $event.srcElement.files;

    if (this.isValidCSVFile(files[0])) {
      let input = $event.target;
      const fileMb = input.files[0].size / 1024 ** 2;

      // Set Arbitrary file size maximum for validation...
      this.csvFileName = input.files[0].name;
      if (fileMb > 5) {
        console.log('File size exceeds 5 MB');
        this.uploadErrors = {
          isvalid: false,
          errors: [`File "${this.csvFileName}" exceeds 5 MB file size limit`],
        };
        this.fileProcessingComplete = true;

        console.log(`this.uploadErrors.isValid: ${this.uploadErrors.isvalid}`);
        console.log(
          `this.fileProcessingComplete: ${this.fileProcessingComplete}`
        );
        console.table(this.uploadErrors.errors);

        this.fileReset();
      } else {
        let reader = new FileReader();
        try {
          reader.readAsText(input.files[0]);

          reader.onload = () => {
            let csvData = reader.result;
            let csvRecordsArray = (<string>csvData).split(/\r\n|\n/);

            let headersRow = this.getHeaderArray(csvRecordsArray);

            this.records = this.getDataRecordsArrayFromCSVFile(
              csvRecordsArray,
              headersRow.length
            );

            // Validate that headers are present and match expectations:
            console.table(this.records);

            let validatedHeaders = this.isHeaderRowValid(headersRow);
            if (!validatedHeaders.isValid) {
              this.uploadErrors = {
                isvalid: validatedHeaders.isValid,
                errors: validatedHeaders.errors,
              };
              this.fileProcessingComplete = true;
              console.log(
                `this.uploadErrors.isValid: ${this.uploadErrors.isvalid}`
              );
              console.log(
                `this.fileProcessingComplete: ${this.fileProcessingComplete}`
              );
              console.table(this.uploadErrors.errors);
            } else {
              this.patientApiService
                .upsertPatientCsvImport(this.records)
                .then((response: any) => {
                  if (response.data == true && response.status == 200) {
                    this.fileProcessingComplete = true;
                    this.successMessage = `Successfully uploaded ${this.records.length} records from file "${this.csvFileName}"`;
                  } else {
                    this.uploadErrors = {
                      isvalid: false,
                      errors: ['Error uploading file'],
                    };
                  }
                  this.fileProcessingComplete = true;
                  this.fileReset();
                });
            }
          };

          reader.onerror = function () {
            console.log('an unhandled error has occured while reading the file!');
          };
        } catch (error) {
          console.log(error);
        }
      }
    } else {
      alert('Please import a valid .csv file.');
      this.fileReset();
    }
  }

  fileReset() {
    this.csvReader.nativeElement.value = '';
    this.records = [];
  }

  getDataRecordsArrayFromCSVFile(csvRecordsArray: any, headerLength: any) {
    let csvArr = [];

    for (let i = 1; i < csvRecordsArray.length; i++) {
      let currentRecord = (<string>csvRecordsArray[i]).split(',');
      if (currentRecord.length == headerLength) {
        let csvRecord: PatientCsvItem = {
          firstName: currentRecord[0].trim(),
          lastName: currentRecord[1].trim(),
          birthDate: new Date(currentRecord[2].trim()),
          genderDescription: currentRecord[3].trim(),
        };
        csvArr.push(csvRecord);
      }
    }
    return csvArr;
  }

  getHeaderArray(csvRecordsArr: any) {
    let headers = (<string>csvRecordsArr[0]).split(',');
    let headerArray = [];
    for (let j = 0; j < headers.length; j++) {
      headerArray.push(headers[j]);
    }
    return headerArray;
  }

  isHeaderRowValid(headersRow: any) {
    let isValid = true;
    let errors: string[] = [];

    // Validate that headers are present and match expectations:
    if (headersRow.length != 4) {
      isValid &&= false;
      errors.push(
        `Incorrect number of header columns. Expected 4, found ${headersRow.length}`
      );
      return { isValid, errors };
    }

    if (
      headersRow[0].length < 1 ||
      (!headersRow[0].toLocaleLowerCase().includes('first') &&
        !headersRow[0].toLocaleLowerCase().includes('name'))
    ) {
      isValid &&= false;
      errors.push('FirstName header is absent or incorrect');
    }
    if (
      headersRow[1].length < 1 ||
      (!headersRow[1].toLocaleLowerCase().includes('last') &&
        !headersRow[1].toLocaleLowerCase().includes('name'))
    ) {
      isValid &&= false;
      errors.push('LastName header is absent or incorrect');
    }
    if (
      headersRow[2].length < 1 ||
      !headersRow[2].toLocaleLowerCase().includes('birthd')
    ) {
      isValid &&= false;
      errors.push('BirthDay header is absent or incorrect');
    }
    if (
      headersRow[3].length < 1 ||
      !headersRow[3].toLocaleLowerCase().includes('gender')
    ) {
      isValid &&= false;
      errors.push('Gender header is absent or incorrect');
    }
    this.uploadErrors = {
      isvalid: isValid,
      errors: errors,
    };
    return { isValid, errors };
  }

  isValidCSVFile(file: any) {
    return file.name.endsWith('.csv');
  }
}
