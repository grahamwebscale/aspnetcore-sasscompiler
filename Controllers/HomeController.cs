using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using aspnetcoresasscompiler.Models;
using System.Text;
using AspNetCore.SassCompiler;
using System.Drawing;

namespace aspnetcoresasscompiler.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ISassCompiler _sassCompiler;

    public HomeController(ILogger<HomeController> logger, ISassCompiler sassCompiler)
    {
        _logger = logger;
        _sassCompiler = sassCompiler;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public async Task<ActionResult> BrandedTheme() {
    var sass = @"$navbar-bg: #ffffff;$brand-header-color: #ffffff;body { background-color: darken($navbar-bg, 10%); color: lighten($brand-header-color, 10%) }.btn { color: lighten($brand-header-color, 10%)}.test1 { background-color: darken($navbar-bg, 10%); color: lighten($brand-header-color, 10%) }";
    Console.WriteLine($"sass: {sass}");
    using var ms = new MemoryStream(Encoding.UTF8.GetBytes(sass));
    // error path - causes hang/deadlock
    var sassResult = await _sassCompiler.CompileToStringAsync(ms, []);
    // success path - passes "-q" arg to turn off warning output
    //var sassResult = await _sassCompiler.CompileToStringAsync(ms, ["-q"]);
    
    _logger.LogInformation("generated css. Length {length}. Result: {result}", sassResult.Length, sassResult);
    return Content(sassResult, "text/css");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
