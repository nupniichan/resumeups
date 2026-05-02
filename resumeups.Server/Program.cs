using Microsoft.AspNetCore.Http.Features;
using resumeups.Server.Models;
using resumeups.Server.Utils;

EnvReader.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<SecuritySettings>(builder.Configuration.GetSection("Security"));
var security = builder.Configuration.GetSection("Security").Get<SecuritySettings>() ?? new SecuritySettings();

builder.WebHost.ConfigureKestrel(o =>
    o.Limits.MaxRequestBodySize = security.MaxRequestBodyBytes);

builder.Services.Configure<FormOptions>(o =>
    o.MultipartBodyLengthLimit = security.MaxUploadBytes);

var corsOrigins = builder.Configuration.GetSection("Cors:Origins").Get<string[]>() ?? [];
builder.Services.AddCors(o => o.AddDefaultPolicy(p =>
{
    if (corsOrigins.Length == 0)
        p.SetIsOriginAllowed(_ => false);
    else
        p.WithOrigins(corsOrigins);

    p.AllowAnyHeader();
    p.AllowAnyMethod();
}));

builder.Services.AddServices();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
