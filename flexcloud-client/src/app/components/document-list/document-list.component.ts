import { Component, OnInit } from '@angular/core';
import { ProjectDocument } from '../../models/models';
import { DocumentService } from '../../services/document.service';

@Component({
  selector: 'app-document-list',
  templateUrl: './document-list.component.html'
})
export class DocumentListComponent implements OnInit {
  documents: ProjectDocument[] = [];
  loading = false;
  error = '';

  constructor(private documentService: DocumentService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = '';
    this.documentService.getAll().subscribe({
      next: (documents) => {
        this.documents = documents;
        this.loading = false;
      },
      error: () => {
        this.error = 'Unable to load documents. Is the API running?';
        this.loading = false;
      }
    });
  }

  deleteDocument(id: number): void {
    if (!confirm('Delete this document?')) return;
    this.documentService.delete(id).subscribe(() => this.load());
  }
}
