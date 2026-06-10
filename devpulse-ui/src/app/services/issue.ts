import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export enum IssuePriority{
  Low = 0,
  Medium = 1,
  High = 2
}

export interface CreateIssueRequest{
  title: string;
  description: string;
  priority: IssuePriority;
  projectId: string;
}

@Injectable({
  providedIn: 'root',
})
export class Issue {
  private apiUrl = 'http://localhost:5165/api/issues';

  constructor(private http: HttpClient){}

  createIssue(issueData: CreateIssueRequest): Observable<string>{
    return this.http.post<string>(this.apiUrl, issueData);
  }

  getIssuesByProject(projectId: string): Observable<any[]>{
    return this.http.get<any[]>(`${this.apiUrl}/project/${projectId}`);
  }
}
