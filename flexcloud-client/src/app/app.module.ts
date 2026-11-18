import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavComponent } from './components/nav/nav.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { ProjectListComponent } from './components/project-list/project-list.component';
import { ProjectFormComponent } from './components/project-form/project-form.component';
import { DocumentListComponent } from './components/document-list/document-list.component';
import { DocumentFormComponent } from './components/document-form/document-form.component';
import { ContactListComponent } from './components/contact-list/contact-list.component';
import { ContactFormComponent } from './components/contact-form/contact-form.component';
import { LeadListComponent } from './components/lead-list/lead-list.component';
import { LeadFormComponent } from './components/lead-form/lead-form.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    DashboardComponent,
    ProjectListComponent,
    ProjectFormComponent,
    DocumentListComponent,
    DocumentFormComponent,
    ContactListComponent,
    ContactFormComponent,
    LeadListComponent,
    LeadFormComponent
  ],
  imports: [BrowserModule, AppRoutingModule, FormsModule, HttpClientModule],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule {}
