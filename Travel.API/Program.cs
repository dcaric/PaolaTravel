using System.Security.Claims;
using System.Text;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Travel.API.Data;
using Travel.API.Helpers;
using Travel.API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//  Enables Swagger (API testing UI in browser)


// D3
// DbContext in Program.cs
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT TOKEN PART START ****************************************************************************
//builder.Services.AddSwaggerGen();
// Enables Swagger in your API project and lets you customize how the Swagger UI looks and works.
builder.Services.AddSwaggerGen(c =>
{
    // c is swagger configuration object, it is used to configure several things about swagger
    // SwaggerDoc is not so important part it is documentation, title etc.
    c.SwaggerDoc("v1", new() { Title = "Travel API", Version = "v1" });

    // here is configuration how JWT is used in the HTTP requests towards backend
    // Tells Swagger that in the Authorization part of HTTP header JWT is used
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme.  
                        Enter 'Bearer' [space] and then your token in the text input below.  
                        Example: Bearer 12345abcdef",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });


    // here is another configuration which tells that all endpoints, all controllers and HTTP request in all controllers
    // has to use JWT in communication
    // This applies the "Bearer" security definition globally to all endpoints. Otherwise, you�d have to mark each controller manually.
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

// binds JwtSettings class with JwtSettings section in the appsettings.json
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// register custom helper class JwTokenGenerator as a singleton - meaning only one instance will be used in the whole app
builder.Services.AddSingleton<JwtTokenGenerator>();

// adds authentication middleware using "Bearer"
/*
 * This code configures JWT-based authentication in your ASP.NET Core app. When a user sends a 
 * JWT token (like in Authorization: Bearer xxx), this setup tells the app:
 * How to validate the token
 * What values (claims) in the token should be used for things like User.Identity.Name
 */
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
            // means: Use the claim with type ClaimTypes.Name as the value for User.Identity.Name
            // this is used to pass the 
            /*NameClaimType = ClaimTypes.Name tells the system
             * "Use the Name claim from JWT as the current user's identity."
             * This directly affects what User.Identity.Name will return.
             * You should match this to whatever claim you're putting in the token for the user's name.
             * THIS IS USED IN Travet.Web / 
             */
            NameClaimType = ClaimTypes.Name 

        };
    });

builder.Services.AddAuthorization();

// JWT TOKEN PART END ****************************************************************************

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("https://localhost:7066") // frontend origin
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
// enabling JWT generation for each endpoint (securing each API request)
app.UseAuthentication();
app.UseAuthorization();


app.UseAuthorization();

app.MapControllers();

app.Run();
