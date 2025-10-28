using AutoMapper;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RecruitingChallenge.API.Helpers;
using RecruitingChallenge.API.Middlewares;
using RecruitingChallenge.DAL;
using RecruitingChallenge.DAL.Repositories.Order;
using RecruitingChallenge.DAL.Repositories.User;
using RecruitingChallenge.Mapper;
using RecruitingChallenge.Service.Orders;
using RecruitingChallenge.Service.Users;

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

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// to do lean - check que pasa sin esto
builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<JwtHelper, JwtHelper>();

builder.Services.AddSingleton(mapper);

builder.Services.AddDbContext<OrderNowDbContext>(options =>
{
    options
        .UseSqlServer(builder.Configuration.GetConnectionString("DbConn"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

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
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderNowDbContext>();
    context.Database.Migrate();
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
