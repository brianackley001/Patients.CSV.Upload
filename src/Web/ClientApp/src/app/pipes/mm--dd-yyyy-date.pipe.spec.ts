import { MmDdYYYYDatePipe } from './mm--dd-yyyy-date.pipe';

describe('MmDdYYYYDatePipe', () => {
  it('create an instance', () => {
    const pipe = new MmDdYYYYDatePipe('en-US'); // Pass the locale as an argument
    expect(pipe).toBeTruthy();
  });
});
