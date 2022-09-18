using Flee.CalcEngine.PublicTypes;
using Flee.PublicTypes;

using System;
using System.Diagnostics;

using Yale.Engine;

namespace Yale.Performance
{
    internal class Program
    {
        private static readonly float runs = 500000;
        private static void Main(string[] args)
        {
            FleePerformance();
            GC.Collect();
            YalePerformance();
        }

        private static void FleePerformance()
        {
            var engine = new CalculationEngine();
            var context = new ExpressionContext();
            var variables = context.Variables;
            variables.Add("x", 1);

            var watch = Stopwatch.StartNew();
            var key = 0.ToString("X");
            engine.Add($"key{key}", $"1", context);
            for (var i = 1; i < runs; i++)
            {
                if (i % 10000 == 0) Console.WriteLine($"Runs {i} Time {watch.Elapsed}. Total ~: {i / watch.ElapsedMilliseconds * 1000} parse/sec ");

                var newKey = i.ToString("X");
                engine.Add($"key{newKey}", $"{i} + key{key} + x", context);
                key = newKey;
            }
            Console.WriteLine($"Runs {runs} Time {watch.Elapsed}. Total avg: {runs / watch.ElapsedMilliseconds * 1000} parse/sec ");
        }

        private static void YalePerformance()
        {
            var instance = new ComputeInstance(new ComputeInstanceOptions { AutoRecalculate = false });
            instance.Variables.Add("x", 1);

            var watch = Stopwatch.StartNew();
            var key = 0.ToString("X");
            instance.AddExpression<int>($"key{key}", $"1");
            for (var i = 1; i < runs; i++)
            {
                if (i % 10000 == 0) Console.WriteLine($"Runs {i} Time {watch.Elapsed}. Total ~: {i / watch.ElapsedMilliseconds * 1000} parse/sec ");

                var newKey = i.ToString("X");
                var op = i % 2 == 0 ? "+" : "-";

                instance.AddExpression<int>($"key{newKey}", $"{i} {op} key{key} + x");
                key = newKey;
            }
            Console.WriteLine($"Runs {runs} Time {watch.Elapsed}. Total avg: {runs / watch.ElapsedMilliseconds * 1000} parse/sec ");
        }
    }
}