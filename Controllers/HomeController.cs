using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EasyMartApp.Models;
using EasyMartApp.Services;

namespace EasyMartApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PromoService promoService;

    public HomeController(ILogger<HomeController> logger, PromoService promoService)
    {
        _logger = logger;
        this.promoService = promoService;
    }

    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("id") == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        int userId = Convert.ToInt32(HttpContext.Session.GetString("id"));
        var dashboard = promoService.GetDashboard(userId);

        ViewData["title"] = "Dashboard";
        return View(dashboard);
    }

    public IActionResult PromoCodes()
    {
        if (HttpContext.Session.GetString("id") == null)
        {
            return RedirectToAction("Login", "Auth");
        }


        int userId = Convert.ToInt32(HttpContext.Session.GetString("id"));
        var userPromoCodes = promoService.GetAllUserPromoCodes(userId);


        ViewData["title"] = "Promo Codes";
        return View(userPromoCodes);
    }

    [HttpPost]
    public IActionResult PromoCodes([FromForm]string code)
    {
        if (HttpContext.Session.GetString("id") == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        int userId = Convert.ToInt32(HttpContext.Session.GetString("id"));
        if (string.IsNullOrEmpty(code))
        {
            ViewData["err"] = "Code is required!";
        }
        else
        {
            bool isClaimed = promoService.ClaimCode(userId, code);
            if (!isClaimed)
            {
                ViewData["err"] = "Invalid code!";
            }
            else
            {
                ViewData["suc"] = "Code claimed successfully!";
            }
        }


        var userPromoCodes = promoService.GetAllUserPromoCodes(userId);

        ViewData["title"] = "Promo Codes";

        return View(userPromoCodes);
    }

    [HttpPost]
    public  IActionResult RedeemItem([FromQuery]int? userId, [FromQuery]int? itemId){
        if(userId==null || itemId==null){
            return BadRequest(new {message="Item id/user id is required!"});
        }

        promoService.RedeemItem(userId.Value, itemId.Value);

        TempData["suc"] = "Item redeemed successfully!";
        return Ok(new {message="Item redeemed successfully!"});
    }

     public IActionResult RedeemedItems()
    {
        if (HttpContext.Session.GetString("id") == null)
        {
            return RedirectToAction("Login", "Auth");
        }

        int userId = Convert.ToInt32(HttpContext.Session.GetString("id"));
        var redeemedItems = promoService.GetUserRedeemedItems(userId);

        ViewData["title"] = "Redeemed Items";
        return View(redeemedItems);
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
