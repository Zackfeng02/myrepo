using Amazon.DynamoDBv2;
using StreamingServiceApp.DbData;

namespace StreamingServiceApp
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.ConsentCookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            // Registering the DynamoDB repositories
            services.AddTransient<IMovieRepository, DynamoDBMovieRepository>();
            services.AddTransient<IUserRepository, DynamoDBUserRepository>();
            services.AddTransient<IReviewRepository, DynamoDBReviewRepository>();
            services.AddTransient<MovieReviewService>();


            // Registering the DynamoDB client as a Singleton
            var connection = new Connection();
            var dynamoDbClient = connection.Connect();
            services.AddSingleton<IAmazonDynamoDB>(dynamoDbClient);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedData.InitializeAsync().Wait();
        }
    }
}
