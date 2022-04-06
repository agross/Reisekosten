using Application;

using Infrastructure;

namespace Web;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }

  IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services)
  {
    // Add services to the container.
    services.AddApplication();
    services.AddInfrastructure(Configuration);

    services.AddControllers()
            .AddApplicationPart(GetType().Assembly);
    services.AddSpaStaticFiles(configuration =>
                                 configuration.RootPath =
                                    "reisekosten-web-client/dist");

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    // Configure the HTTP request pipeline.
    if (env.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();

      app.UseDeveloperExceptionPage();
    }

    app.UseRouting();
    app.UseEndpoints(e => e.MapControllers());

    app.UseSpaStaticFiles();

    app.UseSpa(spa =>
    {
      // To learn more about options for serving an Angular SPA from ASP.NET Core,
      // see https://go.microsoft.com/fwlink/?linkid=864501

      spa.Options.SourcePath = "reisekosten-web-client";

      // if (env.IsDevelopment())
      // {
      //   spa.UseProxyToSpaDevelopmentServer("http://localhost:8080");
      // }
    });
  }
}
