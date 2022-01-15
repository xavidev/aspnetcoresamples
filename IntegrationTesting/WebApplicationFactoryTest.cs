using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DummyMVCApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTesting
{
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
    }
}

