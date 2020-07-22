using System;
using System.CodeDom;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MathExam
{
    // Contains Example settings
    public class StaticFields
    {
        public int SetOp = 1;
        public bool GenerateRandomOperator = true;
    }

    
    public class MathExam : Form
    {
        Button actionButton = new Button();
        Button resetButton = new Button();
        Label l1 = new Label();
        Label l2 = new Label();
        Label l3 = new Label();
        Label l4 = new Label();
        Label l5 = new Label();
        TextBox tb1 = new TextBox();
        TextBox tb2 = new TextBox();

        TextBox HistoryViewer = new TextBox();
        HistoryPreview hp;

        MainMenu MenuBar = new MainMenu();

        MenuItem menu = new MenuItem("Menu");

        MenuItem ExamplesMenu = new MenuItem("Příklady (Mathematicus)");
        MenuItem ShowExamplas = new MenuItem("Zobrazit příklady");
        MenuItem History = new MenuItem("Historie");

        MenuItem Operators = new MenuItem("volba příkladů");
        MenuItem OpPlus = new MenuItem("pouze sčítání");
        MenuItem OpMinus = new MenuItem("pouze odčítání");
        MenuItem OpTimes = new MenuItem("pouze násobení");
        MenuItem OpDivided = new MenuItem("pouze dělení");
        MenuItem OpRandomGenerated = new MenuItem("náhodné příklady");

        StaticFields staticFields = new StaticFields();
        ExamplesRecorder er = new ExamplesRecorder();
        DateTime dt;
        
        
        private MySqlConnection msc;

        private Example example;
        private ConnectToDb connectionToDb;
        private LoggerToDB loggerToDb;

        public int trueR = 0;
        public int falseR = 0;
        public int nCount = 1;
        
        private const string ChangeSettingMsg = "Změny se projevý u dalšího příkladu";

        private bool isCountingAvailable;
        private bool isTextWriterClose;
        private bool DBisConnected = true;

        private string user;
        
        public MathExam()
        {
            dt = new DateTime();
            dt = DateTime.Now;
            connectionToDb = new ConnectToDb();
            loggerToDb = new LoggerToDB();
            example = new Example(staticFields);
            
            Inicializace();
            
            msc = connectionToDb.Connection(dt);
            if (msc == null) 
            {
                DBisConnected = false;
            }
            
            Functions();
        }

        private void Inicializace()
        {
            this.Text = "!MathExam!";
            this.Size = new Size(600, 650);
            this.MaximizeBox = false;
            this.MinimumSize = new Size(600, 650);
            this.MaximumSize = new Size(600, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            actionButton.Size = new Size(100, 60);
            actionButton.Location = new Point(250, 50);
            actionButton.Text = "Start";
            actionButton.Font = new Font("Arial", 24);

            resetButton.Size = new Size(200, 60);
            resetButton.Location = new Point(370, 500);
            resetButton.Text = "Resetovat";
            resetButton.Font = new Font("Arial", 24);

            l1.Size = new Size(300, 60);
            l1.Location = new Point(10, 300);
            l1.Text = Convert.ToString(example.GetFirstNumber()) + example.GetHumanReadableOperator() +
                      Convert.ToString(example.GetSecondNumber()) + "=";
            l1.Font = new Font("Arial", 40);
            l1.BackColor = Color.Gray;

            l2.Size = new Size(250, 60);
            l2.Location = new Point(10, 150);
            l2.Text = "True: " + Convert.ToString(trueR);
            l2.Font = new Font("Arial", 40);
            l2.BackColor = Color.ForestGreen;

            l3.Size = new Size(250, 60);
            l3.Location = new Point(270, 150);
            l3.Text = "False: " + Convert.ToString(falseR);
            l3.Font = new Font("Arial", 40);
            l3.BackColor = Color.Red;

            l4.Size = new Size(500, 60);
            l4.Location = new Point(10, 500);
            l4.Text = "Příklad číslo " + Convert.ToString(nCount);
            l4.Font = new Font("Arial", 40);

            l5.Size = new Size(300, 60);
            l5.Location = new Point(10, 400);
            l5.Text = Convert.ToString("zbytek: ");
            l5.Font = new Font("Arial", 40);
            l5.BackColor = Color.Gray;

            tb1.Size = new Size(250, 600);
            tb1.Font = new Font("Arial", 35);
            tb1.Location = new Point(315, 300);

            tb2.Size = new Size(250, 600);
            tb2.Font = new Font("Arial", 35);
            tb2.Location = new Point(315, 400);
            tb2.Text = "0";

            HistoryViewer.Size = new Size(560, 570);
            HistoryViewer.Location = new Point(10, 10);
            HistoryViewer.Visible = false;
            HistoryViewer.ScrollBars = ScrollBars.Vertical;
            HistoryViewer.Multiline = true;

            MenuBar.MenuItems.Add(menu);

            menu.MenuItems.Add(ExamplesMenu);
            menu.MenuItems.Add(History);

            ExamplesMenu.MenuItems.Add(Operators);
            ExamplesMenu.MenuItems.Add(ShowExamplas);

            Operators.MenuItems.Add(OpPlus);
            Operators.MenuItems.Add(OpMinus);
            Operators.MenuItems.Add(OpTimes);
            Operators.MenuItems.Add(OpDivided);
            Operators.MenuItems.Add(OpRandomGenerated);

            Menu = MenuBar;

            this.Controls.Add(actionButton);
            AddComponents();
            SetVissibleExampleComponents(false);
            SetVissibleHistoryViewComponents(false);
            actionButton.Visible = true;
        }

        private void Functions()
        {
            actionButton.Click += (s, e) => { b1Control(s, e); };

            resetButton.Click += (s, e) =>
            {
                nCount = 1;
                RefreshScreen();
            };

            ShowExamplas.Click += (s, e) =>
            {
                SetVissibleHistoryViewComponents(false);
                SetVissibleExampleComponents(true);
                VisibleOrInvisibleResidueComponents(example.GetOperator());
            };

            History.Click += (s, e) =>
            {
                SetVissibleExampleComponents(false);
                SetVissibleHistoryViewComponents(true);
                hp = new HistoryPreview(HistoryViewer);
                hp.ReadHistoryFile();
            };

            OpPlus.Click += (s, e) =>
            {
                staticFields.SetOp = 3;
                staticFields.GenerateRandomOperator = false;
                MessageBox.Show(ChangeSettingMsg);
            };

            OpMinus.Click += (s, e) =>
            {
                staticFields.SetOp = 4;
                staticFields.GenerateRandomOperator = false;
                MessageBox.Show(ChangeSettingMsg);
            };

            OpTimes.Click += (s, e) =>
            {
                staticFields.SetOp = 1;
                staticFields.GenerateRandomOperator = false;
                MessageBox.Show(ChangeSettingMsg);
            };

            OpDivided.Click += (s, e) =>
            {
                staticFields.SetOp = 2;
                staticFields.GenerateRandomOperator = false;
                MessageBox.Show(ChangeSettingMsg);
            };

            OpRandomGenerated.Click += (s, e) =>
            {
                staticFields.GenerateRandomOperator = true;
                MessageBox.Show(ChangeSettingMsg);
            };
        }

        private void SetVissibleExampleComponents(bool status)
        {
            actionButton.Visible = status;
            resetButton.Visible = status;
            l1.Visible = status;
            l2.Visible = status;
            l3.Visible = status;
            l4.Visible = status;
            l5.Visible = status;
            tb1.Visible = status;
            tb2.Visible = status;
            
        }

        private void SetVissibleHistoryViewComponents(bool status)
        {
            HistoryViewer.Visible = status;
        }

        private void b1Control(object sender, EventArgs e)
        {
            if (!isCountingAvailable)
            {
                actionButton.Text = "Next";
                SetVissibleExampleComponents(true);
                isCountingAvailable = true;
                nCount++;
            }
            else
            {
                int UserAnswer = GetUserResult();
                int UserResidue = GetUserResidue();
                ExapmleEvaluation eval = example.CheckUserAnswer(UserAnswer, UserResidue);
                if (eval.IsAnswerCorrect)
                {
                    trueR++;
                }
                else
                {
                    falseR++;
                    MessageBox.Show(eval.msg, "Chyba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                er.DataCollector(example.ReturnN1(), example.ReturnN2(), example.ReturnOp(), eval.IsAnswerCorrect,
                    UserAnswer,
                    UserResidue, nCount,example);
                if (DBisConnected)
                {
                    loggerToDb.DataCollector(msc,dt,nCount,example.ReturnN1(), example.ReturnN2(), example.ReturnOp(), eval.IsAnswerCorrect,UserAnswer,UserResidue);
                }
                example = new Example(staticFields);
                RefreshScreen();

                nCount++;
                Console.WriteLine(UserAnswer);
            }

            VisibleOrInvisibleResidueComponents(example.GetOperator());
        }

        private void AddComponents()
        {
            this.Controls.Add(resetButton);
            this.Controls.Add(tb1);
            this.Controls.Add(tb2);
            this.Controls.Add(l1);
            this.Controls.Add(l2);
            this.Controls.Add(l3);
            this.Controls.Add(l4);
            this.Controls.Add(l5);
            this.Controls.Add(HistoryViewer);
        }

        private void RefreshScreen()
        {
            l1.Text = Convert.ToString(example.GetFirstNumber()) + example.GetHumanReadableOperator() +
                      Convert.ToString(example.GetSecondNumber()) + "=";
            l2.Text = "True: " + Convert.ToString(trueR);
            l3.Text = "False: " + Convert.ToString(falseR);

            l4.Text = "Příklad číslo " + Convert.ToString(nCount);
            tb1.Text = "";
            tb2.Text = "0";
        }

        private void VisibleOrInvisibleResidueComponents(int op)
        {
            if (op == 2)
            {
                tb2.Visible = true;
                l5.Visible = true;
            }
            else
            {
                tb2.Visible = false;
                l5.Visible = false;
            }
        }

        private int GetUserResult()
        {
            int n = 0;
            try
            {
                n = int.Parse(tb1.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show("Jako vysledek piš pouze čísla");
            }

            return n;
        }

        private int GetUserResidue()
        {
            int n = 0;
            try
            {
                n = int.Parse(tb2.Text);
            }
            catch (Exception e)
            {
                MessageBox.Show("Jako zbytek piš pouze čísla");
            }

            return n;
        }

        public TextBox GetHistoryViewer()
        {
            return HistoryViewer;
        }
    }
}