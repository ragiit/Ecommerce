using System.IO.Compression;
using Ecommerce.Components;
using Ecommerce.Services;
using Microsoft.AspNetCore.ResponseCompression;

//dotnet publish --configuration Release

using Microsoft.AspNetCore.OutputCaching;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<CartService>();
builder.Services.AddSingleton<ProductAdminService>();

builder.Services.AddServerSideBlazor().AddCircuitOptions(options =>
{
    options.DetailedErrors = builder.Environment.IsDevelopment();
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromSeconds(30);
    options.JSInteropDefaultCallTimeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddWebOptimizer(pipeline =>
{
    pipeline.MinifyJsFiles();
    pipeline.MinifyCssFiles();
});

builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
    [
        "application/javascript",
        "text/css",
        "application/json",
        "application/octet-stream",
        "image/svg+xml"
    ]);
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
});

builder.Services.AddOutputCache(options =>
{
    options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(60)));
    options.AddPolicy("StaticPage", builder => builder.Expire(TimeSpan.FromHours(1)));
    options.AddPolicy("ProductData", builder => builder.Expire(TimeSpan.FromMinutes(5))
                                                    .Tag("products"));
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseHttpsRedirection();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseWebOptimizer();

app.UseStaticFiles();
app.UseResponseCompression();

app.UseAntiforgery();

app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var headers = ctx.Context.Response.Headers;
        var cacheControl = "public,max-age=31536000";
        if (!headers.ContainsKey("Cache-Control"))
        {
            headers.Append("Cache-Control", cacheControl);
        }
        if (!headers.ContainsKey("Expires"))
        {
            headers.Append("Expires", DateTime.UtcNow.AddYears(1).ToString("R"));
        }
    }
});

if (!app.Environment.IsDevelopment())
{
    app.UseOutputCache();
}

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();