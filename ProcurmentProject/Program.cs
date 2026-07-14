using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using ProcurmentProject.Dto;
using ProcurmentProject.Helper;
using ProcurmentProject.Interfaces;
using ProcurmentProject.Data.Models;
using ProcurmentProject.Repositories;
using ProcurmentProject.Services;
using System.Text;
using Microsoft.OpenApi.Models;
using ProcurmentProject.Middleware;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(option => {
    option.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    option.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMemoryCache();


var signingKey = builder.Configuration.GetConnectionString("SigningKey");

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "ProcurmentProject",
        ValidAudience = "ProcurmentProject",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!))
    };
});

// add bearer token in header of each endpoint
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "Jwt",
        In = ParameterLocation.Header,
        Description = "Enter Bearer Token"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            { 
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
// Db Injection
builder.Services.AddDbContext<ProcurmentSystemContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
contextLifetime: ServiceLifetime.Transient, 
optionsLifetime: ServiceLifetime.Transient
);

builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<ICompany, CompanyRepository>();
builder.Services.AddScoped<IRole,RoleRepository>();
builder.Services.AddScoped<IProduct, ProductRepository>();
builder.Services.AddScoped<IPurchasedRequisition, PurchasedRequisitionRepository>();
builder.Services.AddScoped<ISupplier, SupplierRepository>();
builder.Services.AddScoped<IRequestForQuotation, RequestForQuotationRepository>();
builder.Services.AddScoped<ISupplierQuotation, SupplierQuotationRepository>();
builder.Services.AddScoped<ISuppliesDelivery, SuppliesDeliveryRepository>();

// services Dependency Injection
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddSingleton<DocumentUploader>();

// stateless class one instance for whole project
builder.Services.AddSingleton<PermissionChecker>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
        if (exceptionHandler?.Error != null)
        {
            app.Logger.LogError(exceptionHandler.Error, "Unhandled exception.");
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new ResponseModel
        {
            Success = false,
            Message = "An unexpected error occurred."
        });
    });
});

app.UseHttpsRedirection();

app.UseMiddleware<ApiProtectionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
