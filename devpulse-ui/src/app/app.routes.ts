import { Routes } from '@angular/router';
import { ProjectCreate } from './components/project-create/project-create';
import { ProjectDashboard } from './components/project-dashboard/project-dashboard';

export const routes: Routes = [
    {path: '', component: ProjectDashboard},
    {path: 'projects/create', component: ProjectCreate}
];
