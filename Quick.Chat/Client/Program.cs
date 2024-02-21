using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Quick.Chat.Client;
using Quick.Chat.Client.Managers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Quick.Chat.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
               .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Quick.Chat.Server"));
builder.Services.AddMudServices(c => { c.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight; });
builder.Services.AddApiAuthorization();

builder.Services.AddTransient<IChatManager, ChatManager>();

await builder.Build().RunAsync();
