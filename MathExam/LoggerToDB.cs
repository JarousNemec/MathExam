using System;
using System.Globalization;
using MySql.Data.MySqlClient;

namespace MathExam
{
    public class LoggerToDB
    {
        private string op;

        
        MySqlCommand cmd2;

        public void DataCollector(MySqlConnection connection, DateTime dt, int order, int FNumber,
            int SNumber, int op, bool TrueOrFalse, int userAnswer, int userRes)
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
            
            cmd2 =
                new MySqlCommand(
                    "INSERT INTO examappsexamples (ID, ExampleOrder,FirstNumber, SecondNumber, Operator, TrueOrFalse,UserAnswer) VALUES (@dt,@order, @fn, @sn, @op, @TorF,@userAnswer)",
                    connection);
            
            cmd2.Parameters.AddWithValue("@dt", dt);
            cmd2.Parameters.AddWithValue("@order", order-1);
            cmd2.Parameters.AddWithValue("@fn", FNumber);
            cmd2.Parameters.AddWithValue("@sn", SNumber);
            cmd2.Parameters.AddWithValue("@op", this.op);
            cmd2.Parameters.AddWithValue("@TorF", TrueOrFalse);
            cmd2.Parameters.AddWithValue("@userAnswer", userAnswer + userRes);
            cmd2.Prepare();
            
            WriteToDB(connection);
        }

        private void WriteToDB(MySqlConnection connection)
        {
            try
            {
                
                cmd2.ExecuteNonQuery();
                Console.WriteLine("successfully wrote to db");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}