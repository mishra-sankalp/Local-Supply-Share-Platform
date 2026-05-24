using System.Text;
using Hangfire;
using Hangfire.PostgreSql;
using LocalSupply.API.Data;
using LocalSupply.API.Interceptor;
using LocalSupply.API.Middleware;
using LocalSupply.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<AuditableEntityInterceptor>();
// Database with PostGIS
builder.Services.AddDbContext<AppDBContext>((serviceProvider, options) =>
{
    var auditableEntityInterceptor = serviceProvider.GetRequiredService<AuditableEntityInterceptor>();

    // Use UseNpgsql instead of UseSqlServer
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(auditableEntityInterceptor); 
});
// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(
        builder.Configuration.GetConnectionString("Redis")!));

// SignalR
builder.Services.AddSignalR();

// JWT Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
        // Allow SignalR to read token from query string
        options.Events = new JwtBearerEvents {
            OnMessageReceived = ctx => {
                var token = ctx.Request.Query["access_token"];
                if (!string.IsNullOrEmpty(token))
                    ctx.Token = token;
                return Task.CompletedTask;
            }
        };
    });

// Hangfire
// builder.Services.AddHangfire(config =>
//     config.UsePostgreSqlStorage(
//         builder.Configuration.GetConnectionString("DefaultConnection")));
// builder.Services.AddHangfireServer();

builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<IAuthService, AuthService>();
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
//app.MapHub<ListingHub>("/hubs/listings");
//app.UseHangfireDashboard("/hangfire");

app.Run();