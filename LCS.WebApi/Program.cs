using Auth.Data;
using Auth.Models;
using LCS.Persistence.Data;
using LCS.WebApi.CustomMiddlewares;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(x =>
    {
        x.AllowAnyOrigin()
        //.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
        //.WithHeaders("Authorization")
        .AllowAnyHeader()
        .WithMethods("GET", "POST", "PUT", "PATCH", "DELETE");
    });
});
var config = builder.Configuration;
builder.Services.AddSqlServer<LCSDbContext>(config.GetConnectionString("LawConString"), opt =>
{
    opt.EnableRetryOnFailure(2);
});
builder.Services.AddSqlServer<AuthDbContext>(config.GetConnectionString("AuthConString"), op =>
{
    op.EnableRetryOnFailure(2);
});
builder.Services.Configure<AuthConfigModel>(config.GetSection(nameof(AuthConfigModel)));
//builder.Services.AddScoped<IMiddleware, ExceptionHandlerMiddleware>();
//builder.Services.AddAuthorization();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
