import { Component, OnInit } from '@angular/core';
import { Lead } from '../../models/models';
import { LeadService } from '../../services/lead.service';

@Component({
  selector: 'app-lead-list',
  templateUrl: './lead-list.component.html'
})
export class LeadListComponent implements OnInit {
  leads: Lead[] = [];
  loading = false;
  error = '';

  constructor(private leadService: LeadService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = '';
    this.leadService.getAll().subscribe({
      next: (leads) => {
        this.leads = leads;
        this.loading = false;
      },
      error: () => {
        this.error = 'Unable to load leads. Is the API running?';
        this.loading = false;
      }
    });
  }

  deleteLead(id: number): void {
    if (!confirm('Delete this lead?')) return;
    this.leadService.delete(id).subscribe(() => this.load());
  }
}
