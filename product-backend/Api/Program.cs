using Api.Configuration;
using Api.ExceptionHandling;
using Application;
using Infrastructure;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services
    .AddOptions<CorsOptions>()
    .Bind(builder.Configuration.GetSection(CorsOptions.SectionName))
    .Validate(
        options => options.AllowedOrigins.Length > 0 &&
            options.AllowedOrigins.All(origin =>
                Uri.TryCreate(origin, UriKind.Absolute, out var uri) &&
                uri.Scheme is "http" or "https" &&
                uri.PathAndQuery == "/"),
        "Cors:AllowedOrigins must contain absolute HTTP or HTTPS origins.")
    .ValidateOnStart();
builder.Services.AddCors();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
var corsOptions =
    app.Services.GetRequiredService<IOptions<CorsOptions>>().Value;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy
    .WithOrigins(corsOptions.AllowedOrigins)
    .AllowAnyHeader()
    .AllowAnyMethod());

app.UseAuthorization();

app.UseExceptionHandler();

app.MapGet("/health", () => Results.Ok(new { status = "healthy" }));
app.MapControllers();

app.Run();

public partial class Program
{
}
