import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DocumentType, Project } from '../../models/models';
import { DocumentService } from '../../services/document.service';
import { ProjectService } from '../../services/project.service';

@Component({
  selector: 'app-document-form',
  templateUrl: './document-form.component.html'
})
export class DocumentFormComponent implements OnInit {
  projects: Project[] = [];
  error = '';
  saving = false;

  model = {
    fileName: '',
    type: 'Other' as DocumentType,
    notes: '',
    projectId: 0
  };

  constructor(
    private documentService: DocumentService,
    private projectService: ProjectService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.projectService.getAll().subscribe((projects) => {
      this.projects = projects;
      if (projects.length) this.model.projectId = projects[0].id;
    });
  }

  save(): void {
    this.saving = true;
    this.error = '';
    this.documentService.create(this.model).subscribe({
      next: () => this.router.navigate(['/documents']),
      error: (err: any) => {
        this.error = err?.error ?? 'Failed to save document.';
        this.saving = false;
      }
    });
  }
}
