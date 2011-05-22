using System;

namespace SharpCut.Examples.PrivateField
{
    public class AccessPrivateField
    {
        private static Accessor<Shammer, int> _realAge =
            typeof (Shammer).GetInstanceFieldAccessor<Shammer, int>("_age");

        public static void Main()
        {
            Console.WriteLine("============= private field access =============");
            var shammer = new Shammer();
            Console.WriteLine("Shammer tells his age: " + shammer.HowOldAreYou());
            Console.WriteLine("Shammer's real age by reflection: " + _realAge[shammer]);
            Console.WriteLine("Set shammer's real age by reflection to: " + 45);
            _realAge[shammer] = 45;
            Console.WriteLine("Shammer now tells his age: " + shammer.HowOldAreYou());
        }
    }
}