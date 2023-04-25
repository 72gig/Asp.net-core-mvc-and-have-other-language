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
        public string? partner { get; set; }
        public string? product { get; set; }
        public float? cost { get; set; }

    }

    public class sqlUser{
        public List<sqlResult> selectSql(Readsql read){
            //保存的地方
            List<sqlResult> dataList = new List<sqlResult>();

            // 連接資料庫
            SqlConnection conn = new SqlConnection("Server=10.0.0.21;Database=crge;User=test_erp;Password=test_erp;TrustServerCertificate=True");

            // 開啟資料庫
            conn.Open();

            // 設定table 和 where
            string? getTable = "";
            string? getwhere = "";
            if (read.Type == "saler"){
                getTable = "c105";
                getwhere = " (fiono='52') ";
            }else if (read.Type == "purchase"){
                getTable = "c105";
                getwhere = " (fiono='01') ";
            }else if (read.Type == "outside"){
                getTable = "c105";
                getwhere = " (fiono='05') ";
            }
            // 依照 Readsql的資料決定得到什麼資料
            // 設定sql 指令
            using (var cmd = new SqlCommand("select * from " + getTable + " where " + getwhere, conn))
            using (var reader = cmd.ExecuteReader())  // 執行並準備讀取
            {
                while (reader.Read())
                {
                    // 在這裡加入資料
                }
            }

            return dataList;
        }
    }
}