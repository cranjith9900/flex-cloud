using Microsoft.EntityFrameworkCore;
using FlexCloud.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Flex Cloud API", Version = "v1" });
});

var useSqlite = builder.Configuration.GetValue<bool>("UseSqlite");
builder.Services.AddDbContext<FlexCloudDbContext>(options =>
{
    if (useSqlite)
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    else
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FlexCloudDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularClient");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
