using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AuthWebApp.Models;
using AuthWebApp.Services;
using AuthWebApp.DTOs;

namespace AuthWebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AuthApiService _authApiService;

    public HomeController(ILogger<HomeController> logger, AuthApiService authApiService)
    {
        _logger = logger;
        _authApiService = authApiService;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("Token") != null)
        {
            return RedirectToAction("Dashboard");
        }
        return RedirectToAction("Login");
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        try
        {
            var response = await _authApiService.LoginAsync(request);
            HttpContext.Session.SetString("Token", response.Token);
            HttpContext.Session.SetString("Username", response.Username);
            return RedirectToAction("Dashboard");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(request);
        }
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        try
        {
            var response = await _authApiService.RegisterAsync(request);
            HttpContext.Session.SetString("Token", response.Token);
            HttpContext.Session.SetString("Username", response.Username);
            return RedirectToAction("Dashboard");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(request);
        }
    }

    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetString("Token") == null)
        {
            return RedirectToAction("Login");
        }
        ViewBag.Username = HttpContext.Session.GetString("Username");
        return View();
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
