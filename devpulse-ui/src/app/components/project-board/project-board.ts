import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Issue, IssuePriority } from '../../services/issue';

@Component({
  selector: 'app-project-board',
  imports: [CommonModule, FormsModule],
  templateUrl: './project-board.html',
  styleUrl: './project-board.scss',
})
export class ProjectBoard implements OnInit{
  projectId = signal<string | null>(null);

  issueTitle = '';
  issueDescription = '';
  selectedPriority = IssuePriority.Medium;

  PriorityEnum = IssuePriority;

  constructor(
    private route: ActivatedRoute,
    private issueService: Issue
  ){}

  ngOnInit(): void {
    const idFromUrl = this.route.snapshot.paramMap.get('id');
    this.projectId.set(idFromUrl);
  }

  onCreateIssue(): void{
    if(!this.issueTitle.trim() || !this.projectId()) return;

    const newIssuePayload = {
      title: this.issueTitle,
      description: this.issueDescription,
      priority: Number(this.selectedPriority),
      projectId: this.projectId()!
    };

    this.issueService.createIssue(newIssuePayload).subscribe({
      next: (newGuid) => {
        console.log('Issue created successfully with Guid: ', newGuid);

        this.issueTitle = '';
        this.issueDescription = '';
        this.selectedPriority = IssuePriority.Medium;
        alert('Issue successfully logged to project backlog!');
      },
      error: (err) => {
        console.error('Failed to create issue: ', err);
      }
    });
  }
}
