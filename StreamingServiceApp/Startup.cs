using Amazon.DynamoDBv2;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
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

            // Registering the AppDbContext for SQL Server
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Get the current user service
            services.AddHttpContextAccessor();
            services.AddScoped<IUserService, UserService>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/Users/Signin"; // Your login path
                options.LogoutPath = "/Users/Signout"; // Your logout path
            });
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
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Initialize seed data for DynamoDB
            SeedData.InitializeAsync().Wait();
        }
    }
}
