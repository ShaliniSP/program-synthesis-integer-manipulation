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
    public delegate int? LearnedFunc(int[] arr);

    public static class Semantics
    {

        public static int? Eval(string v, LearnedFunc d)
        {
            // Console.WriteLine("In EVAL");
            string[] numbers = Regex.Split(v, @"\D+");
            int[] x = new int[numbers.Length];
            for(int i =0; i < numbers.Length; i++){
                x[i] = Int32.Parse(numbers[i]);
            }
            int? result;

            if (d != null)
            {
                result = d(x);
                return result;
            }
            return null;
        }
    }
}
