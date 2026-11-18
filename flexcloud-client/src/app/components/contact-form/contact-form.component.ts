import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ContactService } from '../../services/contact.service';

@Component({
  selector: 'app-contact-form',
  templateUrl: './contact-form.component.html'
})
export class ContactFormComponent implements OnInit {
  id: number | null = null;
  error = '';
  saving = false;

  model = {
    fullName: '',
    email: '',
    phone: '',
    company: ''
  };

  constructor(
    private contactService: ContactService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.id = Number(idParam);
      this.contactService.getById(this.id).subscribe((contact) => {
        this.model = {
          fullName: contact.fullName,
          email: contact.email,
          phone: contact.phone,
          company: contact.company
        };
      });
    }
  }

  save(): void {
    this.saving = true;
    this.error = '';
    const request: Observable<unknown> = this.id
      ? this.contactService.update(this.id, this.model)
      : this.contactService.create(this.model);

    request.subscribe({
      next: () => this.router.navigate(['/contacts']),
      error: (err: any) => {
        this.error = err?.error ?? 'Failed to save contact.';
        this.saving = false;
      }
    });
  }
}
