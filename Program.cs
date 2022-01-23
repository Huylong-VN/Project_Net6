using CRM_Management_Student.Backend.Application;
using CRM_Management_Student.Backend.Data;
using CRM_Management_Student.Backend.Data.Entity;
using CRM_Management_Student.Backend.SignalR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
IWebHostEnvironment environment = builder.Environment;
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddCors(); 
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<INotificationService, NotificationService>();
builder.Services.AddTransient<IClassService, ClassService>();
builder.Services.AddTransient<ISubjectService, SubjectService>();
// Add DbContext
var a=builder.Configuration["JwtConfig:Issuer"];
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<AppDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddIdentity<AppUser, AppRole>(opt =>
{
   // opt.Lockout.AllowedForNewUsers = true;
   // opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2);
   //opt.Lockout.MaxFailedAccessAttempts = 3;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
               .AddJwtBearer(jwt =>
               {
                   var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Key"]);
                   jwt.SaveToken = true;
                   jwt.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuerSigningKey = true, // this will validate the 3rd part of the jwt token using the secret that we added in the appsettings and verify we have generated the jwt token
                       IssuerSigningKey = new SymmetricSecurityKey(key), // Add the secret key to our Jwt encryption
                       ValidateIssuer = true,
                       ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                       ValidAudience = builder.Configuration["JwtConfig:Audience"],
                       ValidateAudience = true,
                       ValidateLifetime = true
                   };
                   jwt.Events = new JwtBearerEvents
                   {
                       OnMessageReceived = context =>
                       {
                           var accessToken = context.Request.Query["access_token"];

                           // If the request is for our hub...
                           var path = context.HttpContext.Request.Path;
                           if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/hub")))
                           {
                               // Read the token out of the query string
                               context.Token = accessToken;
                           }
                           return Task.CompletedTask;
                       }
                   };

               });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsApp.BackendApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                    {
                        Reference=new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        },
                        Scheme="oauth2",
                        Name="Bearer",
                        In=ParameterLocation.Header
                    },
                    new List<string>()
                    }
                });
});


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure Swagger
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});
app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();

app.UseAuthentication();

// global cors policy
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials()); // allow credentials
app.UseAuthorization();
app.MapControllers();
app.MapHub<SignalHub>("/hub");


app.Run();
