import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Project {
  private apiUrl = 'http://localhost:5165/api/projects';

  constructor(private http: HttpClient){}

  createProject(projectData: {name: string; code: string; description: string}): Observable<any> {
    return this.http.post(this.apiUrl, projectData);
  }

  getProjects(): Observable<any[]>{
    return this.http.get<any[]>(this.apiUrl);
  }
}
