using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class ReportController : Controller
    {

        [Route("ReportInput/{type}")]
        //Url: /Report?type=Sale&startDate=2023/4/4%endDate=2023/4/5
        public IActionResult ReportData(string type, string startDate, string endDate)
        {
            return Content($"<h1>test {type}<h1>", "text/html");
        }
    }
}