# BlazorApp1: Blazor Components

Навчальний приклад на **ASP.NET Core Blazor**, присвячений створенню, композиції та життєвому циклу Razor-компонентів.

Проєкт демонструє, як побудувати Blazor-застосунок із серверним хостом та окремою WebAssembly-збіркою клієнта, як передавати параметри між компонентами, реагувати на події та виконувати асинхронну ініціалізацію.

## Можливості

- компонентний підхід Blazor із Razor-компонентами;
- окремі серверний та клієнтський проєкти;
- інтерактивні режими **Interactive Server** та **Interactive WebAssembly**;
- `InteractiveAuto` для автоматичного вибору режиму;
- маршрутизація через `Router`;
- передавання даних через `[Parameter]`;
- обробка подій через `@onclick`;
- демонстрація життєвого циклу компонентів;
- асинхронна ініціалізація через `OnInitializedAsync`;
- умовний рендеринг і відображення стану завантаження;
- компонентна композиція;
- локальні CSS-стилі компонентів;
- Bootstrap;
- стандартна обробка помилок і перепідключення Blazor.

## Технології

| Технологія | Призначення |
|---|---|
| C# | компонентна та серверна логіка |
| .NET 10 | платформа виконання |
| ASP.NET Core | серверний хостинг |
| Blazor | інтерактивний веб-інтерфейс |
| Razor Components | побудова UI |
| WebAssembly | клієнтське виконання |
| SignalR | інтерактивність Server Blazor |
| Bootstrap | базові стилі |
| CSS | оформлення компонентів |

## Структура рішення

```text
aspnetcore_blazorcomponents/
├── BlazorApp1.slnx
│
├── BlazorApp1/
│   ├── Components/
│   │   ├── App.razor
│   │   ├── _Imports.razor
│   │   └── Pages/
│   │       ├── Home.razor
│   │       └── Error.razor
│   │
│   ├── Program.cs
│   ├── BlazorApp1.csproj
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── wwwroot/
│
├── BlazorApp1.Client/
│   ├── Layout/
│   │   ├── MainLayout.razor
│   │   ├── MainLayout.razor.css
│   │   ├── NavMenu.razor
│   │   ├── NavMenu.razor.css
│   │   ├── ReconnectModal.razor
│   │   ├── ReconnectModal.razor.css
│   │   └── ReconnectModal.razor.js
│   │
│   ├── Pages/
│   │   ├── Home.razor
│   │   ├── BusinessCard.razor
│   │   ├── ContactInfo.razor
│   │   ├── HeaderPhoto.razor
│   │   ├── SocialLinks.razor
│   │   ├── FooterNote.razor
│   │   ├── Counter.razor
│   │   ├── Weather.razor
│   │   └── NotFound.razor
│   │
│   ├── Program.cs
│   ├── Routes.razor
│   ├── _Imports.razor
│   └── BlazorApp1.Client.csproj
│
└── LICENSE.txt
```

## Архітектура

Рішення складається з двох основних проєктів.

### BlazorApp1

Серверний ASP.NET Core-проєкт. Він запускає веб-застосунок, налаштовує Razor Components, middleware, статичні ресурси та підключення клієнтської збірки.

У `Program.cs` реєструються інтерактивні компоненти:

```csharp
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();
```

Під час побудови endpoint-ів підключаються обидва режими:

```csharp
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);
```

Таким чином сервер знає про компоненти, розташовані в `BlazorApp1.Client`.

### BlazorApp1.Client

Клієнтський проєкт на `Microsoft.NET.Sdk.BlazorWebAssembly`.

Він містить Razor-компоненти, сторінки, layout і маршрутизацію. Клієнтський `Program.cs` створює `WebAssemblyHostBuilder` та запускає WebAssembly-застосунок.

## Режими інтерактивності

Проєкт підтримує кілька режимів інтерактивного рендерингу Blazor.

- **Interactive Server** — виконує компонентну логіку на сервері та забезпечує інтерактивність через SignalR.
- **Interactive WebAssembly** — виконує компонентну логіку в браузері через WebAssembly.
- **Interactive Auto** — дозволяє застосунку використовувати серверний або WebAssembly-режим залежно від доступності клієнтського середовища.

У `App.razor` для маршрутизації та `HeadOutlet` використовується `InteractiveAuto`:

```razor
<HeadOutlet @rendermode="InteractiveAuto" />
```

```razor
<Routes @rendermode="InteractiveAuto" />
```

## Компонентна композиція

Головна сторінка демонструє персональну картку, побудовану з кількох менших компонентів:

```text
BusinessCard
├── HeaderPhoto
├── ContactInfo
├── SocialLinks
└── FooterNote
```

`Home.razor` передає дані до `BusinessCard`:

```razor
<BusinessCard Name="Олександр Загоруйко"
              Position="Full-stack розробник .NET / Blazor"
              Location="місто Одеса" />
```

`BusinessCard` своєю чергою використовує дочірні компоненти для окремих частин інтерфейсу.

Такий підхід робить UI зрозумілішим, спрощує підтримку та дозволяє повторно використовувати компоненти.

## Параметри компонентів

Blazor-компоненти можуть приймати значення через властивості з атрибутом `[Parameter]`.

Наприклад:

```csharp
[Parameter] public string Name { get; set; } = "Невідомо";
[Parameter] public string Position { get; set; } = "";
[Parameter] public string Location { get; set; } = "";
```

Компонент можна використовувати декларативно:

```razor
<BusinessCard Name="Олександр Загоруйко"
              Position="Full-stack розробник .NET / Blazor"
              Location="місто Одеса" />
```

