using HashidsNet;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RpgCombat.Domain;
using RpgCombat.Infrastructure;
using RpgCombat.Infrastructure.EntityFramework;

namespace RpgCombat.WebApi
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
            services.AddControllers();
            services.AddMediatR(typeof(Application.Placeholder).Assembly);
            services.AddSingleton<IHashids>(new Hashids(salt: Configuration["Salt"], minHashLength: 8));
            services.AddScoped<IRpgCharacterRepository, RpgCharacterRepository>();
            services.AddDbContext<RpgCombatKataDbContext>(optionsBuilder =>
                optionsBuilder.UseSqlite(Configuration["ConnectionString"]));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RpgCombatKataDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dbContext.Database.EnsureDeleted();
            }

            dbContext.Database.EnsureCreated();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
