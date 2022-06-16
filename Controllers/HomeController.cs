using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // For Session
using LoginRegistration.Models;
using Microsoft.AspNetCore.Identity;

namespace LoginRegistration.Controllers;

public class HomeController : Controller
{
    private MyContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, MyContext context )
    {
        _context = context;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    ///////////////REGISTER//////////////////
    [HttpPost("user/register")]
    public IActionResult Register(User newUser)
    {
        if (ModelState.IsValid) 
        {
            if ( _context.Users.Any(a => a.Email == newUser.Email))
            {
                // Add an error if the email is already in the database
                ModelState.AddModelError("Email", "Email already in use!");
                return View("Index");
            }
            // Hash the password and set the newUser password as the hashed password.
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);

            _context.Add(newUser);
            _context.SaveChanges();

            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            return RedirectToAction("success");
        } else 
        {
            return View("Index");
        }
    }
    ///////////////LOGIN//////////////////
    [HttpPost("user/login")]
    public IActionResult UserLogin(LogUser loginUser)
    {
        if ( ModelState.IsValid )
        {
            User? userInDb = _context.Users.FirstOrDefault(a => a.Email == loginUser.LogEmail);

            PasswordHasher<LogUser> Hasher = new PasswordHasher<LogUser>();
            var result = Hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LogPassword);

            if (userInDb == null || result == 0)
            {
                ModelState.AddModelError("LogEmail", "Invalid login attempt");
                return View("login");
            }

            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return RedirectToAction("success");
        }
        return View("login");
    }
    //////////////LOGIN PAGE///////////////////
    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }
    ///////////////SUCCESS//////////////////
    [HttpGet("success")]
    public IActionResult Success()
    {
        if(HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Index");
        }
        User loggedInUser = _context.Users.FirstOrDefault(a => a.UserId == (int)HttpContext.Session.GetInt32("UserId"));
        return View(loggedInUser);
    }
    //////////////LOGOUT///////////////////
    [HttpGet("logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return View("Login");
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
