using System;

namespace DummyMVCApp.Services
{
    public class GitHubService : IGitHubService
    {
        private readonly IGitHubClient client;

        public GitHubService(IGitHubClient client)
        {
            this.client = client;
        }

        public async Task<int> GetStarsFor(string user)
        {
            return await this.client.GetStars(user);
        }
    }

    public interface IGitHubClient
    {
        Task<int> GetStars(string user);
    }

    public class GitHubClient : IGitHubClient
    {
        public Task<int> GetStars(string user)
        {
            //Imaginemos que esta es la implementacion real del cliente.
            return Task.FromResult(10);
        }
    }
}

