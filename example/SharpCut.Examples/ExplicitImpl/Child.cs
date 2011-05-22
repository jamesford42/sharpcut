using System;

namespace SharpCut.Examples.ExplicitImpl
{
    public class Child : Parent, IBar
    {
        private static readonly Action<Parent> _parentFoo =
            typeof(Parent).GetNonVirtualInvoker<Action<Parent>>(typeof(IBar).FullName + ".Foo");

        public void Foo()
        {
            Console.WriteLine("Doing work in Child.");
            _parentFoo(this);
        }
    }
}