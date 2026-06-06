import { Routes } from '@angular/router';
import { ProjectCreate } from './components/project-create/project-create';
export const routes: Routes = [
    {path: '', redirectTo: 'projects/create', pathMatch: 'full'},
    {path: 'projects/create', component: ProjectCreate}
];
