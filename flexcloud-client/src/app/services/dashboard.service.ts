import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { DashboardKpis, LeadsByStatus, MonthlyPipeline, ProjectsByStatus } from '../models/models';

@Injectable({ providedIn: 'root' })
export class DashboardService {
  private baseUrl = `${environment.apiUrl}/dashboard`;

  constructor(private http: HttpClient) {}

  getKpis(): Observable<DashboardKpis> {
    return this.http.get<DashboardKpis>(`${this.baseUrl}/kpis`);
  }

  getLeadsByStatus(): Observable<LeadsByStatus[]> {
    return this.http.get<LeadsByStatus[]>(`${this.baseUrl}/leads-by-status`);
  }

  getProjectsByStatus(): Observable<ProjectsByStatus[]> {
    return this.http.get<ProjectsByStatus[]>(`${this.baseUrl}/projects-by-status`);
  }

  getMonthlyPipeline(months = 6): Observable<MonthlyPipeline[]> {
    return this.http.get<MonthlyPipeline[]>(`${this.baseUrl}/monthly-pipeline`, {
      params: { months: String(months) }
    });
  }
}
