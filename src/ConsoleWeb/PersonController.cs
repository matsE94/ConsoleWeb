using Microsoft.AspNetCore.Mvc;

namespace ConsoleWeb
{
    public class PersonController : ControllerBase
    {
        public IActionResult Get()
        {
            return new JsonResult(new
            {
                Name = "David Fowler",
                Age = 42,
                Height = 69
            });
        }
    }
}