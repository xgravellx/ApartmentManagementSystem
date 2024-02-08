using System.Text;
using ApartmentManagementSystem.Core.Helpers;
using ApartmentManagementSystem.Core.Initialization;
using ApartmentManagementSystem.Core.Interfaces;
using ApartmentManagementSystem.Core.Profiles;
using ApartmentManagementSystem.Core.Services;
using ApartmentManagementSystem.Infrastructure.Data;
using ApartmentManagementSystem.Infrastructure.Interfaces;
using ApartmentManagementSystem.Infrastructure.Repositories;
using ApartmentManagementSystem.Models.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});
builder.Services.AddIdentity<User, Role>( options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 0;
}).AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

builder.Services.AddHostedService<RolesInitialization>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IApartmentService, ApartmentService>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddScoped<IApartmentRepository, ApartmentRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<TokenGeneratorHelper>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
{
    var signatureKey = builder.Configuration.GetSection("TokenOptions")["SignatureKey"]!;
    var issuer = builder.Configuration.GetSection("TokenOptions")["Issuer"]!;

    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = true,
        ValidIssuer = issuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey))
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
