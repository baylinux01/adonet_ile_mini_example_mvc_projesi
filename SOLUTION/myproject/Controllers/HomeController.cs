using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using myproject.Models;
using myproject.Repositories;
using myproject.Services;
namespace myproject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DatabaseService ds;
    private readonly AppUserService aus;

    public HomeController(ILogger<HomeController> logger,DatabaseService ds,AppUserService aus)
    {
        _logger = logger;
        this.ds=ds;
        this.aus=aus;
    }

    public IActionResult Index()
    {
        ViewModel vm=null;
        String username=HttpContext.Session.GetString("username");
        AppUser appUser=null;
        if(username!=null && !username.Trim().Equals(""))
        {
            vm=aus.GetAppUserByUsername(username);
        }
        return View(vm);
    }

    public IActionResult LogInPage()
    {
        return View();
    }

    public IActionResult SignUpPage()
    {
        return View();
    }
    [HttpPost]
    public IActionResult SignUp(String name, String surname, String username, String password1, String password2 )
    {
        ViewModel vm=new ViewModel();

        Result result=aus.Add(name,surname,username,password1,password2);

        vm.Result=result;

        if(result.Success==true)
        {
            HttpContext.Session.SetString("username",username);
            return View("LogInPage",vm);
        }
        else
        {
            return  View("SignUpPage",vm);
        }
        
    }
    [HttpPost]
    public IActionResult LogIn(String username, String password)
    {
        ViewModel vm=new ViewModel();
        Result result=aus.ControlLogin(username,password);
        if(result.Success==true)
        {
            HttpContext.Session.SetString("username",username);
            return RedirectToAction("Index");
        }
        else
        {
            vm.Result=result;
            return  View("LogInPage",vm);
        }
    }

    public IActionResult LogOut()
    {
        
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
       
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
