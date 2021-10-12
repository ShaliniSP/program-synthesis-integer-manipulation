using System;
using Microsoft.ProgramSynthesis;
using Microsoft.ProgramSynthesis.AST;
using System.Text.RegularExpressions;
using Microsoft.ProgramSynthesis.Features;

namespace ProseTutorial
{
    public class RankingScore : Feature<double>
    {
        public RankingScore(Grammar grammar) : base(grammar, "Score") { Console.WriteLine("Ranking Score"); }

        [FeatureCalculator(nameof(Semantics.Eval))]
        public static double Eval(double v, double d)
        {
            Console.WriteLine("In Ranking Eval");
            return 1;
        }

        [FeatureCalculator("func", Method = CalculationMethod.FromLiteral)]
        public static double Func(int func)
        {
            return 1;
        }
    }
}