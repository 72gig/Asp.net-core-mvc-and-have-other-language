using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IActionResultExample.Models
{
    public class Readsql{
        [FromQuery]
        public string? Type { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }

    }

    public class sqlResult{
        // 找單號 日期 客戶 金額
        [FromQuery]
        public string? code { get; set; }
        public DateTime? date { get; set; }
        public string? partnercode { get; set; }
        public string? partner { get; set; }
        public string? productcode { get; set; }
        public string? product { get; set; }
        public double? cost { get; set; }
        public double? qty { get; set; }

    }

    public class sqlUser : sqlResult
    {
        public List<sqlResult> selectSql(Readsql read){

            List<sqlResult> listData = new List <sqlResult>();

            // 連接資料庫
            SqlConnection conn = new SqlConnection("Server=10.0.0.21;Database=crge;User=test_erp;Password=test_erp;TrustServerCertificate=True");

            // 開啟資料庫
            conn.Open();

            // 設定table 和 where
            string? getTable = "";
            string? getWhere = "";
            if (read.Type == "saler"){
                getTable = "c105";
                getWhere = " (fiono='52') ";
            }else if (read.Type == "purchase"){
                getTable = "c105";
                getWhere = " (fiono='01') ";
            }else if (read.Type == "outside"){
                getTable = "c105";
                getWhere = " (fiono='05') ";
            }

            // 要防止有null 的資料 否則不能編輯
            string stfdate = "";
            if (read.StartDate != null){
                stfdate = read.StartDate;
            }else{
                stfdate = "2000/01/01";
            }
            string edfdate = "";
            if (read.EndDate != null){
                edfdate = read.EndDate;
            }else{
                edfdate = "2200/01/01";
            }

            getWhere = getWhere + " and (fdate > '" + settingDate(stfdate) + "') and (fdate < '" + settingDate(edfdate) + "')";
            // 依照 Readsql的資料決定得到什麼資料
            // 設定sql 指令
            using (var cmd = new SqlCommand("select flino, fdate, fcbno,fcbna, fpdno, fpdna, fbaqu, fpdpr from " + getTable + " where " + getWhere, conn))
            using (var reader = cmd.ExecuteReader())  // 執行並準備讀取
            {
                if (reader.HasRows){
                    while (reader.Read())
                    {
                        //保存的地方
                        sqlResult dataList = new sqlResult();
                        // 在這裡加入資料
                        dataList.code = (string)reader["flino"];
                        dataList.date = (DateTime)DateTime.ParseExact((string)reader["fdate"],"yyyy/mm/dd",null);
                        dataList.partnercode = (string)reader["fcbno"];
                        dataList.partner = (string)reader["fcbna"];
                        dataList.productcode = (string)reader["fpdno"];
                        dataList.product = (string)reader["fpdna"];
                        dataList.cost = (double)reader["fpdpr"];
                        dataList.qty = (double)reader["fbaqu"];

                        listData.Add(dataList);
                    }
                }
            }

            conn.Close();

            return listData;

        }
        public string settingDate(string getData){
            if (getData.Contains('-')){
                return getData.Replace('-','/');
            }
            return "";
        }
        
    }
    public class partnerTotal{
        public string Partner { get; set; }
        public double? PdCost { get; set; }
        public double? PdQty { get; set; }
    }
    public class productTotal{
        public string? ProductCode { get; set; }
        public string? Product { get; set; }
        public double? PdCost { get; set; }
        public double? PdQty { get; set; }
    }
}