using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Services;
using StudentTimeTrackerApp.Components;
using StudentTimeTrackerApp.Components.Account;
using StudentTimeTrackerApp.Data;
using StudentTimeTrackerApp.Hubs;

using Microsoft.AspNetCore.Http.Connections;


//SignalR, BlazorHub, and .NET 8/9 : https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/signalr?view=aspnetcore-9.0#blazor-hub-options

namespace StudentTimeTrackerApp
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddSignalR(); // Messaging hub support
             
            /*
             *  builder.Services.AddRazorPages(); // added to fix the app.MapFallbackToPage() line
             */


            builder.Services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    ["application/octet-stream"]);
            });

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
            builder.Services.AddTransient<TimeCardService>();
    
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                })
                .AddIdentityCookies();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();


            var app = builder.Build();

            // SignalR response compression
            app.UseResponseCompression();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.UseDeveloperExceptionPage();
            }  
            else
            {
                app.UseExceptionHandler("/Error", createScopeForErrors: true);
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }





            app.UseHttpsRedirection();

            // serve static files (wwwroot)
            app.UseStaticFiles();

            // routing, auth and endpoints
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();



            // Preserve the existing antiforgery call if you have an extension for it
            try
            {
                app.UseAntiforgery();
            }
            catch
            {
                // If that extension is not available in this hosting model, remove or replace it.
            }


            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode()
                // The following method, `.Add(e => {...});`, was added to resolve the following error: ``` Microsoft.AspNetCore.Routing.Matching.AmbiguousMatchException: The request matched multiple endpoints. ```. Sourced from: https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/signalr?view=aspnetcore-9.0#blazor-hub-options
                .Add(e =>
                {
                    var metadata = e.Metadata;
                    var dispatcherOptions = metadata.OfType<HttpConnectionDispatcherOptions>().FirstOrDefault();

                    if (dispatcherOptions != null)
                    {
                        dispatcherOptions.CloseOnAuthenticationExpiration = true;
                    }
                });
            //.AddAdditionalAssemblies(typeof(BlazorSignalRApp.Client._Imports).Assembly);  //https://github.com/dotnet/blazor-samples/blob/main/9.0/BlazorSignalRApp/BlazorSignalRApp/Program.cs 

            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            // Map the SignalR hub using the HubUrl constant from the ChatHub class.
            //app.MapBlazorHub();
            app.UseRouting();

           /* 
            * app.MapFallbackToPage("/_Host");
            */ 
            
            app.MapHub<ChatHub>(ChatHub.HubUrl);

            app.Run();

        }
            





        /*
        public static void Main(string[] args)
        {
            //WebApplication.CreateHostBuilder(args).Build().Run();
            //CreateHostBuilder(args).Build().Run();


            var builder = WebApplication.CreateBuilder(args);

            // Manually instantiate and call the Startup class methods
            var startup = new Startup(builder.Configuration);
            startup.ConfigureServices(builder.Services);

            var app = builder.Build();
            startup.Configure(app, app.Environment);

            app.Run();
        }
        */




        //public static WebApplicationBuilder CreateHostBuilder(string[] args) =>
        //    Host.CreateBuilder(args)
        //        .ConfigureWebHostDefaults(webBuilder =>
        //        {
        //            webBuilder.UseStartup<Startup>();
        //        });
        //}
    }
}

