using System;

namespace SharpCut.Examples.Base.Base
{
    public class Child : Parent
    {
        static readonly Func<GrandParent, int, int, int> _grandFoo = 
            typeof (GrandParent).GetNonVirtualInvoker<Func<GrandParent, int, int, int>>("Foo");
        
        public override int Foo(int a, int b)
        {
            // return base.base.Foo(a, b) + 1; // this won't compile!
            return _grandFoo(this, a, b) + 1;
        }
    }
}