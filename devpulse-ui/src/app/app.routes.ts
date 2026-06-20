import { Routes } from '@angular/router';
import { ProjectCreate } from './features/projects/project-create/project-create';
import { ProjectDashboard } from './features/projects/project-dashboard/project-dashboard';
import { ProjectBoard } from './features/projects/project-board/project-board';

export const routes: Routes = [
    {path: '', component: ProjectDashboard},
    {path: 'projects/create', component: ProjectCreate},
    {path: 'projects/:id/board', component: ProjectBoard}
];
