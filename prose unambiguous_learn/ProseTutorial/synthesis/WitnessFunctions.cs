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

        public static int? Plus(int[] a)
        {
            Console.WriteLine("Witness Plus");
            int result = a[0];
            foreach( int x in a.Skip(1)){
                Console.WriteLine(x);
                result += x;
            }
            Console.WriteLine("REsult:{0}", result);
            return result;
        }

        public static int? Minus(int[] a)
        {
            int result = a[0];
            foreach( int x in a.Skip(1)){
                result -= x;
            }
            return result;
        }

        public static int? Multiply(int[] a)
        {
            Console.WriteLine("Witness Multiply");
            int result = a[0];
            foreach( int x in a.Skip(1)){
                Console.WriteLine(result);
                result *= x;
            }
            return result;
        }

        public static int? Divide(int[] a)
        {
            int result = a[0];
            foreach( int x in a.Skip(1)){
                if(x != 0)
                    result /= x;
                else
                    return null;
            }
            return result;
        }
        [WitnessFunction(nameof(Semantics.Eval), 1)]
        public DisjunctiveExamplesSpec WitnessFunc(GrammarRule rule, ExampleSpec spec)
        {
            // Console.WriteLine("In Witness Func");
            var result = new Dictionary<State, IEnumerable<object>>();
            foreach (KeyValuePair<State, object> example in spec.Examples)
            {
                // Console.WriteLine(example.Key);

                State inputState = example.Key;
                // Console.WriteLine(inputState[rule.Body[0]]);
                // Console.WriteLine(rule.Body[0]);
                var v = inputState[rule.Body[0]] as string;
            
                string[] numbers = Regex.Split(v, @"\D+");
                int[] x = new int[numbers.Length];
                for(int i = 0; i < numbers.Length; ++i){
                    x[i] = Int32.Parse(numbers[i]);
                    Console.WriteLine(x);
                }
                int output = Int32.Parse((example.Value as string).ToString());
                // Console.WriteLine("After op {0}", output);
                var functions = new List<LearnedFunc>();
                //LearnedFunc f = null;
                // Console.WriteLine("a:{0}, b:{1}, c:{2}", a, b, output);
                if (Multiply(x) == output)
                    functions.Add(new LearnedFunc(Multiply));

                if (Divide(x) == output)
                    functions.Add(new LearnedFunc(Divide));
                
                if (Plus(x) == output)
                {
                    functions.Add(new LearnedFunc(Plus));
                    // Console.WriteLine(f);
                }
                if (Minus(x) == output)
                    functions.Add(new LearnedFunc(Minus));

                if(functions.Count == 0)
                    return null;
                result[inputState] = functions;
                foreach(LearnedFunc f in functions){
                    Console.WriteLine(f);
                }
            }
            // Console.WriteLine("Exiting");
            //ExampleSpec e = new ExampleSpec(result);
            // Console.WriteLine("ExampleSpec(result): {0}", e);
            return new DisjunctiveExamplesSpec(result);
        }
    }
}