namespace SharpCut.Examples.PrivateField
{
    public class Shammer
    {
        private int _age = 40;

        public int HowOldAreYou()
        {
            return _age - 5;
        }
    }
}