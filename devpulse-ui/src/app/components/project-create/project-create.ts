import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Project } from '../../services/project';

@Component({
  selector: 'app-project-create',
  imports: [FormsModule],
  templateUrl: './project-create.html',
  styleUrl: './project-create.scss',
})

export class ProjectCreate {
  projectName = signal('');
  projectCode = signal('');
  description = signal('');

  showSuccess = signal(false);

  constructor(private project: Project){}

  onSubmit(): void{
    const newProject = {
      name: this.projectName(),
      code: this.projectCode(),
      description: this.description()
    };

    this.project.createProject(newProject).subscribe({
      next: (response) => {
        console.log('Database Success! ', response)
        this.showSuccess.set(true);

        setTimeout(()=>{
          this.projectName.set('');
          this.projectCode.set('');
          this.description.set('');  
        }, 0)
        
        setTimeout(() => {
          this.showSuccess.set(false);
        }, 5000);
      },
      error: (err) => {
        console.error('Network Transmission Failed:', err);
        alert('Something went wrong communicating with the server.');
      }
    });
  }
}
