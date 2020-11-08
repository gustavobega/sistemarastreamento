using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

//para uso da autenticação
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;
using Rotativa.AspNetCore;

namespace sistemarastreamento
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Obtendo as configurações do projeto

            AppSettings appsettings = new AppSettings();
            //responsavel pela deserialização do AppSettings.json
            var config = new ConfigureFromConfigurationOptions<object>(Configuration.GetSection("AppSettings"));
            config.Configure(appsettings);
            services.AddSingleton<AppSettings>(appsettings);
            System.Environment.SetEnvironmentVariable("MYSQLSTRCOM", appsettings.StringConexaoMySQL);

            #endregion

            #region Serviço para Cookie authorization

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(option =>
            {

                option.Cookie.Name = "CookieAuth";
                option.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Login/index");
                option.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Login/index");
                option.ExpireTimeSpan = TimeSpan.FromHours(appsettings.CookieTempoVida);
            });

            services.AddAuthorization(option =>
            {
                option.AddPolicy("CookieAuth",
                    new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme).
                    RequireAuthenticatedUser().Build());
            });

            #endregion
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers().AddXmlDataContractSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IHostingEnvironment env2)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            RotativaConfiguration.Setup(env2);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "{controller=Login}/{action=Index}/{id?}");
            });
        }
    }
}
