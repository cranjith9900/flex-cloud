using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlexCloud.Api.Data;
using FlexCloud.Api.DTOs;
using FlexCloud.Api.Models;
using Microsoft.Extensions.Caching.Memory;

namespace FlexCloud.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly FlexCloudDbContext _db;
    private readonly IMemoryCache _cache;

    public DashboardController(FlexCloudDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    // In Azure this KPI summary is cached in Redis; here IMemoryCache stands in locally.
    [HttpGet("kpis")]
    public async Task<ActionResult<DashboardKpisDto>> GetKpis()
    {
        if (_cache.TryGetValue("dashboard:kpis", out DashboardKpisDto? cached) && cached is not null)
            return Ok(cached);

        var totalLeads = await _db.Leads.CountAsync();
        var totalProjects = await _db.Projects.CountAsync();
        var totalDocuments = await _db.Documents.CountAsync();
        var pipelineValue = await _db.Leads
            .Where(l => l.Status == LeadStatus.New || l.Status == LeadStatus.Qualified)
            .SumAsync(l => (decimal?)l.EstimatedValue) ?? 0m;
        var activeBudget = await _db.Projects
            .Where(p => p.Status == ProjectStatus.InProgress || p.Status == ProjectStatus.Planning)
            .SumAsync(p => (decimal?)p.Budget) ?? 0m;

        var convertedCount = await _db.Leads.CountAsync(l => l.Status == LeadStatus.Converted);
        var conversionRate = totalLeads == 0 ? 0 : Math.Round((decimal)convertedCount / totalLeads * 100, 1);

        var dto = new DashboardKpisDto(totalLeads, totalProjects, totalDocuments, pipelineValue, activeBudget, conversionRate);

        _cache.Set("dashboard:kpis", dto, TimeSpan.FromMinutes(1));
        return Ok(dto);
    }

    [HttpGet("leads-by-status")]
    public async Task<ActionResult<IEnumerable<LeadsByStatusDto>>> LeadsByStatus()
    {
        var data = await _db.Leads
            .GroupBy(l => l.Status)
            .Select(g => new LeadsByStatusDto(g.Key.ToString(), g.Count(), g.Sum(l => l.EstimatedValue)))
            .ToListAsync();

        return Ok(data);
    }

    [HttpGet("projects-by-status")]
    public async Task<ActionResult<IEnumerable<ProjectsByStatusDto>>> ProjectsByStatus()
    {
        var data = await _db.Projects
            .GroupBy(p => p.Status)
            .Select(g => new ProjectsByStatusDto(g.Key.ToString(), g.Count(), g.Sum(p => p.Budget)))
            .ToListAsync();

        return Ok(data);
    }

    [HttpGet("monthly-pipeline")]
    public async Task<ActionResult<IEnumerable<MonthlyPipelineDto>>> MonthlyPipeline([FromQuery] int months = 6)
    {
        var since = DateTime.UtcNow.AddMonths(-months);
        var leads = await _db.Leads.Where(l => l.CreatedDate >= since).ToListAsync();

        var trend = leads
            .GroupBy(l => l.CreatedDate.ToString("yyyy-MM"))
            .Select(g => new MonthlyPipelineDto(
                g.Key,
                g.Where(l => l.Status != LeadStatus.Converted).Sum(l => l.EstimatedValue),
                g.Where(l => l.Status == LeadStatus.Converted).Sum(l => l.EstimatedValue)))
            .OrderBy(t => t.Period)
            .ToList();

        return Ok(trend);
    }
}
