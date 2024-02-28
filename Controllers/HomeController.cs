using Microsoft.AspNetCore.Mvc;
using IActionResultExample.Models;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using System.Diagnostics;
using System.IO;

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
            double? productAllqty = 0;
            double? productAllcost = 0;
            var productMostqtyproduct = "";
            var productMostqtyproductcode = "";
            double? productMostqty = 0;
            double? productMostqtyPersent = 0;
            var productMostcostproduct = "";
            var productMostcostproductcode = "";
            double? productMostcost = 0;
            double? productMostcostPersent = 0;


            // return Content($"<h1>test {result}<h1>", "text/html");


            // 使用這個模型 保存資料
            // 在list沒有這個partner的時候新建
            List<partnerTotal> anypartner = new List<partnerTotal>();
            foreach (var i in result){
                if (anypartner.Exists(x => x.Partner == i.partner)){
                    // 如果已經存在 把花費跟件數加上去
                    var model = anypartner.Where(y => y.Partner == i.partner).First();
                    model.PdCost += i.cost*i.qty;
                    model.PdQty += i.qty;
                    // 加入總計資料
                    reportAllcost += i.cost*i.qty;
                    reportAllqty += i.qty;
                }else {
                    // 宣告新的並加入
                    partnerTotal partner = new partnerTotal();
                    partner.Partner = i.partner;
                    partner.PdCost = i.cost*i.qty;
                    partner.PdQty = i.qty;
                    anypartner.Add(partner);
                    // 加入總計資料
                    reportAllcost += i.cost*i.qty;
                    reportAllqty += i.qty;
                }}
            

            // 在list沒有這個product的時候新建
            List<productTotal> anyproduct = new List<productTotal>();
            foreach (var i in result){
                if (anyproduct.Exists(x => x.ProductCode == i.productcode)){
                    // 如果已經存在 把花費跟件數加上去
                    var model = anyproduct.Where(y => y.ProductCode == i.productcode).First();
                    model.PdCost += i.cost*i.qty;
                    model.PdQty += i.qty;
                    // 加入總計資料
                    productAllcost += i.cost*i.qty;
                    productAllqty += i.qty;
                }else {
                    // 宣告新的並加入
                    productTotal product = new productTotal();
                    product.Product = i.product;
                    product.ProductCode = i.productcode;
                    product.PdCost = i.cost*i.qty;
                    product.PdQty = i.qty;
                    anyproduct.Add(product);
                    // 加入總計資料
                    productAllcost += i.cost*i.qty;
                    productAllqty += i.qty;
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

            // 計算完product資料, 找最大值
            foreach(var i in anyproduct){
                if (productMostcost < i.PdCost){
                    productMostcost = i.PdCost;
                    productMostcostproduct = i.Product;
                    productMostcostproductcode = i.ProductCode;
                }
                if (productMostqty < i.PdQty){
                    productMostqty = i.PdQty;
                    productMostqtyproduct = i.Product;
                    productMostqtyproductcode = i.ProductCode;
                }
            }
            productMostcostPersent = productMostcost / productAllcost;
            productMostqtyPersent = productMostqty / productAllqty;

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
            ViewData["partnercount"] = anypartner.Count;
            
            ViewData["productallqty"] = productAllqty;
            ViewData["productallcost"] = productAllcost;
            ViewData["productmostqtyproduct"] = productMostqtyproduct;
            ViewData["productmostqtyproductcode"] = productMostqtyproductcode;
            ViewData["productmostqty"] = productMostqty;
            ViewData["productmostqtypresent"] = productMostqtyPersent;
            ViewData["productmostcostproduct"] = productMostcostproduct;
            ViewData["productmostcostproductcode"] = productMostcostproductcode;
            ViewData["productmostcost"] = productMostcost;
            ViewData["productmostcostpresent"] = productMostcostPersent;

            ViewData["jsonArray"] = jsonanypartner;

            return View(result);
        }


        [Route("webApi/createCsv")]
        [HttpPost]
        public string createCsv(string type, string startDate, string endDate,
                                double allCost, double allQty, double mostCost, double mostQty,
                                sqlResult data){

            string[] values = new string[7];
            string Arguments = @"create_csv.py";
            values[0] = type;
            values[1] = startDate;
            values[2] = endDate;
            values[3] = allCost.ToString();
            values[4] = allQty.ToString();
            values[5] = mostCost.ToString();
            values[6] = mostQty.ToString();
            runPythonScript(Arguments, "-u", values);

            return type + " " + startDate + " " + endDate;
        }

        public static void runPythonScript(string argument, string args = "", params string[] teps){
            string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + argument;
            string arguments = path;
            arguments = "\"" + arguments + "\"";
            foreach (string sigstr in teps)
            {
                arguments += " " + (string?)sigstr;
            }
            arguments += " " + args;

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python.exe";
            // 有空格的處理方式
            start.Arguments = arguments;
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using(Process process = Process.Start(start))
            {
                using(StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }
    }

}