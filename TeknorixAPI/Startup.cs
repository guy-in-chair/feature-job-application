using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TeknorixAPI.Application;
using TeknorixAPI.Application.Abstractions;
using TeknorixAPI.Application.Services;
using TeknorixAPI.Core;
using TeknorixAPI.Filters;
using TeknorixAPI.Helpers;
using TeknorixAPI.Infrastructure;
using TeknorixAPI.Infrastructure.Repositories;
using TeknorixAPI.Logging;
using TeknorixAPI.Middleware;


namespace TeknorixAPI
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
            services.AddControllers(options => 
            { 
                var policy = new AuthorizationPolicyBuilder() 
                .RequireAuthenticatedUser() 
                .Build(); 
                
                options.Filters.Add(new AuthorizeFilter(policy)); 
            });

            //Global Authentication filter
            services.AddMvc(options =>
            {
                options.Filters.Add<TokenValidationFilter>();
            });
            
            //configure CORS policies
            services.AddCors(options => 
            { 
                options.AddDefaultPolicy(builder => 
                { 
#if DEBUG 
                builder.WithOrigins(GlobalHelpers.GetDevModeCorsAllowedClientUrls(Configuration)) 
                    .AllowCredentials() 
                    .AllowAnyHeader() 
                    .AllowAnyMethod(); 
#else          
                builder.WithOrigins(GlobalHelpers.GetReleaseModeCorsAllowedClientUrls(Configuration)) 
                .AllowCredentials() 
                .AllowAnyHeader() 
                .AllowAnyMethod(); 
#endif 
                }); 
            }); 
            
            services.AddAutoMapper(typeof(Startup)); 
            
            //configure custom services
            ServiceInjector.ConfigureServices(services); 
            
            //bind appsettings
            services.Configure<AppSettings>(Configuration); 
            
            services.AddSingleton(Configuration); 
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>(); 
            services.AddSingleton<ILoggerManager, LoggerManager>(); 
            services.AddTransient(typeof(IRepository<>), typeof(Repository<>)); 
            
            services.AddTransient<IJobRepository, JobRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IDepartmentRepository, DepartmentRepository>();

            services.AddDbContextPool<DBContext>(options => options.UseSqlServer( 
                Configuration.GetConnectionString("DBConString"), 
                sqlServerOptionsAction: sqlOptions => 
                {
                    sqlOptions.CommandTimeout(60);
                    sqlOptions.EnableRetryOnFailure( 
                        maxRetryCount: 10, 
                        maxRetryDelay: TimeSpan.FromSeconds(5), 
                        errorNumbersToAdd: null); 
                } 
            )); 
            
            //configure versioning of API
            services.AddApiVersioning(options => 
            { 
                options.ReportApiVersions = true; 
                options.DefaultApiVersion = new ApiVersion(1, 0); 
                options.AssumeDefaultVersionWhenUnspecified = true; 
            }); 
            
            //configure swagger for receiving versioning information.
            services.AddVersionedApiExplorer(options => 
            { 
                options.GroupNameFormat = "'v'VVV"; 
                options.SubstituteApiVersionInUrl = true; 
            }); 
            
            //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>()); 
            
        } 
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider, ILoggerManager logger, IServiceProvider serviceProvider) 
        { 
            if (env.IsDevelopment()) 
            { 
                app.UseDeveloperExceptionPage(); 
            } 
            
            //configure log4net custom logmanager
            app.ConfigureExceptionHandler(logger); 

            app.UseHttpsRedirection(); 
            app.UseRouting(); 
            
            //configuring cors
            app.UseCors(); 
            
            //app.UseIdentityServer();
            app.UseAuthentication(); 
            
            app.UseAuthorization(); 
            
            app.UseEndpoints(endpoints => 
            { 
                endpoints.MapControllers(); 
            }); 
            
            //Configure SwashBuckle and Swagger UI
            
            app.UseSwagger(); 
            app.UseSwaggerUI( 
                options => 
                { 
                    foreach (var description in provider.ApiVersionDescriptions) 
                    { 
#if DEBUG 
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant()); 
#else 
                        options.SwaggerEndpoint($"{Convert.ToString(Configuration["APIAppAliasForSwagger"])}/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant()); 
#endif  
                    } 
                } 
            ); 
        } 
    } 
}
