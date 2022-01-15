using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DummyMVCApp.Controllers;
using DummyMVCApp.CustomMiddleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace IntegrationTesting;

public class TestServer
{
    /*
     * Utilizar TestServer para porbar componenetes aislados de la infrastructura,
     * por ejemplo los middleware.
     * 
     * El servidor en memoria (TestServer) se podria utilizar par probar tu app "real"
     * pero se necesitaria mucha mas configuracion. Para ello usar WebApplicationFactory.
     */

    [Fact]
    public async Task Integration_test_with_test_server_for_testing_Middlewares()
    {
        var builder = new HostBuilder()
            .ConfigureWebHost(configure =>
            {
                configure.Configure(app =>
                    app.UseMiddleware<DummyMiddelware>()
                );
                configure.UseTestServer();
            });

        IHost host = await builder.StartAsync();
        var client = host.GetTestClient();

        var response = await client.GetAsync("/ping");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();

        Assert.Equal("pong", content);
    }
}
