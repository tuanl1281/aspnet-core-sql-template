﻿using NSwag;
using NSwag.Generation.Processors.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Api.Template.Data.Infrastructures;
using Api.Template.Data.Repositories.Common;
using Api.Template.Service.MappingProfiles.Common;

namespace Api.Template.Application.Extensions;

public static class StartupExtension
{
    public static void AddBusinessServices(this IServiceCollection services)
    {
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        #region --- Common ---
        services.AddTransient(typeof(IDbFactory<>), typeof(DbFactory<>));
        services.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        services.AddTransient(typeof(IBaseRepository<,>), typeof(BaseRepository<,>));
        #endregion
    }

    public static void AddMapper(this IServiceCollection services)
    {
        /* Config */
        var profiles = new AutoMapper.MapperConfiguration(_ => _.AddMaps(typeof(MappingProfiles)));
        /* Add service */
        services.AddSingleton(profiles.CreateMapper());
    }

    public static void AddCorsPolicy(this IServiceCollection services)
    {
        services.AddCors(_ => _.AddPolicy("AllowAll", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));
    }
    
    public static void AddSwagger(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration configuration)
    {
        var environmentOfSystem =
            environment.IsDevelopment()
                ? environment.IsStaging()
                    ? "Staging"
                    : "Development"
                : "Production";
        
        var apiVersionDescriptionProvider = 
            services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

        foreach (ApiVersionDescription description in apiVersionDescriptionProvider.ApiVersionDescriptions)
            services.AddSwaggerDocument(document =>
            {
                document.Title = configuration["AppName"] ?? "API";
                document.Description = $"{environmentOfSystem} | Build at {DateTime.Now.ToLocalTime():HH:mm dd/MM/yyyy}";
                document.Version = configuration["AppVersion"] ?? "6.0";

                document.DocumentName = description.GroupName;
                document.PostProcess = _ => _.Info.Version = description.GroupName;
                document.ApiGroupNames = new[] { description.GroupName };

                document.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                document.OperationProcessors.Add(
                    new AspNetCoreOperationSecurityScopeProcessor("JWT"));
                document.AllowReferencesWithProperties = true;
            });
    }
    
    public static void AddJwt(this IServiceCollection services, string key, string issuer, string audience)
    {
        services
            .AddAuthentication(_ =>
            {
                _.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                _.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(_ =>
            {
                _.SaveToken = true;
                _.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = false,
                    ValidIssuer = issuer,
                    ValidAudience = string.IsNullOrEmpty(audience) ? issuer : audience,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(key)),
                    ClockSkew = TimeSpan.Zero,
                };
            });
    }
}  
