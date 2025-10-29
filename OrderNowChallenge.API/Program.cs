using AutoMapper;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OrderNowChallenge.API.Helpers;
using OrderNowChallenge.API.Middlewares;
using OrderNowChallenge.Common.Helpers;
using OrderNowChallenge.DAL;
using OrderNowChallenge.DAL.Repositories.Order;
using OrderNowChallenge.DAL.Repositories.User;
using OrderNowChallenge.Mapper;
using OrderNowChallenge.Service.Orders;
using OrderNowChallenge.Service.Users;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields =
        HttpLoggingFields.RequestPath |
        HttpLoggingFields.RequestProperties |
        HttpLoggingFields.RequestBody |
        HttpLoggingFields.RequestMethod |
        HttpLoggingFields.ResponseBody |
        HttpLoggingFields.ResponseStatusCode;

    logging.RequestHeaders.Add("My-Request-Header");
    logging.ResponseHeaders.Add("My-Response-Header");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;
});

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MapperProfile());
});

var mapper = config.CreateMapper();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtHelper, JwtHelper>();

builder.Services.AddSingleton(mapper);

// Only register SQL Server if not in testing environment
if (!builder.Environment.EnvironmentName.Equals("IntegrationTests", StringComparison.OrdinalIgnoreCase))
{
    builder.Services.AddDbContext<OrderNowDbContext>(options =>
    {
        options
            .UseSqlServer(builder.Configuration.GetConnectionString("DbConn"))
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    });
}

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
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

var app = builder.Build();

Environment.SetEnvironmentVariable("Environment", builder.Configuration["Environment"]);

if (!app.Environment.EnvironmentName.Equals("IntegrationTests", StringComparison.OrdinalIgnoreCase))
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<OrderNowDbContext>();
        context.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggerHttpRequest>();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<ValidateTokenMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
