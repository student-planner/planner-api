using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Planner.AlgorithmPriorityGoals;
using Planner.API;
using Planner.API.Helpers;
using Planner.API.Options;
using Planner.API.Services;
using Seljmov.AspNet.Commons.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<SmtpClientOptions>()
    .Bind(builder.Configuration.GetSection(nameof(SmtpClientOptions)));

builder.Services.AddOptions<CodeTemplateOptions>()
    .Bind(builder.Configuration.GetSection(nameof(CodeTemplateOptions)));

builder.Services.AddScoped<EmailSenderService>();

builder.Services.AddSingleton<JwtHelper>();

builder.Services.AddScoped<IImportanceAlgorithm, ImportanceAlgorithm>();

builder.Services.AddHostedService<CheckGoalsStatusService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));

var app = builder.BuildWebApplication();

app.Run();