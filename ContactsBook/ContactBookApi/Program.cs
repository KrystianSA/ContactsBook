using ContactsBook.Models;
using ContactsBook.Models.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RecruitmentTask;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;
using RecruitmentTask.Mapper;
using RecruitmentTask.Models;
using RecruitmentTask.Services;
using System.Text;
using WebApp.Models.Validators;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration["ConnectionString:DefaultConnection"];
builder.Services.AddDbContext<DataDbContext>(options=>
        options.UseSqlServer(connectionString));

builder.Services.AddAutoMapper(typeof(ContactsMappingProfile));
builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DataProviders>();
builder.Services.AddScoped<IContactService,ContactService>();
builder.Services.AddScoped<IAccountService,AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<RegisterUser>, RegisterUserValidator>();
builder.Services.AddScoped<IValidator<CreateOrUpdateContactDto>,CreateContactDtoValidator>();
builder.Services.AddScoped<IValidator<CreateOrUpdateContactDto>, UpdateContactDtoValidator>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin", builder =>
    {
        builder.AllowAnyMethod();
        builder.AllowAnyHeader();
        builder.AllowAnyOrigin();
    });
});

var authenticationSettings = new AutheticationSettings();
builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);

builder.Services.AddSingleton(authenticationSettings);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme="Bearer";
    options.DefaultScheme="Bearer";
    options.DefaultChallengeScheme="Bearer";
}).AddJwtBearer(options =>
{
    options.SaveToken=true;
    options.RequireHttpsMetadata=false;
    options.TokenValidationParameters= new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = authenticationSettings.JwtIssuer,
        ValidAudience = authenticationSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey))
    };
});

var app = builder.Build();

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DataProviders>();
    seeder.AddData();
}

app.UseSwagger();

app.UseSwaggerUI(x =>
{
    x.SwaggerEndpoint("/swagger/v1/swagger.json", "RecruitmentTask api");
});

app.UseAuthentication();

app.UseRouting();

app.MapControllers();

app.UseAuthorization();

app.UseCors("AllowOrigin");

app.Run();

public partial class Program { }