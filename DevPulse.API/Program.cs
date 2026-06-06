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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.UseCors("AllowAngularClient");
app.UseAuthorization();

app.Run();

