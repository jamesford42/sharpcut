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
using System.Reflection;
using System.Reflection.Emit;

namespace SharpCut.Inception
{
    /// <summary>
    /// http://kennethxu.blogspot.com/2009/05/strong-typed-high-performance.html
    /// </summary>
    /// <author>Kenneth Xu</author>
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("C.SayHello() = " + new C().SayHello("World"));
            Console.ReadKey();
        }

        public static DynamicMethod CreateNonVirtualDynamicMethod(MethodInfo method)
        {
            int offset = (method.IsStatic ? 0 : 1);
            var parameters = method.GetParameters();
            int size = parameters.Length + offset;
            Type[] types = new Type[size];
            if (offset > 0) types[0] = method.DeclaringType;
            for (int i = offset; i < size; i++)
            {
                types[i] = parameters[i - offset].ParameterType;
            }

            DynamicMethod dynamicMethod = new DynamicMethod(
                "NonVirtualInvoker_" + method.Name, method.ReturnType, types, method.DeclaringType);
            ILGenerator il = dynamicMethod.GetILGenerator();
            for (int i = 0; i < types.Length; i++) il.Emit(OpCodes.Ldarg, i);
            il.EmitCall(OpCodes.Call, method, null);
            il.Emit(OpCodes.Ret);
            return dynamicMethod;
        }

        public static TDelegate GetNonVirtualMethod<TDelegate>(this MethodInfo method)
            where TDelegate : class 
        {
            var dynamicMethod = CreateNonVirtualDynamicMethod(method);
            return dynamicMethod.CreateDelegate(typeof(TDelegate)) as TDelegate;
        }

        public static TDelegate GetNonVirtualMethod<TDelegate>(this Type type, string name)
            where TDelegate : class 
        {
            Type delegateType = typeof(TDelegate);
            if (!typeof(MulticastDelegate).IsAssignableFrom(delegateType))
            {
                throw new InvalidOperationException(
                    "Expecting type parameter to be a Delegate type, but got " +
                    delegateType.FullName);
            }
            var invoke = delegateType.GetMethod("Invoke");
            ParameterInfo[] parameters = invoke.GetParameters();
            int size = parameters.Length - 1;
            Type[] types = new Type[size];
            for (int i = 0; i < size; i++)
            {
                types[i] = parameters[i + 1].ParameterType;
            }
            var method = type.GetMethod(name,
                                        BindingFlags.Public | BindingFlags.NonPublic |
                                        BindingFlags.Instance | BindingFlags.InvokeMethod,
                                        null, types, null);
            if (method == null) return default(TDelegate);
            var dynamicMethod = CreateNonVirtualDynamicMethod(method);
            return dynamicMethod.CreateDelegate(delegateType) as TDelegate;
        }
    }

    class A { public virtual string SayHello(string name) { return "Hello " + name + " -A"; } }

    class B : A { public override string SayHello(string name) { return "Hi " + name + " -B"; } }

    class C : B
    {
        private static readonly Func<A, string, string> baseBaseFoo =
            typeof(A).GetNonVirtualMethod<Func<A, string, string>>("SayHello");
        public override string SayHello(string name) { return baseBaseFoo(this, name); }
    }
}
