using System;
using System.Collections.Generic;
using System.Text;

namespace SharpCut.CachePerformance
{
    class Program
    {
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
        static readonly Random random = new Random();
        static int sampleSize = 10;
        static private string[] _names;
        static private Dictionary<string, int> _nameMap;


        public static void Main(string[] args)
        {
            if (args.Length > 0) sampleSize = int.Parse(args[0]);
            SetUp();
            int loopCount = 10000000;
            Console.WriteLine(CountTime(() => MapLookupTest(loopCount)));
            Console.WriteLine(CountTime(() => ArrayLookupTest(loopCount)));
            Console.ReadKey();
        }

        static void SetUp()
        {
            string[] names = new string[sampleSize];
            Dictionary<string, int> nameMap = new Dictionary<string, int>(sampleSize*3);
            for (int i = sampleSize - 1; i >= 0; i--)
            {
                var s = MakeRandomString();
                names[i] = s;
                nameMap[s] = i;
                nameMap[s.ToLower()] = i;
                nameMap[s.ToUpper()] = i;
            }
            _names = names;
            _nameMap = nameMap;
        }

        static void MapLookupTest(int loopCount)
        {
            for (int i = loopCount - 1; i >= 0; i-=sampleSize)
            {
                foreach (string s in _names)
                {
                    int result;
                    if (_nameMap.Count > 15)
                    {
                         result = _nameMap[s];
                    }
                    else
                    {
                        foreach (KeyValuePair<string, int> map in _nameMap)
                        {
                            if (map.Key.Equals(s))
                            {
                                result = map.Value;
                                break;
                            }
                        }
                    }
                }
            }
        }

        static void ArrayLookupTest(int loopCount)
        {
            int count = _names.Length;
            for (int i = loopCount - 1; i >= 0; i -= sampleSize)
            {
                foreach (string s in _names)
                {
                    int result;
                    for (int j = 0; j < count; j++)
                    {
                        if (s.Equals(_names[j], StringComparison.OrdinalIgnoreCase))
                        {
                            result = j;
                            break;
                        }
                    }
                }
            }
        }

        static TimeSpan CountTime(Action action)
        {
            DateTime start = DateTime.Now;
            action();
            return DateTime.Now - start;
        }

        static string MakeRandomString()
        {
            return MakeRandomString(random.Next(5, 20));
        }

        static string MakeRandomString(int size)
        {
            StringBuilder sb = new StringBuilder(size);
            for (int i = size - 1; i >= 0; i--)
            {
                sb.Append(chars[random.Next(chars.Length)]);
            }
            return sb.ToString();
        }
    }
}
