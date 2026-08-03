using Microsoft.EntityFrameworkCore;
using SisTriage.Application.Mappings;
using SisTriage.Application.Security;
using SisTriage.Application.Services;
using SisTriage.Application.Services.Interfaces;
using SisTriage.Domain.Interfaces;
using SisTriage.Infrastructure.Persistence;
using SisTriage.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(cfg => { }, typeof(PacienteProfile).Assembly);
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ITriagemService, TriagemService>();
builder.Services.AddScoped<IFilaAtendimentoService, FilaAtendimentoService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();