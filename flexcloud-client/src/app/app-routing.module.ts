import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ProjectListComponent } from './components/project-list/project-list.component';
import { ProjectFormComponent } from './components/project-form/project-form.component';
import { DocumentListComponent } from './components/document-list/document-list.component';
import { DocumentFormComponent } from './components/document-form/document-form.component';
import { ContactListComponent } from './components/contact-list/contact-list.component';
import { ContactFormComponent } from './components/contact-form/contact-form.component';
import { LeadListComponent } from './components/lead-list/lead-list.component';
import { LeadFormComponent } from './components/lead-form/lead-form.component';

const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  { path: 'projects', component: ProjectListComponent },
  { path: 'projects/new', component: ProjectFormComponent },
  { path: 'projects/:id/edit', component: ProjectFormComponent },
  { path: 'documents', component: DocumentListComponent },
  { path: 'documents/new', component: DocumentFormComponent },
  { path: 'contacts', component: ContactListComponent },
  { path: 'contacts/new', component: ContactFormComponent },
  { path: 'contacts/:id/edit', component: ContactFormComponent },
  { path: 'leads', component: LeadListComponent },
  { path: 'leads/new', component: LeadFormComponent },
  { path: 'leads/:id/edit', component: LeadFormComponent },
  { path: '**', redirectTo: 'dashboard' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
