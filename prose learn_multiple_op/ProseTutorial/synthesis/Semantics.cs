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

        public static int? Eval(string v, char[] ops)
        {
            Console.WriteLine("In EVAL");
            string[] numbers = Regex.Split(v, @"\D+");
            int[] x = new int[numbers.Length];
            for(int i =0; i < numbers.Length; i++){
                 x[i] = Int32.Parse(numbers[i]);
             }
            var result = x[0];
            for(int i =1; i< numbers.Length; ++i){
                switch(ops[i-1]){
                    case '+':
                        result = result + x[i];
                        break;
                    case '-':
                        result = result - x[i];
                        break;
                    case '*':
                        result = result * x[i];
                        break;
                    case '/':
                        if(x[i] !=0)
                            result = result / x[i];
                        else 
                            return null;
                        break;
                    default:
                        break;
                }
            }
            return result;
        }
    }
}
