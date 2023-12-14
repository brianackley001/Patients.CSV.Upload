import { Routes } from '@angular/router';
import { NgbdTableComplete } from './components/table/table.component';
import { UploadComponent } from './components/upload/upload.component';
import { PageNotFoundComponent } from './components/page-not-found/page-not-found.component';

export const routes: Routes = [
  { path: '', redirectTo: 'patients', pathMatch: 'full'},
  { path: 'patients', component: NgbdTableComplete },
  { path: 'upload', component: UploadComponent},
  { path: '404', component: PageNotFoundComponent},
  { path: '**', redirectTo: '404' }
];
