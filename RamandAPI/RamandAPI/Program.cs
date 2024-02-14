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
using Quartz;
using RamandAPI.QuartzOperations;
using Quartz.Impl;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("DailyJob", "JobGroup");
    q.AddJob<DailyJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("MyTrigger", "TriggerGroup")
        .WithCronSchedule("0 0/37 15-22 ? * * *"));
});

builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true);
ISchedulerFactory schedFact = new StdSchedulerFactory();
IScheduler sched = schedFact.GetScheduler().Result;
sched.Start();
IJobDetail job = JobBuilder.Create<DailyJob>()
    .WithIdentity(name: "DailyJob", group: "JobGroup")
    .Build();
ITrigger trigger = TriggerBuilder.Create()
    .WithIdentity(name: "MyTrigger", group: "TriggerGroup")
    .WithCronSchedule("0 0/37 15-22 ? * * *")
    .Build();
sched.ScheduleJob(job, trigger);


builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
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
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
