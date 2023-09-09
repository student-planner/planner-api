using Microsoft.EntityFrameworkCore;
using Planner.API;
using Planner.API.Helpers;
using Planner.API.Options;
using Planner.API.Services;
using Seljmov.AspNet.Commons.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<JwtCreator>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));

ConfigureEmailSender(builder);

var app = builder.BuildWebApplication();

app.Run();

void ConfigureEmailSender(WebApplicationBuilder webApplicationBuilder)
{
    var smtpClientOptions = webApplicationBuilder.Configuration.GetSection(nameof(SmtpClientOptions)).Get<SmtpClientOptions>();
    if (smtpClientOptions == null)
    {
        throw new Exception("SmtpClientOptions is null");
    }

    var codeTemplateOptions = webApplicationBuilder.Configuration.GetSection(nameof(CodeTemplateOptions)).Get<CodeTemplateOptions>();
    if (codeTemplateOptions == null)
    {
        throw new Exception("CodeTemplateOptions is null");
    }
    webApplicationBuilder.Services.AddSingleton(new EmailSenderService(smtpClientOptions, codeTemplateOptions));
}