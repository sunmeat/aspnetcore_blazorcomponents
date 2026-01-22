using BlazorApp1.Client.Pages; // імпорт простору імен сторінок клієнтської частини
using BlazorApp1.Components; // імпорт простору імен спільних компонентів

namespace BlazorApp1 
{
    public class Program 
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args); // фабрика веб-додатка
            builder.Services.AddRazorComponents() // підтримка Razor-компонентів
                .AddInteractiveServerComponents() // інтерактивний серверний режим (SignalR)
                .AddInteractiveWebAssemblyComponents(); // інтерактивний WebAssembly-режим
            var app = builder.Build();
            if (app.Environment.IsDevelopment()) // якщо режим розробки
            {
                app.UseWebAssemblyDebugging(); // дебагінг WebAssembly
            }
            else // якщо продакшен
            {
                app.UseExceptionHandler("/Error"); // глобальний обробник помилок на /Error
                app.UseHsts(); // Strict-Transport-Security, щоб браузери завжди використовували HTTPS
            }
            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true); // перенаправлення помилкових статус-кодів на /not-found
            app.UseHttpsRedirection(); // перенаправлення http → https
            app.UseAntiforgery(); // захист від CSRF-атак
            app.MapStaticAssets(); // статичні файли з wwwroot
            app.MapRazorComponents<App>() // реєстрація кореневого компонента App.razor
                .AddInteractiveServerRenderMode() // інтерактивний Server-рендеринг
                .AddInteractiveWebAssemblyRenderMode() // інтерактивний WebAssembly-рендеринг, корисно для гібридних додатків
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly); // додається клієнтська збірка до пошуку компонентів
            app.Run();
        }
    }
}