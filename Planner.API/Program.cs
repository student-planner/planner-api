using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Planner.API;
using Planner.API.Helpers;
using Planner.API.Services;
using Seljmov.AspNet.Commons.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<JwtCreator>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(connectionString));

builder.Services.TryAddSingleton<EmailSenderService>();

var app = builder.BuildWebApplication();

app.Run();