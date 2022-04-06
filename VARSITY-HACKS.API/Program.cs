using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VARSITY_HACKS.BusinessLogic;
using VARSITY_HACKS.DATA;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.
var connectionString = config.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password = new PasswordOptions
    {
        RequireDigit = false,
        RequiredLength = 6,
        RequiredUniqueChars = 0,
        RequireLowercase = false,
        RequireNonAlphanumeric = false,
        RequireUppercase = false
    };
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();


builder.Services.AddDependencyInjection();
builder.Services.AddCors();


builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = config["Jwt:Issuer"],
        ValidAudience = config["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
    };
});


//google, facebook authentication
builder.Services.AddAuthentication()
    .AddGoogle("google", options =>
     {
         options.ClientId = "452729545424-53qbuc3t0qcmt0v1t4up2sn187svhh28.apps.googleusercontent.com";
         options.ClientSecret = "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";
         options.SignInScheme = IdentityConstants.ExternalScheme;

         //this function is get user google profile image
         options.Scope.Add("profile");
     })
    .AddFacebook("facebook", options =>
     {
         options.AppId = "1709711546047084";
         options.AppSecret = "68692d606d85bcacc4e5146948b42273";
         options.SignInScheme = IdentityConstants.ExternalScheme;
     });
//for Facebook login
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
       .AllowAnyMethod()
       .AllowAnyHeader()
       .SetIsOriginAllowed(origin => true) // allow any origin
       .AllowCredentials()); // allow credentials


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
