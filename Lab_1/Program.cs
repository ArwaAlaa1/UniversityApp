
using Lab_1.CustomMiddeware;
using Lab_1.Extentions;
using Lab_1.Filters;
using Lab_1.Helpers;
using Lab_1.Interfaces;
using Lab_1.Models;
using Lab_1.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Lab_1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<AppNameFilter>(); // Register global filter
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<UniversityDbContext>()
                .AddDefaultTokenProviders();
            var jwtSet = builder.Configuration.GetSection("JwtSettings");
            builder.Services.AddAuthentication(op =>
            {
                op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
                        ValidateLifetime = true,
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("StudentOnly", policy => policy.RequireRole("Student"));
            });

            builder.Services.AddDbContext<UniversityDbContext>(options =>
              options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyUser",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                );
            });


            var app = builder.Build();
            ImagesHelper.Initialize(app.Environment);
            app.UseCors("AllowAnyUser");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
