using HeadlessWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
    })
    //.UseSystemd() Para usarlo como un daemond de Linux
    .UseWindowsService() //Para usarlo como un servicio de windows
    .Build();

await host.RunAsync();


//sc create "My Test Service" BinPath="C:\path\to\MyService.exe" Ejemplo para instalar el servicio.