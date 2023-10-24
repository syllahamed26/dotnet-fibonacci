﻿using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace Leonardo;

public class Fibonacci
{
    public static async Task<IList<int>> RunAsync(string[] args)
    {
        await using var context = new FibonacciDataContext();
        
        if (args.Length >= 100)
        {
            throw new ArgumentException("Too many arguments.");
        }
        
        Stopwatch sw = new();
        sw.Start();

        IList<int> results = new List<int>();
        var tasks = new List<Task<int>>();
        
        foreach (var s in args)
        {
            var tFibonacci = await context.TFibonaccis
                .Where(t => t.FibInput == int.Parse(s))
                .FirstOrDefaultAsync();
            if (tFibonacci == null)
            {
                var task = Task.Run(() =>
                {
                    var result = Run(int.Parse(s));
                    Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
                    return result;
                });
                tasks.Add(task);
            }else
            {
                tasks.Add(Task.FromResult((int)tFibonacci.FibOutput));
            }

            foreach (var task in tasks)
            {
                var result = await task;
                
                context.TFibonaccis.Add(new TFibonacci
                {
                    FibInput = int.Parse(args[tasks.IndexOf(task)]),
                    FibOutput = result
                });
                
                Console.WriteLine($"Result: {result}");
                results.Add(task.Result);
            }
            
            sw.Stop();
            Console.WriteLine($"Total time: {sw.ElapsedMilliseconds} ms");
        }
        
        await context.SaveChangesAsync();
        
        return results;
    }
    
    private static int Run(int n)
    {
        return n <= 2 ? n : Run(n - 1) + Run(n - 2);
    }
}
