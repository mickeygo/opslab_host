using Serilog;
using Ops.Host.App.Extensions;

namespace Ops.Host.App;

public partial class App : Application
{
    private Mutex? _mutex;
    private IHost? _host;

    static IHostBuilder CreateHostBuilder(string[]? args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureServices((builder, services) =>
                {
                    services.AddHttpClient();
                    ConfigureServices(services, builder.Configuration);
                })
                .UseSerilog((hostingContext, loggerConfiguration) =>
                    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration)
                );

    /// <summary>
    /// 获取当前正在使用的 <see cref="App"/> 实例。
    /// </summary>
    public new static App Current => (App)Application.Current;

    /// <summary>
    /// 获取能解析应用服务的 <see cref="IServiceProvider"/> 实例。
    /// </summary>
    public IServiceProvider Services => _host!.Services;

    protected override void OnStartup(StartupEventArgs e)
    {
        // 只允许开启一个
        _mutex = new Mutex(true, "Ops.Host.App", out var createdNew);
        if (!createdNew)
        {
            MessageBox.Show("已有一个程序在运行");
            Environment.Exit(0);
            return;
        }

        base.OnStartup(e);

        try
        {
            Log.Information("应用程序启动");
            _host = CreateHostBuilder(null).Build();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Host terminated unexpectedly");
        }

        // UI线程未捕获异常处理事件
        DispatcherUnhandledException += (s, e) =>
        {
            Log.Error(e.Exception, $"UI线程异常: {e.Exception.Message}");
            // e.Handled = true; // 把 Handled 属性设为true，表示此异常已处理，程序可以继续运行，不会强制退出  
        };

        // Task线程内未捕获异常处理事件
        TaskScheduler.UnobservedTaskException += (s, e) =>
        {
            Log.Error(e.Exception, $"Task线程异常: {e.Exception.Message}");
            // e.SetObserved(); // 设置该异常已察觉（这样处理后就不会引起程序崩溃）
        };

        // 非UI线程未捕获异常处理事件
        AppDomain.CurrentDomain.UnhandledException += (s, e) =>
        {
            if (e.ExceptionObject is Exception ex)
            {
                Log.Error(ex, $"非UI线程异常: {ex.Message}");
            }
            else
            {
                Log.Error($"非UI线程异常: {e.ExceptionObject}");
            }
        };

        _host.Start();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.Information("应用程序关闭退出");
        _host?.Dispose();
        Log.CloseAndFlush();

        base.OnExit(e);
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // options
        services.Configure<OpsHostOptions>(configuration.GetSection("OpsHost"));
        services.Configure<BusinessOptions>(configuration.GetSection("OpsBusiness"));

        // 添加缓存
        services.AddMemoryCache();

        // 添加 Exchange
        services.AddOpsExchange(configuration);

        // 添加
        services.AddHostApp();
    }
}
