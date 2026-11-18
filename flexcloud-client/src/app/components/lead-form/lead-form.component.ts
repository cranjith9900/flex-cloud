import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Contact, LeadStatus } from '../../models/models';
import { ContactService } from '../../services/contact.service';
import { LeadService } from '../../services/lead.service';

@Component({
  selector: 'app-lead-form',
  templateUrl: './lead-form.component.html'
})
export class LeadFormComponent implements OnInit {
  id: number | null = null;
  contacts: Contact[] = [];
  error = '';
  saving = false;

  model = {
    name: '',
    source: '',
    status: 'New' as LeadStatus,
    estimatedValue: 0,
    contactId: 0
  };

  constructor(
    private leadService: LeadService,
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
      this.leadService.getById(this.id).subscribe((lead) => {
        this.model = {
          name: lead.name,
          source: lead.source,
          status: lead.status,
          estimatedValue: lead.estimatedValue,
          contactId: lead.contactId
        };
      });
    }
  }

  save(): void {
    this.saving = true;
    this.error = '';
    const request: Observable<unknown> = this.id
      ? this.leadService.update(this.id, this.model)
      : this.leadService.create(this.model);

    request.subscribe({
      next: () => this.router.navigate(['/leads']),
      error: (err: any) => {
        this.error = err?.error ?? 'Failed to save lead.';
        this.saving = false;
      }
    });
  }
}
