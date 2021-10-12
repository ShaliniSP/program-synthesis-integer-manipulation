using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.ProgramSynthesis.Utils;


namespace ProseTutorial {
    public delegate int LearnedFunc(int a, int b);

    public static class Semantics {

        public static int? Eval(string v, LearnedFunc d){
            Console.WriteLine("In EVAL");
            int x = Int16.Parse(v[0].ToString());
            int y = Int16.Parse(v[2].ToString());
            int result;
            
            if(d != null)
            {    
                result = d(x,y);
                return result;
            }
            return null;
        }

        // public  static LearnedFunc Plus(bool k) {
        //     Console.WriteLine("In PLUS");
        //     if(k == true){
        //         return (a,b) => a + b;
        //         //return plus;
        //     }
        //     return null;
        // }

        // public static LearnedFunc Minus(bool k) {
        //     Console.WriteLine("In MINUS");
        //     if(k == true){
        //         return (x, y) => x - y;
        //         //return minus;
        //     }
        //     return null;
        // }
    }
}
