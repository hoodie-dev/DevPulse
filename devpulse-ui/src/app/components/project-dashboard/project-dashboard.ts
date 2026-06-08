import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Project } from '../../services/project';

@Component({
  selector: 'app-project-dashboard',
  imports: [CommonModule],
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
}
