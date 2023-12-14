import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TableListItem } from "./table-list-item.component";

describe('TableListItem', () => {
  let component: TableListItem;
  let fixture: ComponentFixture<TableListItem>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TableListItem]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(TableListItem);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
