#region License

/*
 * Copyright (C) 2009 the original author or authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#endregion

using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace SharpCut.PerformanceTest
{
    /// <summary>
    /// Compare performance between a) Direct method call. b) Reflection Invoke
    /// c) Delegate call.
    /// </summary>
    /// <author>Kenneth Xu</author>
    static class Program
    {
        const int loop = 100000000;
        const string methodName = "PerfTest";

        static void Main(string[] args)
        {
            Console.WriteLine("===== First  Round =====");
            PerformanceTest();
            Console.WriteLine("===== Second Round =====");
            PerformanceTest();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        public static void PerformanceTest()
        {
            DirectCallPerformanceTest();

            RegularDelegatePerformanceTest();

            MethodInfoInvokePerformanceTest();

            MethodInfoDelegatePerformanceTest();

            DynamicMethodInvokePerformanceTest();

            DynamicMethodDelegatePerformanceTest();
        }

        private static void RegularDelegatePerformanceTest()
        {
            Base b = new Sub();
            DelegatePerformanceTest("Regular Delegate", b.PerfTest);
        }

        private static void MethodInfoDelegatePerformanceTest()
        {
            var methodInfoDelegate = new Sub().GetInstanceInvoker<Func<int, object, int>>(methodName);
            DelegatePerformanceTest("MethodInfo Delegate", methodInfoDelegate);
        }

        private static void DynamicMethodDelegatePerformanceTest()
        {
            var dynamicMethodDelegate = new Sub().GetNonVirtualInvoker<Func<int, object, int>>(typeof(Base), methodName);
            DelegatePerformanceTest("DynamicMethod Delegate", dynamicMethodDelegate);
        }

        private static void DirectCallPerformanceTest()
        {
            Base sub = new Sub();
            object o = new object();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = loop; i > 0; i--) sub.PerfTest(0, o);
            stopwatch.Stop();
            WriteResult("Direct Virtual Call", stopwatch.ElapsedMilliseconds, loop);
        }

        private static void MethodInfoInvokePerformanceTest()
        {
            object o = new object();
            Sub sub = new Sub();
            MethodInfo methodInfo = sub.GetType().GetMethod(methodName);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = loop / 1000; i > 0; i--)
                methodInfo.Invoke(sub, new object[] {1, o});
            stopwatch.Stop();
            WriteResult("MethodInfo.Invoke", stopwatch.ElapsedMilliseconds, loop/1000);
        }

        private static void DynamicMethodInvokePerformanceTest()
        {
            Base sub = new Sub();
            DynamicMethod dynamicMethod = Utils.CreateDynamicMethod(typeof(Base).GetMethod(methodName));
            object o = new object();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = loop / 1000; i > 0; i--)
                dynamicMethod.Invoke(null, new object[] {sub, 1, o});
            stopwatch.Stop();
            WriteResult("DynamicMethod.Invoke", stopwatch.ElapsedMilliseconds, loop/1000);
        }

        private static void DelegatePerformanceTest(string testName, Func<int, object, int> callDelegate)
        {
            object o = new object();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = loop; i > 0; i--) callDelegate(1, o);
            stopwatch.Stop();
            WriteResult(testName, stopwatch.ElapsedMilliseconds, loop);
        }

        private static void WriteResult(string callType, double milliSeconds, int count)
        {
            double nsPerCall = milliSeconds*(1000000/(double)count);
            var s = string.Format("{0,-22}: {1,10:#,##0.000}ns", callType, nsPerCall);
            Console.Out.WriteLine(s);
        }

        private class Base
        {
            public virtual int PerfTest(int i, object o) { return 0; }
        }

        private class Sub : Base
        {
            public override int PerfTest(int i, object o) { return 1; }
        }

    }
}
