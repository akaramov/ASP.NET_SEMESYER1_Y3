using APAERMENT_LAST_API.Configurations;
using APAERMENT_LAST_API.Middlewares;
using APAERMENT_LAST_API.Repositories;
using APAERMENT_LAST_API.Repositories.Interfaces;
using APAERMENT_LAST_API.Services;
using APAERMENT_LAST_API.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Net;
using System.Text;

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
// //================Add Swagger=========
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("V1", new OpenApiInfo
    {
        Version = "V1",
        Title = "My API",
        Description = "Oracle Project with React Type Script"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Id = "Bearer", // ត្រូវរត់ទៅរក ID "Bearer" ដែលបានបង្កើតនៅផ្នែកទី ២
                Type = ReferenceType.SecurityScheme
            }
        },
        new List<string>()
    }
});
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(
            "AllowAll",
            policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
        );
    });

    builder.Services.AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            IssuerSigningKey = new SymmetricSecurityKey
            (
                Encoding.UTF8.GetBytes
                (
                    builder.Configuration["JWT:Secret"]!
                )
            )
        };
    });
    app.Use(async (context, next) =>
    {
        await next();
        if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
        {
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                Success = false,
                StatusCode = 401,
                Message = "Unauthorized",
                Data = new { }
            }));
        }
    });

    app.UseCors("AllowAll");

    app.UseAuthentication();

    app.UseMiddleware<ExceptionMiddleware>();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        //app.UseSwaggerUI();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/V1/swagger.json", "Oracle Project with React Type Script");
        });
    }
});