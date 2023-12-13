import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NgbdTableComplete } from './table.component';

describe('TableComponent', () => {
  let component: NgbdTableComplete;
  let fixture: ComponentFixture<NgbdTableComplete>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NgbdTableComplete]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(NgbdTableComplete);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
