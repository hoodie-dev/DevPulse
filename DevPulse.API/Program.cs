using DevPulse.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// extract connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DevPulseDbContext>(options => 
options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DevPulse.Infrastructure")));
builder.Services.AddScoped<DevPulse.Application.Common.Interfaces.IDevPulseDbContext>(provider => provider.GetRequiredService<DevPulse.Infrastructure.Data.DevPulseDbContext>());

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(DevPulse.Application.Projects.Commands.CreateProjectCommand).Assembly);
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

