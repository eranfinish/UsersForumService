using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using UsersForumService.DAL;
using UsersForumService.DAL.Repositories.Posts;
using UsersForumService.DAL.Repositories.Responses;
using UsersForumService.DAL.Repositories.Users;
using UsersForumService.Helpres;
using UsersForumService.Services.Utils;
using UsersForumService.signalR;
using UsersForumService.Validations;
using services = UsersForumService.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

// Configure EF Core to use SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);

// Define CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Allow your Angular app origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
    //options.AddPolicy("UserForumApp", policy =>
    //{
    //    policy.WithOrigins("http://localhost:4200") // Add URL here
    //          .AllowAnyHeader()                    // Allow all headers
    //          .AllowAnyMethod()                 // Allow all HTTP methods 
    //          .AllowCredentials();                 // Allow credentials if necessary (e.g., cookies or tokens)
    //});
});


// Add services to the container.

builder.Services.AddControllers();

// Add SignalR services to the DI container



// Add FluentValidation services
builder.Services.AddFluentValidationAutoValidation();
//Genetrate validation on all object under the same assembly
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Add Swagger and configure JWT Bearer Authorization
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User Forum API", Version = "v1" });

    // Custom schema ID resolver to avoid conflicts between entities with the same name
    c.CustomSchemaIds(type => type.FullName); // Use full name (namespace + class name) as schema ID

    // Add JWT Authentication to Swagger (your existing setup)
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT token with Bearer prefix. Example: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//DI
builder.Services.AddScoped<IUtilsService, UtilsService>();

builder.Services.AddScoped<services.Users.IUserService, services.Users.UserService>();

builder.Services.AddScoped<services.Posts.IPostService, services.Posts.PostService>();

builder.Services.AddScoped<services.Responses.IResponseService, services.Responses.ResponseService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPostRepository, PostRepository>();

builder.Services.AddScoped<IResponseRepository, ResponseRepository>();



// Add JWT Bearer authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)

     //JWT Bearer Allows secured authentication and authorization from client
     .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))//using in code key for security
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Read the access token from the query string when connecting to SignalR hub
            var accessToken = context.Request.Query["access_token"];

            // If the request is for the SignalR hub, set the token
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) &&
                (path.StartsWithSegments("/forumHub")))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();





var app = builder.Build();



app.Use(async (context, next) =>
{
    var jwtBearerOptions = context.RequestServices.GetRequiredService<IOptionsMonitor<JwtBearerOptions>>();
    var tokenParams = jwtBearerOptions.Get(JwtBearerDefaults.AuthenticationScheme).TokenValidationParameters;

    Console.WriteLine($"Issuer: {tokenParams.ValidIssuer}");
    Console.WriteLine($"Audience: {tokenParams.ValidAudience}");
    await next();
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Enable the CORS policy for the API
//app.UseCors("UserForumApp");

// Ensure DB is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated(); // Ensures the database is created if it doesn't exist
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseMiddleware<AuthorizationMiddleware>();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();



app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ForumHub>("/forumHub");  // Add the SignalR Hub endpoint
});
//app.MapControllers();

app.Run();
