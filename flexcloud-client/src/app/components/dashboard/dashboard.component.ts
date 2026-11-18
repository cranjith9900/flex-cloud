import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { DashboardKpis } from '../../models/models';
import { DashboardService } from '../../services/dashboard.service';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent implements OnInit, AfterViewInit {
  kpis: DashboardKpis | null = null;
  error = '';

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.dashboardService.getKpis().subscribe({
      next: (kpis) => (this.kpis = kpis),
      error: () => (this.error = 'Unable to load KPI data. Is the API running?')
    });
  }

  ngAfterViewInit(): void {
    this.loadLeadsChart();
    this.loadProjectsChart();
    this.loadPipelineChart();
  }

  private loadLeadsChart(): void {
    this.dashboardService.getLeadsByStatus().subscribe({
      next: (data) => {
        new Chart('leadsChart', {
          type: 'doughnut',
          data: {
            labels: data.map((d) => d.status),
            datasets: [
              {
                label: 'Leads',
                data: data.map((d) => d.count),
                backgroundColor: ['#0d6efd', '#ffc107', '#198754', '#dc3545']
              }
            ]
          },
          options: { responsive: true, plugins: { title: { display: true, text: 'Leads by Status' } } }
        });
      },
      error: () => (this.error = 'Unable to load report data. Is the API running?')
    });
  }

  private loadProjectsChart(): void {
    this.dashboardService.getProjectsByStatus().subscribe({
      next: (data) => {
        new Chart('projectsChart', {
          type: 'bar',
          data: {
            labels: data.map((d) => d.status),
            datasets: [
              {
                label: 'Budget ($)',
                data: data.map((d) => d.totalBudget),
                backgroundColor: '#0d6efd'
              }
            ]
          },
          options: { responsive: true, plugins: { title: { display: true, text: 'Project Budget by Status' } } }
        });
      },
      error: () => (this.error = 'Unable to load report data. Is the API running?')
    });
  }

  private loadPipelineChart(): void {
    this.dashboardService.getMonthlyPipeline(6).subscribe({
      next: (data) => {
        new Chart('pipelineChart', {
          type: 'line',
          data: {
            labels: data.map((d) => d.period),
            datasets: [
              { label: 'Open Pipeline', data: data.map((d) => d.newLeadValue), borderColor: '#ffc107', fill: false },
              { label: 'Converted', data: data.map((d) => d.convertedValue), borderColor: '#198754', fill: false }
            ]
          },
          options: { responsive: true, plugins: { title: { display: true, text: 'Monthly Pipeline Value' } } }
        });
      },
      error: () => (this.error = 'Unable to load report data. Is the API running?')
    });
  }
}
