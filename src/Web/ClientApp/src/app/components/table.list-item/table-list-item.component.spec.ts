import { ComponentFixture, TestBed } from '@angular/core/testing'; 
import { NgbHighlight, NgbModal  } from '@ng-bootstrap/ng-bootstrap';
import { FormsModule } from '@angular/forms'; 
import { MmDdYYYYDatePipe } from '../../pipes/mm--dd-yyyy-date.pipe';

import { TableListItem } from "./table-list-item.component";
import { Patient } from '../../dataModels/patient';

describe('TableListItem', () => {
  let component: TableListItem;
  let fixture: ComponentFixture<TableListItem>;

  // Stubs:
  let patientStubInput:Patient = {
    id: 1,
    firstName: 'John',
    lastName: 'Doe',
    birthDate: new Date('2021-01-01'),
    genderDescription: 'Male',
    dateCreated: new Date('2021-01-01'),
    dateUpdated: new Date('2021-01-01'),
  };

  let datePipeStub: Partial<MmDdYYYYDatePipe> = {
    transform: () => new Date('2021-01-01'),};


  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableListItem, NgbHighlight, FormsModule],
      providers: [{provide : MmDdYYYYDatePipe, useValue: datePipeStub}],
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TableListItem);
    component = fixture.componentInstance;
    component.patient = patientStubInput;
    fixture.detectChanges();
    component.ngOnInit();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
