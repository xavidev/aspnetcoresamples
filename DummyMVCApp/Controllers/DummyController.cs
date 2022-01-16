using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DummyMVCApp.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DummyMVCApp.Controllers
{
    [Route("api/[controller]")]
    public class DummyController : Controller
    {
        private readonly IGitHubService service;

        public DummyController(IGitHubService service)
        {
            this.service = service;
        }

        [HttpPost]
        public IActionResult DoThing(DummyModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest();
            }

            return this.Accepted();
        }

        [Route("github/stars/{user}")]
        public async Task<IActionResult> GetStars(string user)
        {
            var count = await this.service.GetStarsFor(user);

            return this.Ok(count);
        }
    }
}

