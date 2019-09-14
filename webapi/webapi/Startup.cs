using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using webapi.core.Interfaces;
using webapi.core.Services;
using webapi.infrastructure.Db;
using webapi.infrastructure.DbObjects;
using webapi.infrastructure.Repositories;

namespace webapi
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
            services.AddMvc();
            services.AddScoped<BookService, BookService>();
            //.AddScoped<IBookRepository, BookRepository>();

            services.AddScoped<IBookRepository, BookMongoDbRepository>();

            services.AddDbContext<MyDb>(options =>
                options.UseMySql(Configuration.GetConnectionString("MyDb")));

            services.AddDbContext<IdentityDb>(options =>
                options.UseMySql(Configuration.GetConnectionString("IdentityDb")));
                
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDb>()
                .AddDefaultTokenProviders();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // if (env.IsDevelopment())
            // {
                app.UseDeveloperExceptionPage();
            //}

            app.UseAuthentication();
            app.UseMvc();

        }
    }
}
