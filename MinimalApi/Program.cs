using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ToDoWebApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using AutoMapper;
using Repositories;
using Services;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddDbContext<ToDoContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));      //DefaultConnection dichiarata in appsettings.Development.json
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>               //serve per documentazione
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v0.1",
        Title = "ToDoWeb API",
        Description = "First try at .NET web api developing by Manuel",
        TermsOfService = new Uri("https://www.youtube.com/watch?v=dQw4w9WgXcQ&pp=ygUXbmV2ZXIgZ29ubmEgZ2l2ZSB5b3UgdXA%3D"), 
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddAutoMapper(typeof(ToDoWebApi.Mapper.MappingProfile).Assembly);  //chiedere .assembly
builder.Services.AddScoped<ToDoItemListRepository>();
builder.Services.AddScoped<ToDoItemListService>();
builder.Services.AddScoped<ToDoItemRepository>();
builder.Services.AddScoped<ToDoItemService>();


var app = builder.Build();

app.MapGet("/api/lists", async () =>
{
    using (var scope = app.Services.CreateScope())
    {
        var service = scope.ServiceProvider.GetRequiredService<ToDoItemListService>();
        await service.GetAllLists();
    }
});

app.MapGet("/api/lists/{id}", async (long id) =>
{
    using (var scope = app.Services.CreateScope())
    {
        var service = scope.ServiceProvider.GetRequiredService<ToDoItemListService>();
        await service.GetList(id);
    }
});

app.MapPost("/api/lists", async (ToDoItemListDTO toDoItemListDto) =>
{
    using (var scope = app.Services.CreateScope())
    {
        var service = scope.ServiceProvider.GetRequiredService<ToDoItemListService>();
        await service.InsertList(toDoItemListDto);
    }
});

app.MapPut("/api/lists/{id}", async (long id, ToDoItemListDTO toDoItemListDto) =>
{
    using (var scope = app.Services.CreateScope())
    {
        var service = scope.ServiceProvider.GetRequiredService<ToDoItemListService>();
        await service.UpdateList(id, toDoItemListDto);
    }
});

app.MapDelete("/api/lists/{id}", async (long id) =>
{
    using (var scope = app.Services.CreateScope())
    {
        var service = scope.ServiceProvider.GetRequiredService<ToDoItemListService>();
        await service.DeleteList(id);
    }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthorization();      dà problemi di authorization se usato questo comando



app.Run();