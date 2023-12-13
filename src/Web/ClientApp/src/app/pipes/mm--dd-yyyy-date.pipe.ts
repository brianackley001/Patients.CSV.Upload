import { Pipe, PipeTransform } from '@angular/core';
import { DatePipe } from '@angular/common';
@Pipe({
  name: 'mmDdYYYYDate',
	standalone: true,
})
export class MmDdYYYYDatePipe extends DatePipe implements PipeTransform {

  override transform(value: any, args?: any): any {
    return super.transform(value, 'M/d/YYYY');
  }

}