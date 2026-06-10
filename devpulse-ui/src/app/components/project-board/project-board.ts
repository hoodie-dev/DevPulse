import { Component, OnInit, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-project-board',
  imports: [],
  templateUrl: './project-board.html',
  styleUrl: './project-board.scss',
})
export class ProjectBoard implements OnInit{
  projectId = signal<string | null>(null);

  constructor(private route: ActivatedRoute){}

  ngOnInit(): void {
    const idFromUrl = this.route.snapshot.paramMap.get('id');
    this.projectId.set(idFromUrl);

    console.log('Project workspace loaded for ID: ', this.projectId());
    
  }
}
