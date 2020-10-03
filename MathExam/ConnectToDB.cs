using System;
using System.Configuration;
using System.Data;
using System.Net;
using MySql.Data.MySqlClient;

namespace MathExam
{
    public class ConnectToDb
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string DB_SCHEMA_NAME = "exampleapp";
        public MySqlConnection Connection(DateTime dt)
        {
            log.Info("connecting to DB");

            MySqlConnection connection = openConnectionToDB();
            if (connection == null)
            {
                log.Info("I can´t connected to Database");
            }
            else
            {
                log.Info("connected to DB");
                log.Info("checking is schema existing");
                if (!IsDBschemaValid(connection))
                {
                    log.Info("schema not existing");
                    log.Info("creating schema");
                    createSchema(connection);
                }

                forceDatabaseName(DB_SCHEMA_NAME, connection);
                LogToSingpost(dt,Dns.GetHostName(),connection);
            }

            return connection;
        }
        MySqlCommand cmd;
        private void LogToSingpost(DateTime dt, string user,MySqlConnection connection)
        {
            cmd =
                new MySqlCommand(
                    "INSERT INTO examappssingpost (ID,Users) VALUES (@dt,@user)", connection);
            

            cmd.Parameters.AddWithValue("@dt", dt);
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
        }
        private void forceDatabaseName(string dbSchemaName, MySqlConnection connection)
        {
            try
            {
                MySqlCommand cmd1 = new MySqlCommand("use `" + DB_SCHEMA_NAME + "`;", connection);
                cmd1.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log.Info("Force Schema problem > " + e.Message);
                return;
            }
        }

        private void createSchema(MySqlConnection connection)
        {
            try
            {
                MySqlCommand cmd1 = new MySqlCommand("CREATE SCHEMA `" + DB_SCHEMA_NAME + "`;", connection);
                cmd1.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log.Info("create new schema problem > " + e.Message);
                return;
            }

            try
            {
                MySqlCommand cmd2 = new MySqlCommand(
                    "CREATE TABLE `exampleapp`.`examappsexamples` (`ID` DATETIME NOT NULL,`ExampleOrder` INT NOT NULL,`FirstNumber` INT NOT NULL,`SecondNumber` INT NOT NULL,`Operator` VARCHAR(45) NOT NULL,`TrueOrFalse` TINYINT NOT NULL,`UserAnswer` VARCHAR(45) NOT NULL);",
                    connection);
                cmd2.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log.Info("create new exampletable problem > " + e.Message);
                return;
            }

            try
            {
                MySqlCommand cmd3 =
                    new MySqlCommand(
                        "CREATE TABLE `exampleapp`.`examappssingpost` (`ID` DATETIME NOT NULL,`Users` VARCHAR(45) NOT NULL);",
                        connection);
                cmd3.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                log.Info("create new maininfotable problem > " + e.Message);
                return;
            }
        }

        private bool IsDBschemaValid(MySqlConnection connection)
        {
            MySqlCommand cmd =
                new MySqlCommand("SELECT SCHEMA_NAME FROM INFORMATION_SCHEMA.SCHEMATA WHERE SCHEMA_NAME = 'exampleapp'",
                    connection);

            var reader = cmd.ExecuteReader();
            int SCHEMA_NAME = reader.GetOrdinal("SCHEMA_NAME");

            bool hasMoreLines = reader.Read();
            reader.Close();
            return hasMoreLines;
        }
        
        private MySqlConnection openConnectionToDB()
        {
            try
            {
                var MySQLConnectionString = ConfigurationSettings.AppSettings["MySQLConnectionString"];
                MySqlConnection connection = new MySqlConnection(MySQLConnectionString);
                connection.Open();

                if (connection.State == ConnectionState.Open)
                {
                    log.Info("connected to schema");
                    return connection;
                }
                else
                {
                    log.Info("error");
                }
            }

            catch (Exception ex)
            {
                log.Info(ex);
            }

            return null;
        }
    }
}