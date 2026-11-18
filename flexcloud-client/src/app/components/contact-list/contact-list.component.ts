import { Component, OnInit } from '@angular/core';
import { Contact } from '../../models/models';
import { ContactService } from '../../services/contact.service';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html'
})
export class ContactListComponent implements OnInit {
  contacts: Contact[] = [];
  loading = false;
  error = '';

  constructor(private contactService: ContactService) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.error = '';
    this.contactService.getAll().subscribe({
      next: (contacts) => {
        this.contacts = contacts;
        this.loading = false;
      },
      error: () => {
        this.error = 'Unable to load contacts. Is the API running?';
        this.loading = false;
      }
    });
  }

  deleteContact(id: number): void {
    if (!confirm('Delete this contact?')) return;
    this.contactService.delete(id).subscribe(() => this.load());
  }
}
