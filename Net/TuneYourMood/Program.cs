using System.Text.Json.Serialization;
using TuneYourMood.Api;
using TuneYourMood.Core.InterfaceRepository;
using TuneYourMood.Core.InterfaceService;
using TuneYourMood.Core;
using TuneYourMood.Data;
using Microsoft.EntityFrameworkCore;
using TuneYourMood.Core.DTOs;
using TuneYourMood.Service;
using TuneYourMood.Data.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(MappingPostProfile));

builder.Services.AddDbContext<DataContext>(option =>
{
    option.UseSqlServer("Data Source=DESKTOP-SSNMLFD;Initial Catalog=TuneYourMood;Integrated Security=True;TrustServerCertificate=True;");
    //option.UseSqlServer("Data Source = DESKTOP-SSNMLFD; Initial Catalog = TuneYourMood; Integrated Security = true; ");
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IService<SongDto>, SongService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ISongRepository, SongRepository>();

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// הוספת JWT Authentication
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
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// הוספת הרשאות מבוסס-תפקידים
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("EditorOrAdmin", policy => policy.RequireRole("Editor", "Admin"));
    options.AddPolicy("ViewerOnly", policy => policy.RequireRole("Viewer"));
});

// Add Swagger services before app.Build()
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
