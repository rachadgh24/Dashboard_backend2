using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Test2.Application.DependencyInjection;
using Test2.DataLayer.DependencyInjection;
using Test2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// JWT authentication (shared scheme with main API / task1)
var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is not configured.");
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("Jwt:Audience is not configured.");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = signingKey,
        RoleClaimType = "role",
        NameClaimType = "name"
    };
    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = context =>
        {
            var principal = context.Principal;
            if (principal?.Identity is not System.Security.Claims.ClaimsIdentity identity)
                return Task.CompletedTask;
            if (!identity.HasClaim(c => c.Type == "role"))
            {
                var roleClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.Role);
                if (roleClaim != null)
                    identity.AddClaim(new System.Security.Claims.Claim("role", roleClaim.Value));
            }
            if (!identity.HasClaim(c => c.Type == "name"))
            {
                var nameClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.Name)
                    ?? principal.FindFirst("unique_name");
                if (nameClaim != null)
                    identity.AddClaim(new System.Security.Claims.Claim("name", nameClaim.Value));
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsJsonAsync(new { message = "Unauthorized: please sign in again." });
        },
        OnForbidden = context =>
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsJsonAsync(new { message = "Forbidden: you do not have permission to access this resource." });
        }
    };
});
builder.Services.AddAuthorization();

builder.Services.AddScoped<IJwtService, JwtService>();

// CORS policy (adjust frontend URL if needed)
builder.Services.AddCors(options =>
{
    options.AddPolicy("NextPolicy",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Application & Data Layer DI
builder.Services.AddApplicationLayerServices(builder.Configuration);
builder.Services.AddDotNetTrainingCoreContext(builder.Configuration);
builder.Services.AddDataLayerRepositories();

var app = builder.Build();

// Swagger only in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("NextPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();