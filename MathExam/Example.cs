using System;

namespace MathExam
{
    public class Result
    {
        public int MathResult;
        public int MathResidue;
    }

    public class ExapmleEvaluation
    {
        public bool IsAnswerCorrect;
        public string msg;
    }

    public class Example
    {
        private int op = 1;
        private int number1;
        private int number2;
        Random r = new Random();
        private StaticFields staticFields;

        public Example(StaticFields staticFields)
        {
            this.staticFields = staticFields;

            op = GetExamOperator();

            CreateNumbersInToExample(op);
        }

        public Result ReturnCorrectResult(int op)
        {
            Result result = new Result();
            switch (op)
            {
                case 1:
                    result.MathResult = number1 * number2;
                    break; // *
                case 2:
                    result.MathResult = number1 / number2;
                    result.MathResidue = number1 % number2;
                    break; // /
                case 3:
                    result.MathResult = number1 + number2;
                    break; // +
                case 4:
                    result.MathResult = number1 - number2;
                    break; //-
            }

            return result;
        }

        public ExapmleEvaluation CheckUserAnswer(int UserAnswer, int UserResidue)
        {
            ExapmleEvaluation eval = new ExapmleEvaluation();
            Result result = ReturnCorrectResult(op);
            if (result.MathResult == UserAnswer && result.MathResidue == UserResidue)
            {
                eval.IsAnswerCorrect = true;
            }
            else
            {
                eval.IsAnswerCorrect = false;
                eval.msg = "Chybná odpověď. Správně je to: " + result.MathResult + " a zbytek je : " +
                           result.MathResidue;
            }

            return eval;
        }

        void CreateNumbersInToExample(int Operator)
        {
            switch (Operator)
            {
                case 1:
                    number1 = r.Next(1, 20);
                    number2 = r.Next(1, 20);
                    break; // *
                case 2:
                    number1 = r.Next(20, 400);
                    number2 = r.Next(1, 20);
                    break; // /
                case 3:
                    number1 = r.Next(1, 1000);
                    number2 = r.Next(1, 1000);
                    break; // +
                case 4:
                    number1 = r.Next(500, 1000);
                    number2 = r.Next(1, 500);
                    break; // -
            }
        }

        private int GetExamOperator()
        {
            if (staticFields.GenerateRandomOperator)
            {
                return r.Next(1, 5);
            }
            else
            {
                return staticFields.SetedOp;
            }
        }

        public int GetOperator()
        {
            return op;
        }

        public string GetHumanReadableOperator()
        {
            switch (op)
            {
                case 1:
                    return "*";
                    break; // *
                case 2:
                    return "/";

                case 3:
                    return "+";
                case 4:
                    return "-";
                default:
                    return "e";
            }
        }

        public int GetFirstNumber()
        {
            return number1;
        }

        public int GetSecondNumber()
        {
            return number2;
        }

        public int ReturnN1()
        {
            return number1;
        }

        public int ReturnN2()
        {
            return number2;
        }

        public int ReturnOp()
        {
            return op;
        }
    }
}