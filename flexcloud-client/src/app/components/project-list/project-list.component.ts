import { Component, OnInit } from '@angular/core';
import { Project } from '../../models/models';
import { ProjectService } from '../../services/project.service';

@Component({
  selector: 'app-project-list',
  templateUrl: './project-list.component.html'
})
export class ProjectListComponent implements OnInit {
  projects: Project[] = [];
  loading = false;
  error = '';

  constructor(private projectService: ProjectService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = '';
    this.projectService.getAll().subscribe({
      next: (projects) => {
        this.projects = projects;
        this.loading = false;
      },
      error: () => {
        this.error = 'Unable to load projects. Is the API running?';
        this.loading = false;
      }
    });
  }

  deleteProject(id: number): void {
    if (!confirm('Delete this project?')) return;
    this.projectService.delete(id).subscribe(() => this.load());
  }
}
