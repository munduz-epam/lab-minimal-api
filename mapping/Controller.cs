using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TestController : Controller {
    [HttpGet]
    public IActionResult Index() => Content("from controller");
}