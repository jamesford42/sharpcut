using System;

namespace SharpCut.Examples.ExplicitImpl
{
    public class Parent : IBar
    {
        void IBar.Foo()
        {
            Console.WriteLine("Doing work in Parent class.");
        }
    }
}