using Microsoft.AspNetCore.Mvc;
using IActionResultExample.Models;

namespace IActionResultExample.Controllers
{
    public class ReportController : Controller
    {

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