namespace SharpCut.Examples.Base.Base
{
    public class Parent : GrandParent
    {
        public override int Foo(int a, int b)
        {
            return a + b;
        }
    }
}