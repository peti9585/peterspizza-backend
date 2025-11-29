using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using Autofac.Extensions.DependencyInjection;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using PetersPizza.Api.Application.SignalR;
using PetersPizza.Api.Infrastructure.API.Host.Middlewares;
using PetersPizza.Api.Service;
using PetersPizza.Api.Service.Transformers;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(AutofacConfiguration.CreateContainer));

builder.Services.AddOpenApi(o =>
{
    o.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
});
builder.Services.AddSwaggerGen();
builder.Services.AddCarter();
builder.Services.AddSignalR();

var allowedOrigins = new[]
{
    "http://localhost:4200"
};

builder.Services.AddCors(options =>
{
    options.AddPolicy(Constants.DefaultCorsPolicy, policy =>
    {
        policy.WithOrigins(allowedOrigins)
            .AllowCredentials()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAntiforgery();
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(Constants.User, p => p.RequireRole(Constants.User));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false; // TODO: Enable in production
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = Constants.ApplicationName,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration[Constants.JwtKey]!)),
            ClockSkew = TimeSpan.Zero
        };
        
        o.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    path.StartsWithSegments("/ordersHub"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("RateLimitPolicy", context =>
    {
        var userId = context.User.Identity?.Name 
                     ?? context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId)) userId = "anonymous";

        return RateLimitPartition.GetTokenBucketLimiter(userId, _ =>
            new TokenBucketRateLimiterOptions
            {
                TokenLimit = 5,
                TokensPerPeriod = 5,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", Constants.ApplicationName);
    });
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/images"
});

app.UseCors(Constants.DefaultCorsPolicy);
//app.UseHttpsRedirection(); TODO: Enable in production

app.UseRouting();
app.UseAntiforgery();

app.UseMiddleware<UnhandledExceptionFilterMiddleware>();
app.MapCarter();
app.MapHub<OrdersHub>("/ordersHub");

app.UseAuthentication();
app.UseAuthorization();

app.UseRateLimiter();

app.Run();