global using CategoryModel = Core.Category.Category;
global using VaultModel = Core.Vault.Vault;
global using CredentialsModel = Core.Credentials.Credentials;
global using UserModel = Core.User.User;

using Core;
using Core.Category;
using Core.Credentials;
using Core.User;
using Core.Vault;
using Infrastructure.Config;
using Infrastructure.Repositories.EfCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using webapi.Helpers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "PasswordVault API Documentation",
        Description = "OpenAPI documentation style for the API on my Password Vault demo project"
    });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "token",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer",
                }
            },
            new string[]{}
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x => { (new JWTokenGenerator(builder.Configuration)).Configure(x);  });

builder.Services.AddScoped<JWTokenGenerator>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVaultRepository, VaultRepository>();
builder.Services.AddScoped<ICredentialsRepository, CredentialsRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAuthorizationManager, AuthorizationManagerJWT>();
builder.Services.AddScoped<IAuthorizationManagerJWT, AuthorizationManagerJWT>();
builder.Services.AddSingleton<ISymmetricEncryptionManager, AESSymmetricalEncryptionManager>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContextPool<ApplicationDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration["POSTGRES_CONNECTION"]);
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PasswordVault API V1");
    c.DocumentTitle = "PasswordVault API Documentation";
    c.RoutePrefix = string.Empty;
});

app.RegisterApiRoutes();
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.Run();