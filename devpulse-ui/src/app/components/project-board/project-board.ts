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

  todoIssues = signal<any[]>([]);
  inProgressIssues = signal<any[]>([]);
  inReviewIssues = signal<any[]>([]);
  doneIssues = signal<any[]>([]);

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

    if(this.projectId()){
      this.loadWorkspaceIssues();
    }
  }

  loadWorkspaceIssues(): void {
    this.issueService.getIssuesByProject(this.projectId()!).subscribe({
      next: (data) => {
        this.todoIssues.set(data.filter(i => i.status ===1 || i.status === "Todo"));
        this.inProgressIssues.set(data.filter(i => i.status === 2 || i.status === "InProgress"));
        this.inReviewIssues.set(data.filter(i => i.status === 3 || i.status === "InReview"));
        this.doneIssues.set(data.filter(i => i.status === 4 || i.status === "Done"));
      },
      error: (err) => {
        console.error('Failed to fill Kanban column feeds: ', err);
        
      }
    });
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
        this.loadWorkspaceIssues();
      },
      error: (err) => {
        console.error('Failed to create issue: ', err);
      }
    });
  }

  getPriorityLabel(priority: number | string): {text:string; css: string}{
    if (priority === 2 || priority === 'High') return { text: 'High', css: 'bg-red-50 text-red-700 border-red-100' };
    if (priority === 1 || priority === 'Medium') return { text: 'Medium', css: 'bg-amber-50 text-amber-700 border-amber-100' };
    return { text: 'Low', css: 'bg-slate-50 text-slate-600 border-slate-100' };
  }
}
