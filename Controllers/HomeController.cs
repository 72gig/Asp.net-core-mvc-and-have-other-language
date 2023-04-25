using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("home")]
        public IActionResult Index()
        {
            string type = new string("test");
            return View();
            //return RedirectToAction("ReporrData", "Report", new { type =  "test"});
            //return RedirectToActionPermanent("ReportData", "Report", new { type = "test" });  // 這個寫法可以轉移到控制器叫做ReportController 方法叫做 ReportData 的Route

            //return new LocalRedirectResult($"ReportInput/{type}"); //應可連到其它外部網站

            //return RedirectPermanent($"ReportInput/{type}");
        }

    }

}