using Leonardo;

using var context = new FibonacciDataContext();

var results = new Fibonacci(context).RunAsync(args);
results.Wait();