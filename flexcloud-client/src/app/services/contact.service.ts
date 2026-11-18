import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Contact } from '../models/models';

@Injectable({ providedIn: 'root' })
export class ContactService {
  private baseUrl = `${environment.apiUrl}/contacts`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Contact[]> {
    return this.http.get<Contact[]>(this.baseUrl);
  }

  getById(id: number): Observable<Contact> {
    return this.http.get<Contact>(`${this.baseUrl}/${id}`);
  }

  create(contact: Partial<Contact>): Observable<Contact> {
    return this.http.post<Contact>(this.baseUrl, contact);
  }

  update(id: number, contact: Partial<Contact>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, contact);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
