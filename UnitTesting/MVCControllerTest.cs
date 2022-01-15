using DummyMVCApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace UnitTesting;

public class MVCControllerTest
{
    [Fact]
    public void DoThing_Should_Return_BadRequest_When_Model_Is_Invalid()
    {
        var controller = new DummyController();
        var model = new DummyModel();
        controller.ModelState.AddModelError(nameof(model.Id),
            "Dummy model must have an Id");

        IActionResult result = controller.DoThing(model);

        Assert.IsType<BadRequestResult>(result);
    }
}
