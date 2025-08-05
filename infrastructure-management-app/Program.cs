using Domain.Entities;
using infrastructure_management_app;
using infrastructure_management_app.Configuration.LogColumnWriter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Persistence;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using NpgsqlTypes;
using Persistence;
using Serilog;
using Serilog.Events;
using Serilog.Context;
using Serilog.Core;
using Serilog.Sinks.PostgreSQL;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using System;
using Persistence.Context;

var builder = WebApplication.CreateBuilder(args);



builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddUserSecrets<Program>() // 🔐 This enables reading from secrets.json
    .AddEnvironmentVariables();


var config = builder.Configuration;
var allowedOrigins = config["AllowedOrigins"].Split(',');

// Register AppDbContext with PostgreSQL
builder.Services.AddDbContext<MaintenanceDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration["ConnectionStrings:PostgreSQL"],
        b => b.MigrationsAssembly("infrastructure_management_app")
    ));


//builder.Services.AddPersistenceService();
Console.WriteLine($"[DEBUG] PostgreSQL Conn: {builder.Configuration.GetConnectionString("PostgreSQL")}");
builder.Services.AddPersistenceService(builder.Configuration);
builder.Services.AddValidationService();

Logger log = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/Log.txt")
    .WriteTo.PostgreSQL(builder.Configuration.GetConnectionString("PostgreSQL"), "logs",
        needAutoCreateTable: true,
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            {"message", new RenderedMessageColumnWriter(NpgsqlDbType.Text)},
            {"message_template", new MessageTemplateColumnWriter(NpgsqlDbType.Text)},
            {"level", new LevelColumnWriter(true , NpgsqlDbType.Varchar)},
            {"time_stamp", new TimestampColumnWriter(NpgsqlDbType.Timestamp)},
            {"exception", new ExceptionColumnWriter(NpgsqlDbType.Text)},
            {"log_event", new LogEventSerializedColumnWriter(NpgsqlDbType.Json)},
            {"user_name", new UserNameColumnWriter()}
        })
    //.WriteTo.Seq(builder.Configuration["Seq:ServerURL"])
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog(log);
builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});



//builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});



//builder.Services.AddSwaggerGen(c =>
//{
//c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });
//// Configure the Swagger generator to use string values for enums
//c.SchemaFilter<EnumSchemaFilter>();
//});
// Create a custom schema filter to handle enums



builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(config["JWT:Key"].ToString());
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["JWT:Issuer"],
        ValidAudience = config["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key),

        NameClaimType = ClaimTypes.Name,

        //ClockSkew = TimeSpan.Zero    // this will ensure the toke expire time refering to your settings
    };
});

// disable cors


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(allowedOrigins)
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

//app.UseSerilogRequestLogging();
app.UseHttpLogging();

// disable cors
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication(); 
app.UseAuthorization();

// log config
app.Use(async (context, next) =>
{
    var userName = context.User?.Identity?.IsAuthenticated != null || true ? context.User.Identity.Name : null;
    LogContext.PushProperty("user_name", userName);
    await next();
});

app.MapControllers();

app.Run();

