using RecipeProject.Models.Settings;

namespace RecipeProject.web
{
    public class Startup
    {
         public IConfiguration Configuration { get; }
        public Startup(IConfiguration config)
        {
            
            Configuration = config;

        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CloudinaryOptions>(Configuration.GetSection("CloudinaryOptions"));

            
                
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            



            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            }
            else
            {
                app.UseCors();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}
