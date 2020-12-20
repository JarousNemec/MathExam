using System;
using System.Configuration;
using System.Data.OleDb;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace MathExam.ConfigTool
{
    public class ConfigData
    {
        public string user;
        public string saveIn;
        public string port;
        public string dbUser;
        public string pass;
        public string dbIp;
        public string connString;
        
    }
    public class ConfigMan : Form
    {
        ConfigData cd = new ConfigData();
        
        Label lUser = new Label();
        Label lSavein = new Label();
        Label lConnString = new Label();
        Label lPort = new Label();
        Label lDBuser = new Label();
        Label lPass = new Label();
        Label lDbIp = new Label();

        Button btSave = new Button();

        RadioButton rbtLocDb = new RadioButton();
        RadioButton rbtWebDb = new RadioButton();

        TextBox tbUser = new TextBox();
        TextBox tbPort = new TextBox();
        TextBox tbDBuser = new TextBox();
        TextBox tbPass = new TextBox();
        TextBox tbDBip = new TextBox();

        public ConfigMan()
        {
            Inicializace();
            LoadData();
            FillDataToGui();
            Functions();
        }


        private void Inicializace()
        {
            //window set
            Text = "!Config!";
            Size = new Size(600, 650);
            MaximizeBox = false;
            MinimumSize = new Size(600, 650);
            MaximumSize = new Size(600, 650);
            StartPosition = FormStartPosition.CenterScreen;

            //labels
            lUser.Text = "User ";
            lUser.Location = new Point(25, 25);
            lUser.Size = new Size(80, 27);
            lUser.Font = new Font("Arial", 20);

            lSavein.Text = "Save on ";
            lSavein.Location = new Point(25, 80);
            lSavein.Size = new Size(120, 27);
            lSavein.Font = new Font("Arial", 20);

            lConnString.Text = "Local connection data";
            lConnString.Location = new Point(20, 140);
            lConnString.Size = new Size(350, 35);
            lConnString.Font = new Font("Arial", 25);

            lPass.Text = "DbPass ";
            lPass.Location = new Point(25, 195);
            lPass.Size = new Size(120, 27);
            lPass.Font = new Font("Arial", 20);

            lPort.Text = "DbPort ";
            lPort.Location = new Point(25, 250);
            lPort.Size = new Size(120, 27);
            lPort.Font = new Font("Arial", 20);

            lDBuser.Text = "DbUser ";
            lDBuser.Location = new Point(25, 305);
            lDBuser.Size = new Size(120, 27);
            lDBuser.Font = new Font("Arial", 20);

            lDbIp.Text = "DbIp ";
            lDbIp.Location = new Point(25, 360);
            lDbIp.Size = new Size(120, 35);
            lDbIp.Font = new Font("Arial", 20);

            //CheckBoxes
            rbtLocDb.Text = "Local";
            rbtLocDb.Location = new Point(180, 83);
            rbtLocDb.Size = new Size(100, 27);
            rbtLocDb.Font = new Font("Arial", 20);

            rbtWebDb.Text = "Web";
            rbtWebDb.Location = new Point(300, 83);
            rbtWebDb.Size = new Size(100, 27);
            rbtWebDb.Font = new Font("Arial", 20);

            //Textboxes
            tbUser.Text = "User";
            tbUser.Location = new Point(160, 20);
            tbUser.Size = new Size(200, 30);
            tbUser.Font = new Font("Arial", 20);

            tbPass.Text = "pass";
            tbPass.Location = new Point(160, 190);
            tbPass.Size = new Size(200, 30);
            tbPass.Font = new Font("Arial", 20);

            tbPort.Text = "port";
            tbPort.Location = new Point(160, 245);
            tbPort.Size = new Size(200, 30);
            tbPort.Font = new Font("Arial", 20);

            tbDBuser.Text = "dbuser";
            tbDBuser.Location = new Point(160, 300);
            tbDBuser.Size = new Size(200, 30);
            tbDBuser.Font = new Font("Arial", 20);

            tbDBip.Text = "ip";
            tbDBip.Location = new Point(160, 355);
            tbDBip.Size = new Size(200, 30);
            tbDBip.Font = new Font("Arial", 20);

            //buttons
            btSave.Text = "SAVE    ";
            btSave.Location = new Point(270, 580);
            // btSave.Size = new Size(200,30);
            // btSave.Font = new Font("Arial", 20);

            //add into window
            this.Controls.Add(lUser);
            this.Controls.Add(lSavein);
            this.Controls.Add(lConnString);
            this.Controls.Add(lDBuser);
            this.Controls.Add(lPass);
            this.Controls.Add(lPort);
            this.Controls.Add(lDbIp);

            this.Controls.Add(rbtLocDb);
            this.Controls.Add(rbtWebDb);

            this.Controls.Add(tbUser);
            this.Controls.Add(tbPass);
            this.Controls.Add(tbPort);
            this.Controls.Add(tbDBuser);
            this.Controls.Add(tbDBip);

            this.Controls.Add(btSave);
        }

        private void LoadData()
        {
            cd.user = ConfigurationSettings.AppSettings["user"];
            cd.saveIn = ConfigurationSettings.AppSettings["saveIn"];
            cd.connString = ConfigurationSettings.AppSettings["MySQLConnectionString"];
            
            cd.pass = DataExtractor2( cd.connString, "password");
            cd.port = DataExtractor2( cd.connString, "port");
            cd.dbUser = DataExtractor( cd.connString, "username");
            cd.dbIp = DataExtractor( cd.connString, "datasource");
        }

        private void FillDataToGui()
        {
            tbUser.Text = cd.user;
            if (cd.saveIn == "web")
            {
                rbtWebDb.Checked = true;
            }
            else
            {
                rbtLocDb.Checked = true;
            }

            tbPass.Text = cd.pass;
            tbPort.Text = cd.port;
            tbDBuser.Text = cd.dbUser;
            tbDBip.Text = cd.dbIp;
        }

        private void SaveChangesToAppStorage()
        {
            cd.user = tbUser.Text;
            if (rbtLocDb.Checked)
            {
                cd.saveIn = "local";
            }
            else
            {
                cd.saveIn = "web";
            }

            cd.pass = tbPass.Text;
            cd.port = tbPort.Text;
            cd.dbUser = tbDBuser.Text;
            cd.dbIp = tbDBip.Text;
            cd.connString = ("datasource="+cd.dbIp+";port="+cd.port+";username="+cd.dbUser+";password="+cd.pass);

        }

        private void SaveChanges()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            config.AppSettings.Settings.Remove("user");
            config.AppSettings.Settings.Add("user", cd.user);
            
            config.AppSettings.Settings.Remove("MySQLConnectionString");
            config.AppSettings.Settings.Add("MySQLConnectionString", cd.connString);
            
            config.AppSettings.Settings.Remove("saveIn");
            config.AppSettings.Settings.Add("saveIn", cd.saveIn);

            config.Save(ConfigurationSaveMode.Modified);
        }


        private string DataExtractor2(string stringToExtract, string fieldToFind)
        {
            String[] values = stringToExtract.Split(';');
            foreach (var value in values)
            {
                if (value.Contains(fieldToFind))
                {
                    return value.Split('=')[1];
                }
            }

            return "nemame";
        }
    
        
        
        private string DataExtractor(string stringToExtract, string fieldToFind)
        {
            string fieldValue = "";
            string fieldInProgress = "";
            bool couldBeEnd = false;
            char[] chars = stringToExtract.ToCharArray();

            foreach (char ch in chars)
            {
                if (ch.Equals(';'))
                {
                    fieldInProgress = "";
                    //Console.WriteLine("bacha");
                    if (couldBeEnd)
                    {
                        break;
                    }
                }
                else if (ch.Equals('='))
                {
                    Console.WriteLine("rovnadlo");
                }
                else if (fieldInProgress == fieldToFind)
                {
                    fieldValue = fieldValue + ch;
                    couldBeEnd = true;
                }
                else
                {
                    fieldInProgress = fieldInProgress + ch;
                }
            }

            Console.WriteLine(fieldInProgress);
            
            return fieldValue;
        }

        private void Functions()
        {
            btSave.Click += (s, e) =>
            {
                SaveChangesToAppStorage();
                SaveChanges();
                Thread.Sleep(500);
                Application.Restart();
                Environment.Exit(0);
            };
        }
    }
}