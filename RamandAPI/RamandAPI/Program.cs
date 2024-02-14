using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using RamandAPI;
using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHangfire(configuration => configuration
        .UseSqlServerStorage("Server=.;Database=Hangfire;User Id=sa;Password=@Admin22;Encrypt=False;"));
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
  options.TokenValidationParameters = new TokenValidationParameters
  {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = builder.Configuration["JWT:issuer"],
      ValidAudience = builder.Configuration["JWT:audience"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]))
  });

var errorLogFilePath = "logs/errors-.txt";
Log.Logger = new LoggerConfiguration()
                            .WriteTo.Console()
                            .WriteTo.File(new JsonFormatter(),
                                          "logs/error-.json",
                                          restrictedToMinimumLevel: LogEventLevel.Error)
                            .WriteTo.File("logs/info-.txt",
                                          rollingInterval: RollingInterval.Day)
                                .MinimumLevel.Debug()
                            .CreateLogger();

builder.Host.UseSerilog();
builder.Services.AddApiVersioning(
    options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
        options.ApiVersionReader = new MediaTypeApiVersionReader("versioning");
    }
);
DI.Configure(builder.Services);
var app = builder.Build();
RabbitSender.init();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHangfireDashboard();
app.UseHangfireServer();

RecurringJob.AddOrUpdate(() => RabbitSender.CallApi(), Cron.Minutely);
RecurringJob.AddOrUpdate(() => Log.Error("sup"),Cron.Minutely);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
