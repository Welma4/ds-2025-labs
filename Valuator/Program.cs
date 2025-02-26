using StackExchange.Redis;
using Valuator.ViewModel;
using Valuator.Pages;
using Valuator.Services;
namespace Valuator;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();
        builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            ConnectionMultiplexer.Connect(("127.0.0.1:6379")));
        builder.Services.AddScoped<RedisStorage>();
        builder.Services.AddTransient<TextAnalyseService>();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
        }
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapRazorPages();

        app.Run();
    }
}