using Flee.CalcEngine.PublicTypes;
using Flee.PublicTypes;

using System;
using System.Diagnostics;

using Yale.Engine;

namespace Yale.Performance
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var runs = 50000;

            YalePerformance(runs);
            GC.Collect();
            FleePerformance(runs);
        }

        private static void FleePerformance(int runs)
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
                if (i % 5000 == 0) Console.WriteLine($"Runs {i} Time {watch.Elapsed}. {i / (watch.ElapsedMilliseconds / 1000)} parse/sec ");

                var newKey = i.ToString("X");
                engine.Add($"key{newKey}", $"{i} + key{key} + x", context);
                key = newKey;
            }
            Console.WriteLine($"Runs {runs} Time {watch.Elapsed}. Total: {runs / (watch.ElapsedMilliseconds / 1000)} parse/sec ");
        }

        private static void YalePerformance(int runs)
        {
            var instance = new ComputeInstance(new ComputeInstanceOptions{ AutoRecalculate = false });
            instance.Variables.Add("x", 1);

            var watch = Stopwatch.StartNew();
            var key = 0.ToString("X");
            instance.AddExpression<int>($"key{key}", $"1");
            for (var i = 1; i < runs; i++)
            {
                if (i % 5000 == 0) Console.WriteLine($"Runs {i} Time {watch.Elapsed}. {i / (watch.ElapsedMilliseconds / 1000)} parse/sec ");
                var newKey = i.ToString("X");
                instance.AddExpression<int>($"key{newKey}", $"{i} + key{key} + x");
                key = newKey;
            }
            Console.WriteLine($"Runs {runs} Time {watch.Elapsed}. Total: {runs / (watch.ElapsedMilliseconds / 1000)} parse/sec ");
        }
    }
}