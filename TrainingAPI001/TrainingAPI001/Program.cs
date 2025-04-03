using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using TrainingAPI001;
using TrainingAPI001.Endpoints;
using TrainingAPI001.Entities;
using TrainingAPI001.Repositories;

var builder = WebApplication.CreateBuilder(args);

string? firstName = "Training 001";
string firstNameInUpperCase = firstName.ToUpper();

// Get Config & Service
var applicationNameC = builder.Configuration.GetValue<string>("ApplicationNameC");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        configuration.WithOrigins(builder.Configuration["allowedOrigins"]!).AllowAnyMethod()
        .AllowAnyHeader();
    });

    options.AddPolicy("free", confiuaration =>
    {
        confiuaration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGenresRepository, GenreRepository>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Middleware
if (builder.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseOutputCache();

app.MapGet("/", () => applicationNameC);

app.MapGroup("/genres").MapGenres();

app.Run();