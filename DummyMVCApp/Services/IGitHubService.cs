using System;
namespace DummyMVCApp.Services
{
    public interface IGitHubService
    {
        Task<int> GetStarsFor(string user);
    }
}

