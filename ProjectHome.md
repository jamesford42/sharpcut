# Project Description #
Delivers easy to use, high performance and strong typed reflection library in a less than 50K DLL file. Supports .Net 2.0 and above.

# History #

SharpCut was originally [Common.Reflection](http://kennethxu.blogspot.com/2009/05/strong-typed-high-performance_18.html). It's now a separate project that supports constructor, property and field in addition to methods.

# Documentation #
> [Installation](Installation.md)

# Feel It #

Take a look at below typical reflection calls.
```
    private static int Process(object calculator)
    {
        // This is tedious, ugly and error prone.
        MethodInfo method = calculator.GetType().GetMethod(
            "Compute", 
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, 
            null, 
            new Type[]{typeof(int)}, 
            null);

        int result = 0;
        for (int i = 0; i < 10000000; i++)
        {
            // This is tedious, ugly and error prone. And slow...
            result = (int)method.Invoke(null, new object[] { result });
        }
        return result;
    }
```

With SharpCut, the code above can be simplified and speed up as below. You can see that not only SharpCut let you make the reflection invocation strong typed, but also [thousands times faster](http://kennethxu.blogspot.com/2009/05/strong-typed-high-performance_15.html).
```
    private  static int Process(object calculator)
    {
        // Easy, clean and strong typed.
        var method = calculator.GetInstanceInvoker<Func<int, int>>("Compute");
        int result = 0;
        for (int i = 0; i < 10000000; i++)
        {
            // This is simple, clean and as fast as normal method call.
            result = method(result);
        }
        return result;
    }
```