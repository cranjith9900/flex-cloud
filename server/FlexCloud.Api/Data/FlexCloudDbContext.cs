using Microsoft.EntityFrameworkCore;
using FlexCloud.Api.Models;

namespace FlexCloud.Api.Data;

public class FlexCloudDbContext : DbContext
{
    public FlexCloudDbContext(DbContextOptions<FlexCloudDbContext> options) : base(options) { }

    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Lead> Leads => Set<Lead>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Document> Documents => Set<Document>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Lead>()
            .HasOne(l => l.Contact)
            .WithMany(c => c.Leads)
            .HasForeignKey(l => l.ContactId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Contact)
            .WithMany(c => c.Projects)
            .HasForeignKey(p => p.ContactId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Document>()
            .HasOne(d => d.Project)
            .WithMany(p => p.Documents)
            .HasForeignKey(d => d.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Lead>().Property(l => l.Status).HasConversion<string>();
        modelBuilder.Entity<Project>().Property(p => p.Status).HasConversion<string>();
        modelBuilder.Entity<Document>().Property(d => d.Type).HasConversion<string>();

        modelBuilder.Entity<Contact>().HasData(
            new Contact { Id = 1, FullName = "Maria Gonzalez", Email = "maria@brightspace-interiors.com", Phone = "555-0110", Company = "Bright Space Interiors", CreatedDate = new DateTime(2024, 01, 10) },
            new Contact { Id = 2, FullName = "David Chen", Email = "dchen@urbanlofts.com", Phone = "555-0144", Company = "Urban Lofts LLC", CreatedDate = new DateTime(2024, 03, 22) }
        );

        modelBuilder.Entity<Lead>().HasData(
            new Lead { Id = 1, Name = "Showroom Redesign", Source = "Referral", Status = LeadStatus.Qualified, EstimatedValue = 45000m, CreatedDate = new DateTime(2024, 02, 01), ContactId = 1 },
            new Lead { Id = 2, Name = "Loft Renovation", Source = "Website", Status = LeadStatus.Converted, EstimatedValue = 80000m, CreatedDate = new DateTime(2024, 03, 25), ContactId = 2 },
            new Lead { Id = 3, Name = "Office Fit-Out", Source = "Trade Show", Status = LeadStatus.New, EstimatedValue = 120000m, CreatedDate = new DateTime(2024, 05, 05), ContactId = 1 }
        );

        modelBuilder.Entity<Project>().HasData(
            new Project { Id = 1, Name = "Urban Loft Renovation", ClientName = "Urban Lofts LLC", Status = ProjectStatus.InProgress, Budget = 80000m, StartDate = new DateTime(2024, 04, 01), ContactId = 2 },
            new Project { Id = 2, Name = "Bright Space Showroom", ClientName = "Bright Space Interiors", Status = ProjectStatus.Planning, Budget = 45000m, StartDate = new DateTime(2024, 06, 15), ContactId = 1 }
        );

        modelBuilder.Entity<Document>().HasData(
            new Document { Id = 1, FileName = "floor-plan-v1.dwg", Type = DocumentType.FloorPlan, Notes = "Initial layout", UploadedDate = new DateTime(2024, 04, 05), ProjectId = 1 },
            new Document { Id = 2, FileName = "client-contract.pdf", Type = DocumentType.Contract, Notes = "Signed agreement", UploadedDate = new DateTime(2024, 04, 02), ProjectId = 1 },
            new Document { Id = 3, FileName = "render-lobby.png", Type = DocumentType.RenderImage, Notes = "3D render of lobby", UploadedDate = new DateTime(2024, 06, 20), ProjectId = 2 }
        );
    }
}
