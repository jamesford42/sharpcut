using System;

namespace SharpCut.Examples.ExplicitImpl
{
    public class CallExplicitImpl
    {
        public static void Main()
        {
            Console.WriteLine("========== call explicit implementation ========");
            new Child().Foo();
        }
    }
}