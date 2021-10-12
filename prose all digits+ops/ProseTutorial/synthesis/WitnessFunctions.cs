using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.ProgramSynthesis;
using System.Threading.Tasks;
using Microsoft.ProgramSynthesis.Rules;
using Microsoft.ProgramSynthesis.Specifications;
using Microsoft.ProgramSynthesis.Learning;
using ProseTutorial;

namespace ProseTutorial
{
    public class WitnessFunctions : DomainLearningLogic
    {
        public WitnessFunctions(Grammar grammar) : base(grammar) { }

        public static Regex[] UsefulRegexes = {
            new Regex(@"\w+"),  // Word
            new Regex(@"\d+"),  // Number
            new Regex(@"\s+"),  // Space
            new Regex(@".+"),  // Anything
            new Regex(@"$")  // End of line
        };

        public static int? Plus(int a, int b)
        {
            return a + b;
        }

        public static int? Minus(int a, int b)
        {
            return a - b;
        }

        public static int? Multiply(int a, int b)
        {
            return a * b;
        }

        public static int? Divide(int a, int b)
        {
            if (b != 0)
                return a / b;

            else
                return null;
        }
        [WitnessFunction(nameof(Semantics.Eval), 1)]
        public ExampleSpec WitnessFunc(GrammarRule rule, ExampleSpec spec)
        {
            // Console.WriteLine("In Witness Func");
            var result = new Dictionary<State, object>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                // Console.WriteLine(example.Key);

                State inputState = example.Key;
                // Console.WriteLine(inputState[rule.Body[0]]);
                // Console.WriteLine(rule.Body[0]);
                var v = inputState[rule.Body[0]] as string;
                
                
                

                string[] numbers = Regex.Split(v, @"\D+");
                int a = Int32.Parse(numbers[0].ToString());
                int b = Int32.Parse(numbers[1].ToString());
                // Console.WriteLine(a);
                // Console.WriteLine(b);

                // Console.WriteLine("Before op {0}", example.Value);
                int output = Int32.Parse((example.Value as string).ToString());
                // Console.WriteLine("After op {0}", output);
                LearnedFunc f = null;
                // Console.WriteLine("a:{0}, b:{1}, c:{2}", a, b, output);
                if (a * b == output)
                    f = new LearnedFunc(Multiply);

                else if (a / b == output)
                    f = new LearnedFunc(Divide);
                
                else if (a + b == output)
                {
                    f = new LearnedFunc(Plus);
                    // Console.WriteLine(f);
                }
                else if (a - b == output)
                    f = new LearnedFunc(Minus);

                result[inputState] = f;
            }
            // Console.WriteLine("Exiting");
            ExampleSpec e = new ExampleSpec(result);
            // Console.WriteLine("ExampleSpec(result): {0}", e);
            return e;
        }
    }
}