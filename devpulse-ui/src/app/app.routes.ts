import { Routes } from '@angular/router';
import { ProjectCreate } from './features/projects/project-create/project-create';
import { ProjectDashboard } from './features/projects/project-dashboard/project-dashboard';
import { ProjectBoard } from './features/projects/project-board/project-board';
import { LoginComponent } from './features/auth/login/login.component';
import { RegisterComponent } from './features/auth/register/register.component';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    {path: '', redirectTo: 'login', pathMatch: 'full'},
    {path: 'projects/create', component: ProjectCreate},
    {path: 'projects/:id/board', component: ProjectBoard},
    {path: 'dashboard', component: ProjectDashboard}
];
