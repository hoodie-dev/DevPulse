import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Project } from '../../../services/project';

@Component({
  selector: 'app-project-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './project-dashboard.html',
  styleUrl: './project-dashboard.scss',
})
export class ProjectDashboard implements OnInit {
  projects = signal<any[]>([]);
  isLoading = signal(true);

  constructor(private projectService: Project) {}

  ngOnInit(): void {
    this.loadProjects();
  }

  loadProjects(): void {
    this.projectService.getProjects().subscribe({
      next: (data) => {
        this.projects.set(data);
        this.isLoading.set(false);
      },
      error: (err) => {
        console.error('Failed to retrieve dashboard metrics: ', err);
        this.isLoading.set(false);
      },
    });
  }

  onDeleteProject(id: string): void{
    if(confirm('Are you sure you want to permanently delete this project?')){
      this.projectService.deleteProject(id).subscribe({
        next: ()=>{
          console.log('Project dropped from server successfully.');
          this.projects.update(allProjects => allProjects.filter(p =>p.id !== id));
        },
        error: (err) => {
          console.error('Deletion failed: ', err);
          alert('Could not delete this project. Please check backend API logs.')
        }
      });
    }
  }
}
