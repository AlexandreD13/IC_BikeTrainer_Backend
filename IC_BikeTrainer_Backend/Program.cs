using System.Text;
using AspNetCoreRateLimit;
using IC_BikeTrainer_Backend.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File("Logs/info.log", restrictedToMinimumLevel: LogEventLevel.Information)
    .WriteTo.File("Logs/warning.log", restrictedToMinimumLevel: LogEventLevel.Warning)
    .CreateLogger();

// To use the logger
//
// public class MyService
// {
    // private readonly ILogger<MyService> _logger;
    //
    // public MyService(ILogger<MyService> logger)
    // {
    //     _logger = logger;
    // }
    //
    // public void DoSomething()
    // {
    //     _logger.LogInformation("Doing something!");
    // }
// }
//
// public class MyController : ControllerBase
// {
    // private readonly ILogger<MyController> _logger;
    //
    // public MyController(ILogger<MyController> logger)
    // {
    //     _logger = logger;
    // }
    //
    // [HttpGet]
    // public IActionResult Index()
    // {
    //     _logger.LogInformation("Index accessed");
    //     return Ok();
    // }
// }

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Register services
builder.Services.AddControllers();

builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationServices(builder.Configuration); // Inject dependencies
builder.Services.ConfigureSwagger(); // Configure Swagger

// JWT Token
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "IC_BikeTrainer_Backend API V1");
        options.InjectStylesheet("/swagger-ui/custom.css");
    });
}

app.UseRouting();
app.UseIpRateLimiting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }
