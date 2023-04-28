using Microsoft.AspNetCore.Mvc;
using IActionResultExample.Models;
using System.Text.Json;

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

            //收集總計資料
            double? reportAllqty = 0;
            double? reportAllcost = 0;
            var reportMostqtyPartner = "";
            double? reportMostqty = 0;
            double? reportMostqtyPersent = 0;
            var reportMostcostPartner = "";
            double? reportMostcost = 0;
            double? reportMostcostPersent = 0;


            // return Content($"<h1>test {result}<h1>", "text/html");


            // 使用這個模型 保存資料
            // 在list沒有這個partner的時候新建
            List<partnerTotal> anypartner = new List<partnerTotal>();
            foreach (var i in result){
                if (anypartner.Exists(x => x.Partner == i.partner)){
                    // 如果已經存在 把花費跟件數加上去
                    var model = anypartner.Where(y => y.Partner == i.partner).First();
                    model.PdCost += i.cost;
                    model.PdQty += i.qty;
                    // 加入總計資料
                    reportAllcost += i.cost;
                    reportAllqty += i.qty;
                }else {
                    // 宣告新的並加入
                    partnerTotal partner = new partnerTotal();
                    partner.Partner = i.partner;
                    partner.PdCost = i.cost;
                    partner.PdQty = i.qty;
                    anypartner.Add(partner);
                    // 加入總計資料
                    reportAllcost += i.cost;
                    reportAllqty += i.qty;
                }}
            
            // 計算完partner資料, 找最大值
            foreach(var i in anypartner){
                if (reportMostcost < i.PdCost){
                    reportMostcost = i.PdCost;
                    reportMostcostPartner = i.Partner;
                }
                if (reportMostqty < i.PdQty){
                    reportMostqty = i.PdQty;
                    reportMostqtyPartner = i.Partner;
                }
            }
            reportMostcostPersent = reportMostcost / reportAllcost;
            reportMostqtyPersent = reportMostqty / reportAllqty;

            // 傳送json
            var jsonanypartner = JsonSerializer.Serialize(anypartner);
            //jsonanypartner = jsonanypartner.Replace("\"","'");

            //傳送ViewData資料
            ViewData["thisreport"] = setting.Type;
            ViewData["reportstartdate"] = setting.StartDate;
            ViewData["reportenddate"] = setting.EndDate;
            ViewData["reportallcost"] = reportAllcost;
            ViewData["reportallqty"] = reportAllqty;
            ViewData["reportmostcostpartner"] = reportMostcostPartner;
            ViewData["reportmostcostpresent"] = reportMostcostPersent;
            ViewData["reportmostqtypartner"] = reportMostqtyPartner;
            ViewData["reportmostqtypresent"] = reportMostqtyPersent;
            ViewData["reportmostqty"] = reportMostqty;
            ViewData["reportmostcost"] = reportMostcost;

            ViewData["jsonArray"] = jsonanypartner;

            return View(result);
        }
    }

}