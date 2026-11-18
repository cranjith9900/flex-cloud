import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Contact, ProjectStatus } from '../../models/models';
import { ContactService } from '../../services/contact.service';
import { ProjectService } from '../../services/project.service';

@Component({
  selector: 'app-project-form',
  templateUrl: './project-form.component.html'
})
export class ProjectFormComponent implements OnInit {
  id: number | null = null;
  contacts: Contact[] = [];
  error = '';
  saving = false;

  model = {
    name: '',
    clientName: '',
    status: 'Planning' as ProjectStatus,
    budget: 0,
    startDate: new Date().toISOString().slice(0, 10),
    endDate: '' as string | null,
    contactId: 0
  };

  constructor(
    private projectService: ProjectService,
    private contactService: ContactService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.contactService.getAll().subscribe((contacts) => {
      this.contacts = contacts;
      if (!this.model.contactId && contacts.length) {
        this.model.contactId = contacts[0].id;
      }
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = Number(idParam);
      this.projectService.getById(this.id).subscribe((project) => {
        this.model = {
          name: project.name,
          clientName: project.clientName,
          status: project.status,
          budget: project.budget,
          startDate: project.startDate?.slice(0, 10),
          endDate: project.endDate ? project.endDate.slice(0, 10) : '',
          contactId: project.contactId
        };
      });
    }
  }

  save(): void {
    this.saving = true;
    this.error = '';

    const payload = {
      ...this.model,
      endDate: this.model.endDate || undefined
    };

    const request: Observable<unknown> = this.id
      ? this.projectService.update(this.id, payload)
      : this.projectService.create(payload);

    request.subscribe({
      next: () => this.router.navigate(['/projects']),
      error: (err: any) => {
        this.error = err?.error ?? 'Failed to save project.';
        this.saving = false;
      }
    });
  }
}
