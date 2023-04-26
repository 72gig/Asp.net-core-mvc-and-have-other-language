using Microsoft.AspNetCore.Mvc;
using IActionResultExample.Models;

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

        [Route("ReportInput/{type?}/{startdate?}/{enddate?}")]
        //Url: http://localhost:5267/ReportInput?type=purchase&startdate=2023-04-11&enddate=2023-04-27
        public IActionResult ReportData([FromQuery]string? type,
                                        [FromQuery]string? startdate,
                                        [FromQuery]string? enddate,
                                        Readsql setting)
        {
            var qrerydata = Request.QueryString;

            // 傳到Model 分析
            sqlUser data = new sqlUser();
            var result =  data.selectSql(setting);

            return Content($"<h1>test {result}<h1>", "text/html");
        }
    }

}