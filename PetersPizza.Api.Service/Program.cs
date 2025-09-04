using System.Text;
using Autofac.Extensions.DependencyInjection;
using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

app.UseCors(Constants.DefaultCorsPolicy);
//app.UseHttpsRedirection(); TODO: Enable in production
app.UseRouting();

app.UseMiddleware<UnhandledExceptionFilterMiddleware>();
app.MapCarter();

app.UseAuthentication();
app.UseAuthorization();

app.Run();