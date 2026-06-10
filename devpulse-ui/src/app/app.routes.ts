import { Routes } from '@angular/router';
import { ProjectCreate } from './components/project-create/project-create';
import { ProjectDashboard } from './components/project-dashboard/project-dashboard';
import { ProjectBoard } from './components/project-board/project-board';

export const routes: Routes = [
    {path: '', component: ProjectDashboard},
    {path: 'projects/create', component: ProjectCreate},
    {path: 'projects/:id/board', component: ProjectBoard}
];
