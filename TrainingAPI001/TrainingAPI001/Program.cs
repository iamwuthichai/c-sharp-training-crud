using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.OutputCaching;
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

var genresEndpoints = app.MapGroup("/genres");

app.MapGet("/", () => applicationNameC);

app.MapGet("/genres", async (IGenresRepository repository) =>
{
    return Results.Ok(await repository.GetAll());
}).CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genres-get"));

app.MapGet("/genres/{id:int}", async (int id, IGenresRepository repository) =>
{
    var genre = await repository.GetById(id);

    if (genre is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(genre);
});

app.MapPost("/genres", async (Genre genre, IGenresRepository repository,
    IOutputCacheStore outputCacheStore) =>
{
    var id = await repository.Create(genre);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return Results.Created($"/genres/{id}", genre);
});

app.MapPut("/genres/{id:int}", async (int id, Genre genre, IGenresRepository repository,
    IOutputCacheStore outputCacheStore
    ) =>
{
    var exists = await repository.Exists(id);

    if (!exists)
    {
        return Results.NotFound();
    }

    await repository.Update(genre);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return Results.NoContent();
});

app.MapDelete("/genres/{id:int}", async (int id, IGenresRepository repository, 
    IOutputCacheStore outputCacheStore) =>
{
    var exists = await repository.Exists(id);

    if (!exists)
    {
        return Results.NotFound();
    }

    await repository.Delete(id);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return Results.NoContent();
});

app.Run();
