using System.Diagnostics;

namespace Leonardo;

public class Fibonacci
{
    public static async Task<IList<int>> RunAsync(string[] args)
    {
        Stopwatch sw = new();
        sw.Start();

        IList<int> results = new List<int>();
        var tasks = new List<Task<int>>();
        
        foreach (var s in args)
        {
            var task = Task.Run(() =>
            {
                var result = Run(int.Parse(s));
                Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");
                return result;
            });
            tasks.Add(task);
        }

        foreach (var task in tasks)
        {
            var result = await task;
            Console.WriteLine($"Result: {result}");
            results.Add(task.Result);
        }
        
        sw.Stop();
        Console.WriteLine($"Total time: {sw.ElapsedMilliseconds} ms");
        
        return results;
    }
    
    private static int Run(int n)
    {
        return n <= 2 ? n : Run(n - 1) + Run(n - 2);
    }
}
