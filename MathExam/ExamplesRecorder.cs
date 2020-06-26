using System;
using System.IO;
using System.Net;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

namespace MathExam
{
    public class ExamplesRecorder
    {
        private int n1;
        private int n2;
        private string correctly;
        private int userResult;
        private int userResidue;
        private int trueResult;
        private int trueResidue;
        private int nOfExample;

        private string op = "+";

        ExamplesLogger logger = new ExamplesLogger();

        public void DataCollector(int n1, int n2, int op, bool correctly, int userResult, int userResidue, int nOfExample,Example example)
        {
            this.n1 = n1;
            this.n2 = n2;
            this.userResult = userResult;
            this.userResidue = userResidue;
            example.ReturnCorrectResult(op);
            Result result = example.ReturnCorrectResult(op);
            trueResult = result.MathResult;
            trueResidue = result.MathResidue;
            this.nOfExample = nOfExample;
            switch (op)
            {
                case 1:
                    this.op = " * ";
                    break;
                case 2:
                    this.op = " / ";
                    break;
                case 3:
                    this.op = " + ";
                    break;
                case 4:
                    this.op = " - ";
                    break;
                default:
                    this.op = " nothing ";
                    break;
            }

            if (correctly)
            {
                this.correctly = "správně";
            }
            else
            {
                this.correctly = "špatně";
            }
            DataWritter();
        }

        private void DataWritter()
        {
            ExamplesLogger.Write("----------------------------------------------------------\n");
            ExamplesLogger.Write("Příklad č. " + (nOfExample - 1) + "\n");
            ExamplesLogger.Write("Status: " + correctly + "\n");
            ExamplesLogger.Write("Příklad: " + n1 + op + n2 + "\n");
            ExamplesLogger.Write("Správný výsledek: " + trueResult + " zb. " + trueResidue + "\n");
            ExamplesLogger.Write("Odpověď uživatele: " + userResult + " zb. " + userResidue + "\n");
            ExamplesLogger.Write("Čas: " + DateTime.Now.Hour + ":" + DateTime.Now.Minute + ":" + DateTime.Now.Second + "\n");
            ExamplesLogger.Write("Datum: " + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + "\n");
            ExamplesLogger.Write("----------------------------------------------------------\n");
        }

        /*public TextWriter ReturnTextWriter()
        {
            return tw;
        }*/
        
    }
}