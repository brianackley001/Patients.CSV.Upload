import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { Patient } from '../dataModels/patient';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'patient-exercise';
  patients : Patient[] = [
    new Patient(1,'Robert', 'Smith', 'male', new Date('01/01/1970'), new Date('01/01/2019'), new Date('01/01/2019')),
    new Patient(1,'Steve', 'Kilbey', 'male', new Date('01/01/1970'), new Date('01/01/2019'), new Date('01/01/2019')),
    new Patient(1,'Michael', 'Stipe', 'male', new Date('01/01/1970'), new Date('01/01/2019'), new Date('01/01/2019')),
    new Patient(1,'Daniel', 'Ash', 'male', new Date('01/01/1970'), new Date('01/01/2019'), new Date('01/01/2019')),
    new Patient(1,'Trent', 'Reznor', 'male', new Date('01/01/1970'), new Date('01/01/2019'), new Date('01/01/2019')),
    new Patient(1,'Nick', 'Cave', 'male', new Date('01/01/1970'), new Date('01/01/2019'), new Date('01/01/2019')),
    new Patient(1,'Debbie', 'Harry', 'female', new Date('01/01/1970'), new Date('01/01/2019'), new Date('01/01/2019')),
    new Patient(1,'Joan', 'Jett', 'female', new Date('01/01/1970'), new Date('01/01/2019'), new Date('01/01/2019')),
    new Patient(1,'Siouxsie', 'Sioux', 'female', new Date('01/01/1970'), new Date('01/01/2019'), new Date('01/01/2019')),
    new Patient(1,'Taylor', 'Swift', 'female', new Date('01/01/1970'), new Date('01/01/2019'), new Date('01/01/2019')),
  ];
}
