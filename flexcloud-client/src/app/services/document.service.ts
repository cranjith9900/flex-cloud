import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ProjectDocument } from '../models/models';

@Injectable({ providedIn: 'root' })
export class DocumentService {
  private baseUrl = `${environment.apiUrl}/documents`;

  constructor(private http: HttpClient) {}

  getAll(projectId?: number): Observable<ProjectDocument[]> {
    const params: Record<string, string> = {};
    if (projectId) params['projectId'] = String(projectId);
    return this.http.get<ProjectDocument[]>(this.baseUrl, { params });
  }

  create(document: Partial<ProjectDocument>): Observable<ProjectDocument> {
    return this.http.post<ProjectDocument>(this.baseUrl, document);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
