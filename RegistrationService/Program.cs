using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using RegistrationService.Models;
using RegistrationService.Services;
using RegistrationService.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("ConnectDB");
builder.Services.AddDbContext<RegistrationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectDB"));
});

builder.Services.AddSingleton(ab =>
    new BlobServiceClient(builder.Configuration.GetConnectionString("AzureBlob")
));

builder.Services.AddScoped<IBlobServices, BlobServices>();   

var app = builder.Build();

// Cors 
app.UseCors(options =>
{
    options.WithOrigins("http://localhost:3000")
        .AllowAnyHeader()
        .AllowAnyMethod();
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();