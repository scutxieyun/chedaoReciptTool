using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using WndInteract;
namespace CheDaoReciptHike
{
    static class NewShuiKongInterface
    {
        static int status;
        public static int init() {
            ScriptExecuter.init();
            status = -1;
            if(ScriptExecuter.readScript("chedaoshuikong_cfg.txt") != true) return -1;
            status = 0;
            ScriptExecuter.debug_level = Program.trace_sw.Level;
            return 0;
        }
        public static int SendRecipt(CheRequest req) {
            if (status != 0) { return -1; }
            Dictionary<String, String> rec = new Dictionary<string, string>();
            rec.Add("Customer_Text",req.Customer_Text);
            rec.Add("Product_Code", req.Product_Code);
            rec.Add("Product_Price",req.Product_Price);
            rec.Add("Amount", req.Amount);
            rec.Add("Product_Number", req.Product_Number);
            rec.Add("Product_Type", req.Product_Type);
            rec.Add("Pump_Number", req.Pump_Number);
            return ScriptExecuter.execute(rec);
        }
        public static string getLastError() {
            return ScriptExecuter.getLastError();
        }
    }
}
