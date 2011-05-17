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
using NUnit.Framework;

namespace SharpCut.UnitTests.Method
{
    /// <summary>
    /// Test cases for <see cref="Reflections"/>
    /// </summary>
    /// <author>Kenneth Xu</author>
    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))] 
    public class MethodTest<T>
    {
        [Test] public void GetStaticInvokerOrNull_ReturnsNull_WhenParameterMisMatch()
        {
            Assert.IsNull(typeof (Base).GetStaticInvokerOrNull<Action<string, MethodTest<T>>>("PublicStatic"));
        }

        [Test] public void GetStaticInvoker_Chokes_WhenParameterMisMatch()
        {
            var e = Assert.Throws<MissingMethodException>(
                () => typeof (Base).GetStaticInvoker<Action<string, MethodTest<T>>>("PublicStatic"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains("PublicStatic", e.Message);
            StringAssert.Contains(typeof(string).ToString(), e.Message);
            StringAssert.Contains(typeof(MethodTest<T>).ToString(), e.Message);
        }

        [Test] public void GetStaticInvokerOrNull_ReturnsNull_WhenReturnTypeMisMatch()
        {
            Assert.IsNull(typeof (Base).GetStaticInvokerOrNull<Func<T, double>>("PublicStatic"));
        }

        [Test] public void GetStaticInvokerOrNull_Chokes_WhenReturnTypeMisMatch()
        {
            var e = Assert.Throws<MissingMethodException>(
                () => typeof(Base).GetStaticInvoker<Func<T, double>>("PublicStatic"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains("PublicStatic", e.Message);
            StringAssert.Contains(typeof(T).ToString(), e.Message);
            StringAssert.Contains(typeof(double).ToString(), e.Message);
        }

        [Test] public void GetInstanceInvokerOrNull_ReturnsNull_WhenParameterMisMatch()
        {
            Assert.IsNull(typeof(Base).GetInstanceInvokerOrNull<Action<Base, string, MethodTest<T>>>("PublicInstance"));
            Assert.IsNull(new Base().GetInstanceInvokerOrNull<Action<string, MethodTest<T>>>("PublicInstance"));
        }

        [Test] public void GetInstanceInvoker_Chokes_WhenParameterMisMatch()
        {
            var e = Assert.Throws<MissingMethodException>(
                () => typeof(Base).GetInstanceInvoker<Action<Base, string, MethodTest<T>>>("PublicInstance"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains("PublicInstance", e.Message);
            StringAssert.Contains(typeof(string).ToString(), e.Message);
            StringAssert.Contains(typeof(MethodTest<T>).ToString(), e.Message);

            e = Assert.Throws<MissingMethodException>(
                () => new Base().GetInstanceInvoker<Action<string, MethodTest<T>>>("PublicInstance"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains("PublicInstance", e.Message);
            StringAssert.Contains(typeof(string).ToString(), e.Message);
            StringAssert.Contains(typeof(MethodTest<T>).ToString(), e.Message);
        }

        [Test] public void GetInstanceInvokerOrNull_ReturnsNull_WhenReturnTypeMisMatch()
        {
            Assert.IsNull(typeof(Base).GetInstanceInvokerOrNull<Func<Base, double>>("PublicInstance"));
            Assert.IsNull(new Base().GetInstanceInvokerOrNull<Func<double>>("PublicInstance"));
        }

        [Test] public void GetInstanceInvoker_Chokes_WhenReturnTypeMisMatch()
        {
            var e = Assert.Throws<MissingMethodException>(
                () => typeof(Base).GetInstanceInvoker<Func<Base, double>>("PublicInstance"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains("PublicInstance", e.Message);
            StringAssert.Contains(typeof(T).ToString(), e.Message);
            StringAssert.Contains(typeof(double).ToString(), e.Message);

            e = Assert.Throws<MissingMethodException>(
                () => new Base().GetInstanceInvoker<Func<double>>("PublicInstance"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains("PublicInstance", e.Message);
            StringAssert.Contains(typeof(T).ToString(), e.Message);
            StringAssert.Contains(typeof(double).ToString(), e.Message);
        }

        [Test] public void GetNonVirtualInvokerOrNull_ReturnsNull_WhenParameterMisMatch()
        {
            Assert.IsNull(typeof(Base).GetNonVirtualInvokerOrNull<Action<Base, string, MethodTest<T>>>("PublicVirtualInstance"));
            Assert.IsNull(new Base().GetNonVirtualInvokerOrNull<Action<string, MethodTest<T>>>(typeof(Base), "PublicVirtualInstance"));
        }

        [Test] public void GetNonVirtualInvoker_Chokes_WhenParameterMisMatch()
        {
            var e = Assert.Throws<MissingMethodException>(
                () => typeof(Base).GetNonVirtualInvoker<Action<Base, string, MethodTest<T>>>("PublicVirtualInstance"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains("PublicVirtualInstance", e.Message);
            StringAssert.Contains(typeof(string).ToString(), e.Message);
            StringAssert.Contains(typeof(MethodTest<T>).ToString(), e.Message);

            e = Assert.Throws<MissingMethodException>(
                () => new Base().GetNonVirtualInvoker<Action<Base, string, MethodTest<T>>>(typeof(Base), "PublicVirtualInstance"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains("PublicVirtualInstance", e.Message);
            StringAssert.Contains(typeof(string).ToString(), e.Message);
            StringAssert.Contains(typeof(MethodTest<T>).ToString(), e.Message);
        }

        [Test] public void GetNonVirtualInvokerOrNull_ReturnsNull_WhenReturnTypeMisMatch()
        {
            Assert.IsNull(typeof(Base).GetNonVirtualInvokerOrNull<Func<Base, T, double>>("PublicVirtualInstance"));
            Assert.IsNull(new Base().GetNonVirtualInvokerOrNull<Func<T, double>>(typeof(Base), "PublicVirtualInstance"));
        }

        [Test] public void GetNonVirtualInvoker_Chokes_WhenReturnTypeMisMatch()
        {
            var e = Assert.Throws<MissingMethodException>(
                () => typeof(Base).GetNonVirtualInvoker<Func<Base, T, double>>("PublicVirtualInstance"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains("PublicVirtualInstance", e.Message);
            StringAssert.Contains(typeof(T).ToString(), e.Message);
            StringAssert.Contains(typeof(double).ToString(), e.Message);

            e = Assert.Throws<MissingMethodException>(
                () => new Base().GetNonVirtualInvoker<Func<T, double>>(typeof(Base), "PublicVirtualInstance"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains("PublicVirtualInstance", e.Message);
            StringAssert.Contains(typeof(T).ToString(), e.Message);
            StringAssert.Contains(typeof(double).ToString(), e.Message);
        }

        [Test] public void GetStaticInvokerOrNull_Chokes_OnNonDelegateGenericParameter()
        {
            var e = Assert.Throws<InvalidOperationException>(
                () => typeof (Base).GetStaticInvokerOrNull<MethodTest<T>>("PublicStatic"));
            StringAssert.Contains("Delegate type", e.Message);
            StringAssert.Contains(GetType().FullName, e.Message);
            Assert.Throws<InvalidOperationException>(
                () => typeof(Base).GetStaticInvoker<MethodTest<T>>("PublicStatic"));

        }

        [Test] public void GetInstanceInvokerOrNull_Chokes_OnNonDelegateGenericParameter()
        {
            Assert.Throws<InvalidOperationException>(
                () => typeof(Base).GetInstanceInvokerOrNull<MethodTest<T>>("PublicInstance"));
            Assert.Throws<InvalidOperationException>(
                () => typeof(Base).GetInstanceInvoker<MethodTest<T>>("PublicInstance"));

            Assert.Throws<InvalidOperationException>(
                () => new Base().GetInstanceInvokerOrNull<MethodTest<T>>("PublicInstance"));
            Assert.Throws<InvalidOperationException>(
                () => new Base().GetInstanceInvoker<MethodTest<T>>("PublicInstance"));

        }

        [Test] public void GetNonVirtualInvokerOrNull_Chokes_OnNonDelegateGenericParameter()
        {
            Assert.Throws<InvalidOperationException>(
                () => typeof(Base).GetNonVirtualInvokerOrNull<MethodTest<T>>("PublicVirtualInstance"));
            Assert.Throws<InvalidOperationException>(
                () => typeof(Base).GetNonVirtualInvoker<MethodTest<T>>("PublicVirtualInstance"));

            Assert.Throws<InvalidOperationException>(
                () => new Base().GetNonVirtualInvokerOrNull<MethodTest<T>>(typeof(Base), "PublicVirtualInstance"));
            Assert.Throws<InvalidOperationException>(
                () => new Base().GetNonVirtualInvoker<MethodTest<T>>(typeof(Base), "PublicVirtualInstance"));
        }

        [Test] public void GetInstanceInvokerByType_Chokes_OnNoParameterDelegate()
        {
            var e = Assert.Throws<InvalidOperationException>(
                () => typeof(Base).GetInstanceInvokerOrNull<Action>("PublicInstance"));
            StringAssert.Contains(typeof(Action).ToString(), e.Message);
            e = Assert.Throws<InvalidOperationException>(
                () => typeof(Base).GetInstanceInvoker<Action>("PublicInstance"));
            StringAssert.Contains(typeof(Action).ToString(), e.Message);
        }

        [Test] public void GetNonVirtualInvokerByType_Chokes_OnNoParameterDelegate()
        {
            var e = Assert.Throws<InvalidOperationException>(
                () => typeof(Base).GetNonVirtualInvokerOrNull<Action>("PublicVirtualInstance"));
            StringAssert.Contains(typeof(Action).ToString(), e.Message);
            e = Assert.Throws<InvalidOperationException>(
                () => typeof(Base).GetNonVirtualInvoker<Action>("PublicVirtualInstance"));
            StringAssert.Contains(typeof(Action).ToString(), e.Message);
        }

        [Test] public void GetInstanceInvokerByType_ReturnsNull_WhenFirstParameterMismatch()
        {
            Assert.IsNull(typeof(Base).GetInstanceInvokerOrNull<Func<string, T, string, int, T>>("PublicInstance"));
        }

        [Test] public void GetInstanceInvokerByType_Chokes_WhenFirstParameterMismatch()
        {
            var e = Assert.Throws<MissingMethodException>(
                () => typeof(Base).GetInstanceInvoker<Func<string, T, string, int, T>>("PublicInstance"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains(typeof(string).ToString(), e.Message);
        }

        [Test] public void GetNonVirtualInvokerByType_ReturnsNull_WhenFirstParameterMismatch()
        {
            Assert.IsNull(typeof(Base).GetNonVirtualInvokerOrNull<Func<string, object, T, object>>("PublicVirtualInstance"));
        }

        [Test] public void GetNonVirtualInvokerByType_Chokes_WhenFirstParameterMismatch()
        {
            var e = Assert.Throws<MissingMethodException>(
                () => typeof(Base).GetNonVirtualInvoker<Func<string, object, T, object>>("PublicVirtualInstance"));
            StringAssert.Contains(typeof(Base).ToString(), e.Message);
            StringAssert.Contains(typeof(string).ToString(), e.Message);
        }

        [Test] public void GetStaticInvokerOrNull_InvokesPublicMethod()
        {
            T value = (T) Convert.ChangeType(1212456, typeof(T));
            var d = typeof (Base).GetStaticInvokerOrNull<Action<T>>("PublicStatic");
            Base.PublicStatic(value);
            string expected = Base.JustCalled;
            Base.JustCalled = null;
            d(value);
            Assert.AreEqual(expected, Base.JustCalled);
        }

        [Test] public void GetStaticInvoker_InvokesPrivateMethod()
        {
            T value = (T)Convert.ChangeType(1212456, typeof(T));
            var d = typeof(Base).GetStaticInvoker<Action<T>>("PrivateStatic");
            string expected = string.Format("PrivateStatic({0})", value);
            Base.JustCalled = null;
            d(value);
            Assert.AreEqual(expected, Base.JustCalled);
        }

        [Test] public void GetInstanceInvokerByType_InvokesSubPublicVirtualMethod()
        {
            Base b = new Sub();
            T value = (T)Convert.ChangeType(1212456, typeof(T));
            var d = typeof(Base).GetInstanceInvokerOrNull<Action<Base, T>>("PublicVirtualInstance");
            b.PublicVirtualInstance(value);
            string expected = Base.JustCalled;
            Base.JustCalled = null;
            d(b, value);
            Assert.AreEqual(expected, Base.JustCalled);
        }

        [Test] public void GetInstanceInvokerByType_InvokesBaseInternalMethod()
        {
            Base b = new Sub();
            T result = (T)Convert.ChangeType(384745, typeof(T));
            T value = (T)Convert.ChangeType(1212456, typeof(T));
            var d = typeof(Base).GetInstanceInvoker<Func<Base, T, string, int, T>>("InternalInstance");
            b.InternalInstance(value, "b", 88);
            string expected = Base.JustCalled;
            Base.JustCalled = null;
            Base.WillReturnT = result;
            Assert.AreEqual(result, d(b, value, "b", 88));
            Assert.AreEqual(expected, Base.JustCalled);
        }

        [Test] public void GetInstanceInvokerByInstance_InvokesSubPublicMethod()
        {
            Base b = new Sub();
            T result = (T)Convert.ChangeType(384745, typeof(T));
            T value = (T)Convert.ChangeType(1212456, typeof(T));
            var d = b.GetInstanceInvokerOrNull<Func<T>>("PublicInstance");
            (new Sub()).PublicInstance();
            string expected = Sub.JustCalled;
            Sub.JustCalled = null;
            Sub.WillSubReturnT = result;
            Assert.AreEqual(result, d());
            Assert.AreEqual(expected, Base.JustCalled);
        }

        [Test] public void GetInstanceInvokerByInstance_InvokesSubInternalVirtualMethod()
        {
            Base b = new Sub();
            object result = "result";
            T value = (T)Convert.ChangeType(1212456, typeof(T));
            var d = b.GetInstanceInvoker<Func<object, T, object>>("InternalProtectedVirtualInstance");
            b.InternalProtectedVirtualInstance("b", value);
            string expected = Sub.JustCalled;
            Sub.JustCalled = null;
            Sub.WillSubReturnObject = result;
            Assert.AreEqual(result, d("b", value));
            Assert.AreEqual(expected, Base.JustCalled);
        }

        [Test] public void GetNonVirtualInvokerByType_InvokesBasePublicVirtualMethod()
        {
            Base b = new Sub();
            object result = "result";
            T value = (T)Convert.ChangeType(1212456, typeof(T));
            var d = typeof(Base).GetNonVirtualInvokerOrNull<Func<Base, object, T, object>>("PublicVirtualInstance");
            new Base().PublicVirtualInstance("object", value);
            string expected = Base.JustCalled;
            Base.JustCalled = null;
            Base.WillReturnObject = result;
            Assert.AreEqual(result, d(b, "object", value));
            Assert.AreEqual(expected, Base.JustCalled);
        }

        [Test] public void GetNonVirtualInvokerByType_InvokesBaseInternalMethod()
        {
            Base b = new Sub();
            T result = (T)Convert.ChangeType(384745, typeof(T));
            T value = (T)Convert.ChangeType(1212456, typeof(T));
            var d = typeof(Base).GetNonVirtualInvoker<Func<Base, T, string, int, T>>("InternalInstance");
            b.InternalInstance(value, "b", 88);
            string expected = Base.JustCalled;
            Base.JustCalled = null;
            Base.WillReturnT = result;
            Assert.AreEqual(result, d(b, value, "b", 88));
            Assert.AreEqual(expected, Base.JustCalled);
        }

        [Test] public void GetNonVirtualInvokerByInstance_InvokesBasePublicMethod()
        {
            Base b = new Sub();
            T result = (T)Convert.ChangeType(384745, typeof(T));
            var d = b.GetNonVirtualInvokerOrNull<Func<T>>(typeof(Base), "PublicInstance");
            b.PublicInstance();
            string expected = Base.JustCalled;
            Base.JustCalled = null;
            Base.WillReturnT = result;
            Assert.AreEqual(result, d());
            Assert.AreEqual(expected, Base.JustCalled);
        }

        [Test] public void GetNonVirtualInvokerByInstance_InvokesBaseInternalVirtualMethod()
        {
            Base b = new Sub();
            object result = "result";
            T value = (T)Convert.ChangeType(1212456, typeof(T));
            var d = b.GetNonVirtualInvoker<Func<object, T, object>>(typeof(Base), "InternalProtectedVirtualInstance");
            new Base().InternalProtectedVirtualInstance("b", value);
            string expected = Base.JustCalled;
            Base.JustCalled = null;
            Base.WillReturnObject = result;
            Assert.AreEqual(result, d("b", value));
            Assert.AreEqual(expected, Base.JustCalled);
        }

        [Test] public void GetInstanceInvokerByType_InvokesSub_OnLoseParameterMatch()
        {
            var d = typeof(Base).GetInstanceInvoker<LostParameterByTypeDelegate>("PublicVirtualInstance");
            Sub s1 = new Sub();
            Sub s2 = new Sub();
            int intout;
            s1.PublicVirtualInstance(3445, "objectb", s2, out intout);
            string expected = Sub.JustCalled;
            int expectedOut = intout;
            Sub.JustCalled = null;
            var result = d(s1, 3445, "objectb", s2, out intout);
            Assert.AreSame(s1, result);
            Assert.AreEqual(expected, Sub.JustCalled);
            Assert.AreEqual(expectedOut, intout);
        }

        [Test] public void GetInstanceInvokerByInstance_InvokesSub_OnLoseParameterMatch()
        {
            Sub s1 = new Sub();
            Sub s2 = new Sub();
            var d = s1.GetInstanceInvoker<LostParameterByInstanceDelegate>("PublicVirtualInstance");
            int intout;
            s1.PublicVirtualInstance(3445, "objectb", s2, out intout);
            string expected = Sub.JustCalled;
            int expectedOut = intout;
            Sub.JustCalled = null;
            var result = d(3445, "objectb", s2, out intout);
            Assert.AreSame(s1, result);
            Assert.AreEqual(expected, Sub.JustCalled);
            Assert.AreEqual(expectedOut, intout);
        }

        [Test] public void GetNonVirtualInvokerByType_InvokesSub_OnLoseParameterMatch()
        {
            var d = typeof(Base).GetNonVirtualInvoker<LostParameterByTypeDelegate>("PublicVirtualInstance");
            Sub s1 = new Sub();
            Sub s2 = new Sub();
            int intout;
            new Base().PublicVirtualInstance(3445, "objectb", s2, out intout);
            string expected = Base.JustCalled;
            int expectedOut = intout;
            Base.JustCalled = null;
            var result = d(s1, 3445, "objectb", s2, out intout);
            Assert.AreSame(s1, result);
            Assert.AreEqual(expected, Base.JustCalled);
            Assert.AreEqual(expectedOut, intout);
        }

        [Test] public void GetNonVirtualInvokerByInstance_InvokesSub_OnLoseParameterMatch()
        {
            Sub s1 = new Sub();
            Sub s2 = new Sub();
            var d = s1.GetNonVirtualInvoker<LostParameterByInstanceDelegate>(typeof(Base), "PublicVirtualInstance");
            int intout;
            new Base().PublicVirtualInstance(3445, "objectb", s2, out intout);
            string expected = Sub.JustCalled;
            int expectedOut = intout;
            Sub.JustCalled = null;
            var result = d(3445, "objectb", s2, out intout);
            Assert.AreSame(s1, result);
            Assert.AreEqual(expected, Sub.JustCalled);
            Assert.AreEqual(expectedOut, intout);
        }

        [Test] public void GetInvokerOrNull_FiltersMethod()
        {
            Base b = new Base();

            var dProtected = Reflections.GetInvokerOrNull<Func<T>>(
                b, b.GetType(), "ProtectedVirtualInstance",
                BindingFlags.Instance | BindingFlags.NonPublic, null);
            Assert.NotNull(dProtected);

            var dPrivate = Reflections.GetInvokerOrNull<Action<T>>(
                b, b.GetType(), "PrivateInstance",
                BindingFlags.Instance | BindingFlags.NonPublic, null);
            Assert.NotNull(dPrivate);

            dProtected = Reflections.GetInvokerOrNull<Func<T>>(
                b, b.GetType(), "ProtectedVirtualInstance",
                BindingFlags.Instance | BindingFlags.NonPublic, m=>m.IsFamily);
            Assert.NotNull(dProtected);

            dPrivate = Reflections.GetInvokerOrNull<Action<T>>(
                b, b.GetType(), "PrivateInstance",
                BindingFlags.Instance | BindingFlags.NonPublic, m => m.IsFamily);
            Assert.Null(dPrivate);
        }

        [Test] public void GetInvoker_FiltersMethod()
        {
            Base b = new Base();
            var message = "protected methods only";
            var dProtected = Reflections.GetInvoker<Func<T>>(
                b, b.GetType(), "ProtectedVirtualInstance",
                BindingFlags.Instance | BindingFlags.NonPublic, null, null);
            Assert.NotNull(dProtected);

            var dPrivate = Reflections.GetInvoker<Action<T>>(
                b, b.GetType(), "PrivateInstance",
                BindingFlags.Instance | BindingFlags.NonPublic, null, null);
            Assert.NotNull(dPrivate);

            dProtected = Reflections.GetInvoker<Func<T>>(
                b, b.GetType(), "ProtectedVirtualInstance",
                BindingFlags.Instance | BindingFlags.NonPublic, m=>m.IsFamily, message);
            Assert.NotNull(dProtected);

            var e = Assert.Throws<MissingMethodException>(
                delegate
                    {
                        Reflections.GetInvoker<Action<T>>(
                            b, b.GetType(), "PrivateInstance",
                            BindingFlags.Instance | BindingFlags.NonPublic, m => m.IsFamily, message);
                    });
            StringAssert.Contains(message, e.Message);
        }

        private delegate IDisposable LostParameterByTypeDelegate(Sub o, int a, string b, Sub c, out int d);
        private delegate IDisposable LostParameterByInstanceDelegate(int a, string b, Sub c, out int d);

        internal class Base : IDisposable
        {
            public static string JustCalled { get; set; }
            public static T WillReturnT { get; set; }
            public static object WillReturnObject { get; set; }

            #region Public Static
            public static void PublicStatic(T a)
            {
                JustCalled = string.Format("PublicStatic({0})", a);
            }

            public static T PublicStatic()
            {
                JustCalled = string.Format("PublicStatic()");
                return WillReturnT;
            }

            public static T PublicStatic(T a, string b, int c)
            {
                JustCalled = string.Format("PublicStatic({0}, {1}, {2})", a, b, c);
                return WillReturnT;
            }

            public static object PublicStatic(object a)
            {
                JustCalled = string.Format("PublicStatic({0}", a);
                return WillReturnObject;
            }
            #endregion

            #region Non Public Static
            private static void PrivateStatic(T a)
            {
                JustCalled = string.Format("PrivateStatic({0})", a);
            }

            protected static T ProtectedStatic()
            {
                JustCalled = string.Format("ProtectedStatic()");
                return WillReturnT;
            }

            internal static T InternalStatic(T a, string b, int c)
            {
                JustCalled = string.Format("InternalStatic({0}, {1}, {2})", a, b, c);
                return WillReturnT;
            }

            internal protected static object InternalProtectedStatic(object a, T b)
            {
                JustCalled = string.Format("InternalProtectedStatic({0}, {1}", a, b);
                return WillReturnObject;
            }
            #endregion

            #region Public Instance

            public virtual Base PublicVirtualInstance(int a, object b, Base c, out int d)
            {
                JustCalled = string.Format("PublicVirtualInstance({0}, {1}, {2})", a, b, c);
                d = a + 10;
                return this;
            }
            public virtual void PublicVirtualInstance(T a)
            {
                JustCalled = string.Format("PublicVirtualInstance({0})", a);
            }

            public T PublicInstance()
            {
                JustCalled = string.Format("PublicInstance()");
                return WillReturnT;
            }

            public T PublicInstance(T a, string b, int c)
            {
                JustCalled = string.Format("PublicInstance({0}, {1}, {2})", a, b, c);
                return WillReturnT;
            }

            public virtual object PublicVirtualInstance(object a, T b)
            {
                JustCalled = string.Format("PublicVirtualInstance({0}, {1}", a, b);
                return WillReturnObject;
            }
            #endregion

            #region Non Public Instance
            private void PrivateInstance(T a)
            {
                JustCalled = string.Format("PrivateInstance({0})", a);
            }

            protected virtual T ProtectedVirtualInstance()
            {
                JustCalled = string.Format("ProtectedVirtualInstance()");
                return WillReturnT;
            }

            internal T InternalInstance(T a, string b, int c)
            {
                JustCalled = string.Format("InternalInstance({0}, {1}, {2})", a, b, c);
                return WillReturnT;
            }

            internal protected virtual object InternalProtectedVirtualInstance(object a, T b)
            {
                JustCalled = string.Format("InternalProtectedVirtualInstance({0}, {1}", a, b);
                return WillReturnObject;
            }
            #endregion

            #region IDisposable Members

            public void Dispose()
            {
            }

            #endregion
        }

        internal class Sub : Base
        {
            public static T WillSubReturnT { get; set; }
            public static object WillSubReturnObject { get; set; }

            public override Base PublicVirtualInstance(int a, object b, Base c, out int d)
            {
                JustCalled = string.Format("Sub.PublicVirtualInstance({0}, {1}, {2})", a, b, c);
                d = a + 100;
                return this;
            }

            public override object PublicVirtualInstance(object a, T b)
            {
                JustCalled = string.Format("Sub.PublicVirtualInstance({0}, {1}", a, b);
                return WillSubReturnObject;
            }

            public override void PublicVirtualInstance(T a)
            {
                JustCalled = string.Format("Sub.PublicVirtualInstance({0})", a);
            }

            protected internal override object InternalProtectedVirtualInstance(object a, T b)
            {
                JustCalled = string.Format("Sub.InternalProtectedVirtualInstance({0}, {1}", a, b);
                return WillSubReturnObject;
            }

            protected override T ProtectedVirtualInstance()
            {
                JustCalled = string.Format("Sub.ProtectedVirtualInstance()");
                return WillSubReturnT;
            }

            new public T PublicInstance()
            {
                JustCalled = string.Format("Sub.PublicInstance()");
                return WillSubReturnT;
            }


            new internal T InternalInstance(T a, string b, int c)
            {
                JustCalled = string.Format("Sub.InternalInstance({0}, {1}, {2})", a, b, c);
                return WillSubReturnT;
            }
        }
    }
}