using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using TrainingAPI001;
using TrainingAPI001.Entities;
using TrainingAPI001.Repositories;

var builder = WebApplication.CreateBuilder(args);

string? firstName = "Training 001";
string firstNameInUpperCase = firstName.ToUpper();

//if (firstName is not null)
//{
//    string firstNameInUpperCase = firstName.ToUpper();
//}

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

app.MapGet("/genres", [EnableCors(policyName: "free")] () =>
{
    var genres = new List<Genre>()
    {
        new Genre 
        {
            Id = 1,
            Name = "Drama"
        },
        new Genre
        {
            Id = 2,
            Name = "Funny"
        },
        new Genre
        {
            Id = 3,
            Name = "Dev101"
        }
    };
    return genres;
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(15)));

app.MapPost("/genres", async (Genre genre, IGenresRepository repository) =>
{
    var id = await repository.Create(genre);
    return Results.Created($"/genres/{id}", genre);
});

app.Run();
