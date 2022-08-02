using JobProcessor.Data;
using JobProcessor.Service.Interfaces;
using JobProcessor.Service.Managers;
using JobProcessor.Service.BackgroundJobProcessors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IJobManager, JobManager>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseInMemoryDatabase("testdb");
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<IntegerArraySortingBackgroundJobProcessor>();

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

app.Run();