Це одна з фундаментальних ідей Blazor: інтерфейс складається з компонентів, які взаємодіють через параметри, події та стан.

## Життєвий цикл компонентів

`BusinessCard.razor` демонструє кілька етапів життєвого циклу:

```csharp
protected override void OnInitialized()
{
    Console.WriteLine("BusinessCard > OnInitialized");
}

protected override async Task OnInitializedAsync()
{
    await Task.Delay(300);
    Console.WriteLine("BusinessCard > OnInitializedAsync завершено");
}

protected override void OnParametersSet()
{
    Console.WriteLine($"Оновилися параметри: {Name}");
}
```

У прикладі можна побачити:

- синхронну ініціалізацію;
- асинхронну ініціалізацію;
- реакцію на встановлення параметрів;
- виконання логіки після створення компонента.

`OnInitializedAsync` використовує невелику затримку для імітації асинхронної операції (наприклад, завантаження аватарки).

## Counter

Сторінка `/counter` демонструє найпростіший інтерактивний компонент Blazor.

Стан зберігається у полі:

```csharp
private int currentCount = 0;
```

Обробник події змінює стан:

```csharp
private void IncrementCount()
{
    currentCount++;
}
```

Кнопка підключає обробник:

```razor
<button class="btn btn-primary" @onclick="IncrementCount">
    Click me
</button>
```

Після зміни стану Blazor автоматично оновлює відповідну частину UI.

## Weather

Сторінка `/weather` демонструє асинхронне завантаження даних та умовний рендеринг.

До завершення завантаження показується повідомлення:

```razor
@if (forecasts == null)
{
    <p><em>Завантаження...</em></p>
}
```

Після завершення `OnInitializedAsync` створюється масив із п'яти прогнозів.

У цьому прикладі немає зовнішнього API. Дані генеруються локально, а `Task.Delay` використовується для імітації асинхронної операції.

## Маршрутизація

Маршрут сторінки визначається директивою `@page`.

Наприклад:

```razor
@page "/counter"
```

`Routes.razor` використовує `Router` для пошуку компонента, який відповідає поточному URL:

```razor
<Router AppAssembly="typeof(Program).Assembly"
        NotFoundPage="typeof(Pages.NotFound)">
    <Found Context="routeData">
        <RouteView RouteData="routeData"
                   DefaultLayout="typeof(Layout.MainLayout)" />
        <FocusOnNavigate RouteData="routeData" Selector="h1" />
    </Found>
</Router>
```

Для знайденої сторінки застосовується `MainLayout`, а для невідомого маршруту передбачена сторінка `NotFound`.

## Layout

`MainLayout.razor` визначає загальну структуру сторінок застосунку.

Він містить:

- бічну навігацію (`NavMenu`);
- верхню панель;
- область для поточного компонента через `@Body`;
- UI для відображення неперехопленої помилки Blazor;
- `ReconnectModal` для відновлення з'єднання.

Ключовий елемент layout:

```razor
<article class="content px-4">
    @Body
</article>
```

`@Body` є місцем, у яке Blazor вставляє поточну сторінку.

## Обробка помилок та безпека

Серверний застосунок використовує стандартний pipeline ASP.NET Core:

```csharp
app.UseHttpsRedirection();
app.UseAntiforgery();
app.MapStaticAssets();
```

У production-режимі також активуються:

- глобальний обробник винятків (`UseExceptionHandler("/Error")`);
- HSTS;
- обробка помилкових статус-кодів із перенаправленням на сторінку `NotFound`.

У development-режимі доступний WebAssembly debugging.

## Конфігурація

Базові налаштування знаходяться в `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

## Вимоги

Для запуску потрібен:

- **.NET 10 SDK**
- IDE або редактор із підтримкою C# та Razor (Visual Studio, JetBrains Rider, VS Code)

Перевірити версію SDK:

```bash
dotnet --version
```

## Запуск

Клонуйте репозиторій:

```bash
git clone https://github.com/sunmeat/aspnetcore_blazorcomponents.git
cd aspnetcore_blazorcomponents
```

Відновіть залежності:

```bash
dotnet restore
```

Запустіть серверний проєкт:

```bash
dotnet run --project BlazorApp1
```

Після запуску відкрийте адресу, яку ASP.NET Core виведе в консолі (зазвичай `https://localhost:7xxx`).

Також рішення можна відкрити у Visual Studio або JetBrains Rider через файл `BlazorApp1.slnx`.

## Корисні маршрути

| URL | Призначення |
|---|---|
| `/` | головна сторінка з прикладом композиції компонентів |
| `/counter` | інтерактивний лічильник |
| `/weather` | асинхронне завантаження та відображення даних |

## Навчальна мета

Проєкт призначений насамперед для вивчення **Blazor Components** і може використовуватися як невеликий практичний приклад перед переходом до більших ASP.NET Core Blazor-застосунків.

На його прикладі можна послідовно розібрати:

1. структуру Blazor-рішення;
2. Razor Components;
3. параметри компонентів;
4. композицію компонентів;
5. події та зміну стану;
6. життєвий цикл компонентів;
7. асинхронну ініціалізацію;
8. маршрутизацію;
9. layout;
10. Server та WebAssembly rendering modes.

## Ліцензія

Проєкт поширюється відповідно до умов ліцензії, зазначеної у [LICENSE.txt](LICENSE.txt).

## Автор

**Олександр Загоруйко**

- GitHub: [github.com/sunmeat](https://github.com/sunmeat)
- LinkedIn: [linkedin.com/in/sunmeat](https://linkedin.com/in/sunmeat)
- Telegram: [t.me/sunmeat](https://t.me/sunmeat)
