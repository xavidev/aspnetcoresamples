using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DummyMVCApp.Controllers;
using DummyMVCApp.Services;
using IntegrationTesting.Stubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;

namespace IntegrationTesting
{
    /*
     * IClassFixture: Crea una instancia de TFixture antes de construir la clase del test
     * y la inyecta por constructor.
     */
    public class WebApplicationFactoryTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> fixture;

        public WebApplicationFactoryTest(WebApplicationFactory<Program> fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Test_Real_App_With_WebApplicationFactory()
        {
            var client = this.fixture.CreateClient();
            var response = await client.PostAsync("/api/dummy", JsonContent.Create(new DummyModel()));

            Assert.True(!response.IsSuccessStatusCode);
            Assert.True(response.StatusCode == System.Net.HttpStatusCode.BadRequest);
        }


        /*
         * En algunos tests de integracion nos puede interesar mockear
         * alguna de las dependencias.
         * 
         * En este caso, no es bueno que nuestro test dependa de la implementacion
         * real de la API de GitHub. Convertiria nuestro test en un test fragil que
         * ser veria afectado por cualquier cambio en la API de GitHub.
         */
        [Fact]
        public async Task Test_GetStars()
        {
            var customFactory = this.fixture.WithWebHostBuilder(config =>
           {
               config.ConfigureTestServices(services =>
               {
                   services.RemoveAll<IGitHubClient>();
                   services.AddSingleton<IGitHubClient, StubGitHubClient>();
               });
           });

            var client = customFactory.CreateClient();

            var response = await client.GetAsync("/api/dummy/github/stars/xavi");
            response.EnsureSuccessStatusCode();

            Assert.Equal("20", await response.Content.ReadAsStringAsync());
        }
    }
}

