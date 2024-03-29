using EasyMartApp.Services;
using EasyMartApp.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace EasyMartApp.Controllers;
public class AuthController : Controller
{
    private readonly PromoService promoService;

    public AuthController(PromoService promoService)
    {
        this.promoService = promoService;
    }

    public IActionResult Login()
    {
        if (HttpContext.Session.GetString("id") != null)
        {
            return RedirectToAction("Index", "Home");
        }
        ViewData["title"] = "Login";
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginModel model)
    {
        ViewData["title"] = "Login";
        if (model == null || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
        {
            ViewData["err"] = "Email and password is required!";
            return View();
        }
        else
        {
            var _user = promoService.GetUser(model.Email, model.Password);
            if (_user == null)
            {
                ViewData["err"] = "Invalid username or password!";
                return View();
            }else if(!_user.IsVerified){
                return RedirectToAction("VerifyPhone", new {Controller = "Auth", Phone=_user.Phone, UserId = _user.Id});
            }
            else
            {
                HttpContext.Session.SetString("id", _user.Id.ToString());
                HttpContext.Session.SetString("fname", _user.FirstName);
                HttpContext.Session.SetString("lname", _user.LastName);
                HttpContext.Session.SetString("email", _user.Email);

                return RedirectToAction("Index", "Home");
            }
        }
    }

    public IActionResult Signup()
    {
        if (HttpContext.Session.GetString("id") != null)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["title"] = "Sign Up";
        return View();
    }

    [HttpPost]
    public IActionResult Signup(RegisterModel model)
    {
        ViewData["title"] = "Sign Up";

        if (model == null
        || string.IsNullOrEmpty(model.FirstName)
        || string.IsNullOrEmpty(model.LastName)
        || string.IsNullOrEmpty(model.Email)
        || string.IsNullOrEmpty(model.Phone)
        || string.IsNullOrEmpty(model.Password))
        {
            ViewData["err"] = "All fields are required!";
            return View();
        }
        else
        {
            var _user = promoService.GetUser(model.Email);
            if (_user != null)
            {
                ViewData["err"] = "User with specified email already exist!";
                return View();
            }
            else
            {
                _user = promoService.RegisterUser(new Models.User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    Phone = model.Phone
                });

                //TempData["suc"] = "Registration successful!";
                return RedirectToAction("VerifyEmail", new {Controller = "Auth", Email=model.Email, UserId = _user.Id});
            }
        }
    }

    public async Task<IActionResult> VerifyEmail([FromQuery] int? userId, [FromQuery] string? email)
    {
        ViewData["title"] = "Verify Email";

        if (userId == null || email == null)
        {
            return RedirectToAction("Login");
        }

        int maskCount = email.Length / 2;
        int offset = maskCount / 2;
        string maskedEmail = email.Remove(offset, maskCount).Insert(offset, new string('X', maskCount));
        string expectedCode = promoService.GenerateCode(6);
        await promoService.SendMail(expectedCode, email);

        var model = new VerifyEmailModel{
            UserId = userId.Value,
            Email = email,
            MaskedEmail = maskedEmail,
            ExpectedCode = expectedCode
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult VerifyEmail(VerifyEmailModel model)
    {
        ViewData["title"] = "Verify Email";

        if(string.IsNullOrEmpty(model.Code)){
            ViewData["err"] = "Code is required!";
        }else if(!model.Code.Equals(model.ExpectedCode)){
            ViewData["err"] = "Invalid code!";
        }else{
            promoService.VerifyUser(model.UserId);
             TempData["suc"] = "Registration successful!";
                return RedirectToAction("Login");
        }

        return View(model);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return RedirectToAction("Login", "Auth");
    }
}
