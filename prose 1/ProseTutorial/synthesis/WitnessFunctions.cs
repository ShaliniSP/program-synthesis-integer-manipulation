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

namespace ProseTutorial {
    public class WitnessFunctions : DomainLearningLogic {
        public WitnessFunctions(Grammar grammar) : base(grammar) {}

        // We will use this set of regular expressions in this tutorial 
        public static Regex[] UsefulRegexes = {
            new Regex(@"\w+"),  // Word
            new Regex(@"\d+"),  // Number
            new Regex(@"\s+"),  // Space
            new Regex(@".+"),  // Anything
            new Regex(@"$")  // End of line
        };

        public static int Plus(int a, int b) {
            return a + b;
        }

        public static int Minus(int a, int b) {
            return a - b;
        }
        [WitnessFunction(nameof(Semantics.Eval), 1)]
        public ExampleSpec WitnessFunc(GrammarRule rule, ExampleSpec spec) {
            Console.WriteLine("In Witness Func");
            var result = new Dictionary<State, object>();
            foreach (KeyValuePair<State, object> example in spec.Examples) {
                    //        foreach (KeyValuePair<State, object> example in spec.Examples)
                Console.WriteLine(example.Key);

                State inputState = example.Key;
                Console.WriteLine(inputState[rule.Body[0]]);
                Console.WriteLine(rule.Body[0]);
                var v = inputState[rule.Body[0]] as string;
                int a = Int32.Parse(v[0].ToString());
                int b = Int32.Parse(v[2].ToString());
                Console.WriteLine("Before op {0}", example.Value);
                int output = Int32.Parse((example.Value as string)[0].ToString() );
                Console.WriteLine("After op");
                LearnedFunc f = null;
                Console.WriteLine("a:{0}, b:{1}, c:{2}", a, b, output);
                if(a + b == output){
                    f = new LearnedFunc(Plus);
                    Console.WriteLine(f);
                }
                else if (a - b == output)
                    f = new LearnedFunc(Minus);
                result[inputState] = f;
            }
            Console.WriteLine("Exiting");
            ExampleSpec e = new ExampleSpec(result);
            Console.WriteLine("ExampleSpec(result): {0}",e);
            return e;
        }

        // [WitnessFunction(nameof(Semantics.Plus), 0)]
        // public ExampleSpec WitnessKPlus(GrammarRule rule, ExampleSpec spec) {
        //     Console.WriteLine("In Witness K PLUS");
        //     var result = new Dictionary<State, object>();
        //     foreach (KeyValuePair<State, object> example in spec.Examples) {
        //             //        foreach (KeyValuePair<State, object> example in spec.Examples)
        //        Console.WriteLine(example.Key);

        //         State inputState = example.Key;
        //         Console.WriteLine(inputState);
        //         Console.WriteLine("before v");
        //         Console.WriteLine(rule.Body[0]);
        //         var v = inputState[rule.Body[0]] as string;
        //         Console.WriteLine("after v");
        //         int a = Int32.Parse(v[0].ToString());
        //         int b = Int32.Parse(v[2].ToString());
        //         int output = (int)example.Value ;
        //         Console.WriteLine("a:{0}, b:{1}, c:{2}", a, b, output);
        //         if(a + b == output)
        //             result[inputState] = true;
        //         result[inputState] = false;

        //     }
        //     return new ExampleSpec(result);
        // }

        // [WitnessFunction(nameof(Semantics.Minus), 0)]
        // public ExampleSpec WitnessKMinus(GrammarRule rule, ExampleSpec spec) {
        //     Console.WriteLine("In Witness K MINUS");
        //     var result = new Dictionary<State, object>();
        //     foreach (KeyValuePair<State, object> example in spec.Examples) {
        //         State inputState = example.Key;
        //         var v = inputState[rule.Body[0]] as string;
        //         int a = Int32.Parse(v[0].ToString());
        //         int b = Int32.Parse(v[2].ToString());
        //         int output = (int)example.Value; 
        //         if(a - b == output)
        //             result[inputState] = true;
        //         result[inputState] = false;

        //     }
        //     return new ExampleSpec(result);
        // }

        // static void BuildStringMatches(string inp, out List<Regex>[] leftMatches,
        //                                out List<Regex>[] rightMatches) {
        //     Console.WriteLine("Build String Matches");
        //     leftMatches = new List<Regex>[inp.Length + 1];
        //     rightMatches = new List<Regex>[inp.Length + 1];
        //     for (int p = 0; p <= inp.Length; ++p) {
        //         leftMatches[p] = new List<Regex>();
        //         rightMatches[p] = new List<Regex>();
        //     }
        //     foreach (Regex r in UsefulRegexes) {
        //         foreach (Match m in r.Matches(inp)) {
        //             leftMatches[m.Index + m.Length].Add(r);
        //             rightMatches[m.Index].Add(r);
        //         }
        //     }
        // }

    }
}