import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Project } from '../models/models';

@Injectable({ providedIn: 'root' })
export class ProjectService {
  private baseUrl = `${environment.apiUrl}/projects`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Project[]> {
    return this.http.get<Project[]>(this.baseUrl);
  }

  getById(id: number): Observable<Project> {
    return this.http.get<Project>(`${this.baseUrl}/${id}`);
  }

  create(project: Partial<Project>): Observable<Project> {
    return this.http.post<Project>(this.baseUrl, project);
  }

  update(id: number, project: Partial<Project>): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, project);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
