using APAERMENT_LAST_API.Configurations;
using APAERMENT_LAST_API.Repositories;
using APAERMENT_LAST_API.Repositories.Interfaces;
using APAERMENT_LAST_API.Services;
using APAERMENT_LAST_API.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var conStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseOracle(conStr);
});
// ========================
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<APAERMENT_LAST_API.Middlewares.ExceptionMiddleware>();

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
// ///=========Add Automapper====
builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration).Assembly);

// //=============Add Scoped=========
// //====Respository=====
builder.Services.AddScoped<IBuildingRepository, BuildingRepository>();

// //=====Service====
builder.Services.AddScoped<IBuildingService, BuildingService>();