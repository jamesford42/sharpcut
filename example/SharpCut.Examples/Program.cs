using SharpCut.Examples.Base.Base;
using SharpCut.Examples.ExplicitImpl;
using SharpCut.Examples.PrivateField;

namespace SharpCut.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            BaseBase.Main();
            AccessPrivateField.Main();
            CallExplicitImpl.Main();
        }
    }
}
