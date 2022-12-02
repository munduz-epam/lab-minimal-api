using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class TestController : Controller {
    [HttpGet]
    public string Index() => "from controller";
}