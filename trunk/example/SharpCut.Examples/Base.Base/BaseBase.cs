using System;

namespace SharpCut.Examples.Base.Base
{
    public class BaseBase
    {
        public static void Main()
        {
            var grandParent = new GrandParent();
            var parent = new Parent();
            var child = new Child();

            Console.WriteLine("================== base.base ===================");

            Console.WriteLine("Grand Parent Foo(5, 6) is 5*6=" + grandParent.Foo(5, 6));
            Console.WriteLine("Parent       Foo(5, 6) is 5+6=" + parent.Foo(5, 6));
            Console.WriteLine("Child        Foo(5, 6) is base.base.Foo(5,6)+1=" + child.Foo(5, 6));
        }
    }
}