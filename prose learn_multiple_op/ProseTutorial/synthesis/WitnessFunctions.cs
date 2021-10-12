using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
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


        public static string RunFromCmd(string args)
        {
            string file = "/home/Shalini/Desktop/SEM7/RC/Prose/prose-20191107T015135Z-001/prose/prose-tutorial4(working_with_learn_multiple_op)/ga.py";
            string result = string.Empty;

            try
            {

                var info = new ProcessStartInfo(@"/usr/bin/python3");
                info.Arguments = file + " " + args;

                info.RedirectStandardInput = false;
                info.RedirectStandardOutput = true;
                info.UseShellExecute = false;
                info.CreateNoWindow = true;

                using (var proc = new Process())
                {
                    proc.StartInfo = info;
                    proc.Start();
                    proc.WaitForExit();
                    if (proc.ExitCode == 0)
                    {
                        result = proc.StandardOutput.ReadToEnd();
                    }                    
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("R Script failed: " + result, ex);
            }
        }

        public static char[] removeChars(char[] c){
            char[] new_c = new char[(c.Length-2)/3];
            int j = 0;
            for(int i = 0; i < c.Length; i++){
                if(c[i] == '+'|| c[i] == '-' || c[i] == '*' || c[i] == '/')
                {
                    new_c[j] = c[i];
                    ++j;
                }
            }
            return new_c;
        }
        public static List<char[]> GA(int [] x, int output)
        {
            string arg = string.Join(" ", x.Select(i => i.ToString()).ToArray());
            arg = output.ToString() +  " " + arg;
            //Console.WriteLine(arg);
            string res = RunFromCmd(arg);
            string tuples = res.Split('[', ']')[1];
            if(tuples.Length == 0)
                return null;
            Console.WriteLine("Tuples:{0}", tuples);
            List<string> result = tuples.Split('(', ')').ToList();
            result.RemoveAll(r => r == "" || r == ", ");
            List<char[]> sequences = new List<char[]>();
            foreach(string s in result){
            //    Console.WriteLine("String: {0}", s);
                char[] c = s.ToCharArray();
                char[] new_c = removeChars(c);
                //Console.WriteLine("CHARS REMOVED");
                //Console.WriteLine(new_c);
                sequences.Add(new_c);
            }
            //Console.WriteLine("Seq made");
            //foreach(char[] c in sequences){
            //    Console.WriteLine(c);
           //}
            if(sequences.Count != 0)
                return sequences;
            return null;
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
                //    Console.WriteLine(x);
                }
                int output = Int32.Parse((example.Value as string).ToString());
                // Console.WriteLine("After op {0}", output);

                List<char[]> final = GA(x, output);
                //Console.WriteLine("GA LEARNED");
                if(final == null)
                    return null;
                result[inputState] = final;
                //Console.WriteLine(final.Count);
                // foreach(char[] f in final){
                //     Console.WriteLine(f.Length);
                // }
            }
            // Console.WriteLine("Exiting");
            //ExampleSpec e = new ExampleSpec(result);
            // Console.WriteLine("ExampleSpec(result): {0}", e);
            return new DisjunctiveExamplesSpec(result);
        }
    }
}