using BusBoard.Api;
using BusBoard.Api.JSON_Classes;
using Microsoft.AspNetCore.Mvc;
using BusBoard.Web.Models;
using BusBoard.Web.ViewModels;

namespace BusBoard.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult IndexRetry()
    {
        return View();
    }

    [HttpGet]
    public async Task<ActionResult> BusInfo(PostcodeSelection selection)
    {
        // Add some properties to the BusInfo view model with the data you want to render on the page.
        // Write code here to populate the view model with info from the APIs.
        // Then modify the view (in Views/Home/BusInfo.cshtml) to render upcoming buses.
        if (!int.TryParse(selection.StopCount,out int cnt))
        {
            return RedirectToAction(nameof(IndexRetry));
        }

        var latlong = (await APIHandler.Post2LatLong(selection.Postcode));
        if (!latlong.ContainsKey("lat"))
        {
            return RedirectToAction(nameof(IndexRetry));
        }
        List<Stop> stops = new List<Stop>();
        for (int i = 0; i < cnt; i++)
        {
            var stopDic = (await APIHandler.LatLongToStop(latlong["lat"], latlong["long"], i));
            var departures = await APIHandler.AtcocodeToBusDepart(stopDic["atcocode"]);
            {
                if (!departures.ContainsKey("all"))
                {
                    Stop stop = new Stop(stopDic["name"], stopDic["distance"],stopDic["atcocode"], new List<Bus>());
                    stops.Add(stop);
                }
                else
                {
                    var dep = departures["all"];
                    Stop stop = new Stop(stopDic["name"], stopDic["distance"],stopDic["atcocode"], dep);
                    stops.Add(stop);

                }
            }
        }
        var info = new BusInfo(selection.Postcode, stops);
        return View(info);
    }

    public ActionResult About()
    {
        ViewBag.Message = "Information about this site";

        return View();
    }

    public ActionResult Contact()
    {
        ViewBag.Message = "Contact us!";

        return View();
    }
}
