using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModelBase.Base.Filter;
using ModelBase.Base.Logger;
using NpcProxyLink.Base.Server;
using Swashbuckle.AspNetCore.Swagger;

namespace NpcProxyLink
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
            Log.Info("开始启动");

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(
                    options =>
                    {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                    }
                );

            //注册过滤器
            services.AddMvc(options =>
            {
                options.Filters.Add<HttpGlobalExceptionFilter>();
            });
            ;
            services.AddMvcCore().AddAuthorization().AddJsonFormatters();

            services.AddSwaggerGen(c =>
            {
                //配置第一个Doc
                c.SwaggerDoc("v1", new Info { Title = "My API_1", Version = "v1" });

            });

            //数据库初始化
            ServerConfig.Init(Configuration);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            //部署时将其注释掉或者启用授权
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swagger";

            });

            app.UseSwagger();
            app.UseMvc();

            Log.Info("启动成功");
        }
    }
}
