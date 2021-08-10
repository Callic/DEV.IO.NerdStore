using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NSE.Identidade.API.Data;
using NSE.Identidade.API.Extensions;
using NSE.Identidade.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Identidade.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection IdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()//adicionar as roles do usuário
                .AddErrorDescriber<IdentityPortugueseMessages>()//Traduz os erros do identity
                .AddEntityFrameworkStores<ApplicationDbContext>()//para identificar o context do db que vai ser usado
                .AddDefaultTokenProviders();//Token para validar novo usuário, resetar sehas

            services.TokenConfiguration(configuration);
            return services;
        }
        public static IApplicationBuilder UserIdentityConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
        private static IServiceCollection TokenConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            #region Configuração Token
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(bearerOpt =>
                {
                    bearerOpt.RequireHttpsMetadata = true;
                    bearerOpt.SaveToken = true;
                    bearerOpt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudiences = appSettings.ValidoEm,
                        ValidIssuer = appSettings.Emissor
                    };
                });
            #endregion
            return services;
        }
    }
}
