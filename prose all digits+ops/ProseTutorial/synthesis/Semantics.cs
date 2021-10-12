using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.ProgramSynthesis.Utils;


namespace ProseTutorial
{
    public delegate int? LearnedFunc(int a, int b);

    public static class Semantics
    {

        public static int? Eval(string v, LearnedFunc d)
        {
            // Console.WriteLine("In EVAL");
            string[] numbers = Regex.Split(v, @"\D+");
            int x = Int32.Parse(numbers[0].ToString());
            int y = Int32.Parse(numbers[1].ToString());
            int? result;

            if (d != null)
            {
                result = d(x, y);
                return result;
            }
            return null;
        }
    }
}
