using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using IMDB.Repository.Interfaces;
using IMDB.Repository;
using IMDB.Service;
using IMDBAPI.Services.Interfaces;
using IMDBAPI.Services;
using AutoMapper;
using IMDB.Repository.DBConnection;
using Supabase;
using IMDBAPI.Middlewares;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace IMDBAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IMDBdbContext>(Configuration.GetSection(key:"ConnectionStrings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {
                option.SaveToken = true;
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWTkey:ValidAudience"],
                    ValidIssuer = Configuration["JWTkey:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTkey:Secret"]))
                };
            });
            services.AddControllers().AddNewtonsoftJson();
            services.AddScoped<ErrorHandler>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IActorService, ActorService>();
            services.AddScoped<IProducerService, ProducerService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IActorRepository, ActorRepository>();
            services.AddScoped<IProducerRepository, ProducerRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IReviewsRepository, ReviewsRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<SupabaseStorageService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "IMDBAPI", Version = "v1" });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "IMDBAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ErrorHandler>();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}