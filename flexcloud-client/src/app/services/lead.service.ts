import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Lead } from '../models/models';

@Injectable({ providedIn: 'root' })
export class LeadService {
  private baseUrl = `${environment.apiUrl}/leads`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Lead[]> {
    return this.http.get<Lead[]>(this.baseUrl);
  }

  getById(id: number): Observable<Lead> {
    return this.http.get<Lead>(`${this.baseUrl}/${id}`);
  }

  create(lead: Partial<Lead>): Observable<Lead> {
    return this.http.post<Lead>(this.baseUrl, lead);
  }

  update(id: number, lead: Partial<Lead>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, lead);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
