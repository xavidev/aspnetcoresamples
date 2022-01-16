using System;
using System.Threading.Tasks;
using DummyMVCApp.Services;

namespace IntegrationTesting.Stubs
{
    public class StubGitHubClient : IGitHubClient
    {
        public Task<int> GetStars(string user)
        {
            return Task.FromResult(20);
        }
    }
}

