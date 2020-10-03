using System;
using System.Configuration;
using System.Net;
using MySql.Data.MySqlClient;

namespace MathExam
{
    public class WebDbLogger
    {
        private string op;
        private string user;
        private string https;

        public void DataCollector(DateTime dt, int count, int FNumber, int SNumber, int op, bool TrueOrFalse,
            int userAnswer, int userRes)
        {
            switch (op)
            {
                case 1:
                    this.op = "*";
                    break;
                case 2:
                    this.op = "/";
                    break;
                case 3:
                    this.op = "+";
                    break;
                case 4:
                    this.op = "-";
                    break;
                default:
                    this.op = "nothing";
                    break;
            }

            if (ConfigurationSettings.AppSettings["user"] == "none")
            {
                user = Dns.GetHostName();
            }
            else
            {
                user = ConfigurationSettings.AppSettings["saveIn"];
            }

            
            WebClient webClient = new WebClient();
            
            webClient.QueryString.Add("id", Convert.ToString(dt));
            webClient.QueryString.Add("users", user);
            webClient.QueryString.Add("order", Convert.ToString(count));
            webClient.QueryString.Add("firstn", Convert.ToString(FNumber));
            webClient.QueryString.Add("secondn", Convert.ToString(SNumber));
            webClient.QueryString.Add("op", this.op);
            webClient.QueryString.Add("torf", Convert.ToString(TrueOrFalse));
            webClient.QueryString.Add("answer", Convert.ToString(userAnswer)+"zb."+Convert.ToString(userRes));
            
            string result = webClient.DownloadString("http://jarda.cekuj.net/MiddleMan/");
            Console.WriteLine(result);
            
            //http://jarda.cekuj.net/MiddleMan/MiddleMan.php?id=1&users=jaja&order=1&firstn=10&secondn=3&op=divide&torf=1&answer=10zb1
        }
    }
}