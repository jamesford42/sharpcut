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
using NUnit.Framework;

namespace SharpCut.UnitTests.Property
{
    /// <summary>
    /// Test cases for static property related extension methods in <see cref="Reflections"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>Kenneth Xu</author>
    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))]
    public class StaticPropertyTest<T>
    {
        static readonly T _one = (T)Convert.ChangeType(1, typeof (T));
        static readonly T _two = (T)Convert.ChangeType(2, typeof (T));

        #region getter
        [Test] public void GetStaticPropertyGetterOrNull_WhenNoSuchPropertyName_ReturnsNull()
        {
            var getter = typeof (PropertyBase<T>).GetStaticPropertyGetterOrNull<T>("NoSuchProperty");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetStaticPropertyGetter_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof (PropertyBase<T>).GetStaticPropertyGetter<T>("NoSuchProperty"));
        }

        [Test] public void GetStaticPropertyGetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = typeof(PropertyBase<T>).GetStaticPropertyGetterOrNull<T>("StaticPropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetStaticPropertyGetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = typeof(PropertyBase<T>).GetStaticPropertyGetter<T>("StaticPropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetStaticPropertyGetterOrNull_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = typeof(PropertyBase<T>).GetStaticPropertyGetterOrNull<T>("StaticReadOnlyT");
            AssertGetterT(getter);
        }

        [Test] public void GetStaticPropertyGetter_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = typeof(PropertyBase<T>).GetStaticPropertyGetter<T>("StaticReadOnlyT");
            AssertGetterT(getter);
        }

        [Test] public void GetStaticPropertyGetterOrNull_WhenWriteOnly_ReturnsNull()
        {
            var getter = typeof(PropertyBase<T>).GetStaticPropertyGetterOrNull<T>("StaticWriteOnlyT");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetStaticPropertyGetter_WhenWriteOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetStaticPropertyGetter<T>("StaticWriteOnlyT"));
        }

        [Test] public void GetStaticPropertyGetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = typeof(PropertyBase<T>).GetStaticPropertyGetterOrNull<object>("StaticPropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetStaticPropertyGetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = typeof(PropertyBase<T>).GetStaticPropertyGetter<object>("StaticPropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetStaticPropertyGetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var getter = typeof(PropertyBase<T>).GetStaticPropertyGetterOrNull<T>("StaticPropertyObject");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetStaticPropertyGetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetStaticPropertyGetter<T>("StaticPropertyObject"));
        }

        private static void AssertGetterT(Func<object> getter)
        {
            PropertyBase<T>.StaticPropertyT = _one;
            Assert.That(getter(), Is.EqualTo(_one));
            PropertyBase<T>.StaticPropertyT = _two;
            Assert.That(getter(), Is.EqualTo(_two));
        }

        private static void AssertGetterT(Func<T> getter)
        {
            AssertGetterT(new Func<object>(()=>getter()));
        }
        #endregion getter

        #region setter
        [Test] public void GetStaticPropertySetterOrNull_WhenNoSuchPropertyName_ReturnsNull()
        {
            var setter = typeof(PropertyBase<T>).GetStaticPropertySetterOrNull<T>("NoSuchProperty");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetStaticPropertySetter_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetStaticPropertySetter<T>("NoSuchProperty"));
        }

        [Test] public void GetStaticPropertySetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = typeof(PropertyBase<T>).GetStaticPropertySetterOrNull<T>("StaticPropertyT");
            AssertSetterT(setter);
        }

        [Test] public void GetStaticPropertySetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = typeof(PropertyBase<T>).GetStaticPropertySetter<T>("StaticPropertyT");
            AssertSetterT(setter);
        }

        [Test] public void GetStaticPropertySetterOrNull_WhenReadOnly_ReturnsNull()
        {
            var setter = typeof(PropertyBase<T>).GetStaticPropertySetterOrNull<T>("StaticReadOnlyT");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetStaticPropertySetter_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetStaticPropertySetter<T>("StaticReadOnlyT"));
        }

        [Test] public void GetStaticPropertySetterOrNull_WhenWriteOnly_ReturnsWorkingDelegate()
        {
            var setter = typeof(PropertyBase<T>).GetStaticPropertySetterOrNull<T>("StaticWriteOnlyT");
            AssertSetterT(setter);
        }

        [Test] public void GetStaticPropertySetter_WhenWriteOnly_ReturnsWorkingDelegate()
        {
            var setter = typeof(PropertyBase<T>).GetStaticPropertySetter<T>("StaticWriteOnlyT");
            AssertSetterT(setter);
        }

        [Test] public void GetStaticPropertySetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = typeof(PropertyBase<T>).GetStaticPropertySetterOrNull<T>("StaticPropertyObject");
            AssertSetterObject(setter);
        }

        [Test] public void GetStaticPropertySetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = typeof(PropertyBase<T>).GetStaticPropertySetter<T>("StaticPropertyObject");
            AssertSetterObject(setter);
        }

        [Test] public void GetStaticPropertySetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var setter = typeof(PropertyBase<T>).GetStaticPropertySetterOrNull<object>("StaticPropertyT");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetStaticPropertySetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetStaticPropertySetter<object>("StaticPropertyT"));
        }

        private static void AssertSetterT(Action<T> setter)
        {
            setter(_one);
            Assert.That(PropertyBase<T>.StaticPropertyT, Is.EqualTo(_one));
            setter(_two);
            Assert.That(PropertyBase<T>.StaticPropertyT, Is.EqualTo(_two));
        }

        private static void AssertSetterObject(Action<T> setter)
        {
            setter(_one);
            Assert.That(PropertyBase<T>.StaticPropertyObject, Is.EqualTo(_one));
            setter(_two);
            Assert.That(PropertyBase<T>.StaticPropertyObject, Is.EqualTo(_two));
        }
        #endregion setter

        #region accessor
        [Test] public void GetStaticPropertyOrNull_WhenNoSuchPropertyName_ReturnsNoDelegates()
        {
            var accessor = typeof(PropertyBase<T>).GetStaticPropertyAccessorOrNull<T>("NoSuchProperty");
            Assert.That(accessor.Get, Is.Null);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetStaticProperty_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetStaticPropertyAccessor<T>("NoSuchProperty"));
        }

        [Test] public void GetStaticPropertyOrNull_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = typeof(PropertyBase<T>).GetStaticPropertyAccessorOrNull<T>("StaticPropertyT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }

        [Test] public void GetStaticProperty_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = typeof(PropertyBase<T>).GetStaticPropertyAccessorOrNull<T>("StaticPropertyT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }
        [Test] public void GetStaticPropertyOrNull_WhenReadOnly_ReturnsGetter()
        {
            var accessor = typeof(PropertyBase<T>).GetStaticPropertyAccessorOrNull<T>("StaticReadOnlyT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetStaticProperty_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetStaticPropertyAccessor<T>("StaticReadOnlyT"));
        }

        [Test] public void GetStaticPropertyOrNull_WhenWriteOnly_ReturnsSetter()
        {
            var accessor = typeof(PropertyBase<T>).GetStaticPropertyAccessorOrNull<T>("StaticWriteOnlyT");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterT(accessor.Set);
        }

        [Test] public void GetStaticProperty_WhenWriteOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetStaticPropertyAccessor<T>("StaticWriteOnlyT"));
        }

        [Test] public void GetStaticPropertyOrNull_WhenWidenedMatch_ReturnsSetter()
        {
            if (typeof(T).IsValueType) return;

            var accessor = typeof(PropertyBase<T>).GetStaticPropertyAccessorOrNull<T>("StaticPropertyObject");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterObject(accessor.Set);
        }

        [Test] public void GetStaticProperty_WhenWidenedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetStaticPropertyAccessor<T>("StaticPropertyObject"));
        }

        [Test] public void GetStaticPropertyOrNull_WhenNarrowedMatch_ReturnsGetter()
        {
            if (typeof(T).IsValueType) return;
            var accessor = typeof(PropertyBase<T>).GetStaticPropertyAccessorOrNull<object>("StaticPropertyT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetStaticProperty_WhenNarrowedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetStaticPropertyAccessor<object>("StaticPropertyT"));
        }
        #endregion accessor
    }
}