using Hackathon.AuthService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Prometheus;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Auth Service API",
        Version = "v1",
        Description = "Microsservi�o de autentica��o"
    });

    // Configura��o para aceitar JWT no Swagger
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Insira o token JWT no campo abaixo: Bearer {seu_token}",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(opt =>
   {
       opt.TokenValidationParameters = new()
       {
           ValidateIssuer = false,
           ValidateAudience = false,
           ValidateLifetime = true,
           IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("minha-chave-super-secreta") // mesma usada para gerar o token
           )
       };
   });

builder.Services.AddDbContext<AppDbContext>(opt =>
   opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

var app = builder.Build();

app.UseHttpMetrics(); // Prometheus  

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hackathon Auth Service API v1");
    c.RoutePrefix = ""; // para mostrar na raiz do app (localhost:5000)
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapMetrics();     // Prometheus  

app.Run();
