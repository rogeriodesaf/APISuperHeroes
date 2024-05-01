using ApiSuperHeroes.Data;
using ApiSuperHeroes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/SuperHeroes", async (ApplicationDbContext context) =>

{
     
    return await context.Heroes.ToListAsync();

});

app.MapPost("/SuperHeroes", async (ApplicationDbContext context, Heroes heroes) =>
{
  
    context.Add(heroes);
    await context.SaveChangesAsync();
    return await context.Heroes.ToArrayAsync();
});

app.MapDelete("/SuperHeroes/{id}", async (ApplicationDbContext context, int id) =>
{
    var hero = await context.Heroes.FindAsync(id);
    if (hero == null) return Results.NotFound("Usuário não localizado");

    context.Heroes.Remove(hero);
        await context.SaveChangesAsync();
        return Results.Ok(await context.Heroes.ToArrayAsync());
    
    
    
});

app.MapPut("SuperHeroes", async (ApplicationDbContext context, Heroes heroes) =>
{
    var heroUpdate = context.Heroes.AsNoTracking().FirstOrDefault(a=>a.Id == heroes.Id);
    if (heroUpdate == null) return Results.NotFound("Herói não localizado");

    heroUpdate.Name = heroes.Name;
    heroUpdate.FirstName = heroes.FirstName;
    heroUpdate.LastName = heroes.LastName;
    heroUpdate.Country = heroes.Country;

    context.Update(heroUpdate);
    await context.SaveChangesAsync();
    return Results.Ok(await context.Heroes.ToArrayAsync());
});

app.MapGet("/SuperHeroes/{id}", async (ApplicationDbContext context, int id) =>
{
    var hero =  await context.Heroes.FirstOrDefaultAsync(a => a.Id == id);
    if(hero is null) return Results.NotFound("Herói não localizado");
    
    return Results.Ok(hero);    
});
app.Run();
