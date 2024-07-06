using Quick.Chat.Client.Managers;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MudBlazor;
using MudBlazor.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace Quick.Chat.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddHttpClient("Quick.Chat.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
                .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Quick.Chat.ServerAPI"));
            builder.Services.AddMudServices(c => { c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight; });
            builder.Services.AddApiAuthorization();

            builder.Services.Configure<AntiforgeryOptions>(config =>
            {
                config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            builder.Services.Configure<CookieTempDataProviderOptions>(config =>
            {
                config.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            });

            //builder.Services.AddEmojiPicker();
            builder.Services.AddTransient<IChatManager, ChatManager>();
            await builder.Build().RunAsync();
        }
    }
}
