export interface Contact {
  id: number;
  fullName: string;
  email: string;
  phone: string;
  company: string;
  createdDate: string;
}

export type LeadStatus = 'New' | 'Qualified' | 'Converted' | 'Lost';

export interface Lead {
  id: number;
  name: string;
  source: string;
  status: LeadStatus;
  estimatedValue: number;
  createdDate: string;
  contactId: number;
  contactName?: string;
}

export type ProjectStatus = 'Planning' | 'InProgress' | 'OnHold' | 'Completed';

export interface Project {
  id: number;
  name: string;
  clientName: string;
  status: ProjectStatus;
  budget: number;
  startDate: string;
  endDate?: string;
  contactId: number;
  contactName?: string;
  documentCount: number;
}

export type DocumentType = 'FloorPlan' | 'Contract' | 'Invoice' | 'RenderImage' | 'Other';

export interface ProjectDocument {
  id: number;
  fileName: string;
  type: DocumentType;
  notes: string;
  uploadedDate: string;
  projectId: number;
  projectName?: string;
}

export interface DashboardKpis {
  totalLeads: number;
  totalProjects: number;
  totalDocuments: number;
  pipelineValue: number;
  activeProjectBudget: number;
  conversionRate: number;
}

export interface LeadsByStatus {
  status: string;
  count: number;
  totalValue: number;
}

export interface ProjectsByStatus {
  status: string;
  count: number;
  totalBudget: number;
}

export interface MonthlyPipeline {
  period: string;
  newLeadValue: number;
  convertedValue: number;
}
