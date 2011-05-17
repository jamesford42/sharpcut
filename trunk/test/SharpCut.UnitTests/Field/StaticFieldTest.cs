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

namespace SharpCut.UnitTests.Field
{
    /// <summary>
    /// Test cases for static field related extension methods in <see cref="Reflections"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>Kenneth Xu</author>
    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))]
    public class StaticFieldTest<T>
    {
        static readonly T _one = (T)Convert.ChangeType(1, typeof(T));
        static readonly T _two = (T)Convert.ChangeType(2, typeof(T));

        public StaticFieldTest()
        {
            // this statically initialize the FieldBase<T> class.
            FieldBase<T>.StaticFieldObject = null;
        }

        #region getter
        [Test]
        public void GetStaticFieldGetterOrNull_WhenNoSuchFieldName_ReturnsNull()
        {
            var getter = typeof(FieldBase<T>).GetStaticFieldGetterOrNull<T>("NoSuchField");
            Assert.That(getter, Is.Null);
        }

        [Test]
        public void GetStaticFieldGetter_WhenNoSuchFieldName_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetStaticFieldGetter<T>("NoSuchField"));
        }

        [Test]
        public void GetStaticFieldGetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = typeof(FieldBase<T>).GetStaticFieldGetterOrNull<T>("StaticFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetStaticFieldGetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = typeof(FieldBase<T>).GetStaticFieldGetter<T>("StaticFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetStaticFieldGetterOrNull_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = typeof(FieldBase<T>).GetStaticFieldGetterOrNull<T>("StaticReadOnlyT");
            Assert.That(getter(), Is.EqualTo(FieldBase<T>.StaticReadOnlyT));
        }

        [Test]
        public void GetStaticFieldGetter_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = typeof(FieldBase<T>).GetStaticFieldGetter<T>("StaticReadOnlyT");
            Assert.That(getter(), Is.EqualTo(FieldBase<T>.StaticReadOnlyT));
        }

        [Test]
        public void GetStaticFieldGetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = typeof(FieldBase<T>).GetStaticFieldGetterOrNull<object>("StaticFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetStaticFieldGetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = typeof(FieldBase<T>).GetStaticFieldGetter<object>("StaticFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetStaticFieldGetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var getter = typeof(FieldBase<T>).GetStaticFieldGetterOrNull<T>("StaticFieldObject");
            Assert.That(getter, Is.Null);
        }

        [Test]
        public void GetStaticFieldGetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetStaticFieldGetter<T>("StaticFieldObject"));
        }

        private static void AssertGetterT(Func<object> getter)
        {
            FieldBase<T>.StaticFieldT = _one;
            Assert.That(getter(), Is.EqualTo(_one));
            FieldBase<T>.StaticFieldT = _two;
            Assert.That(getter(), Is.EqualTo(_two));
        }

        private static void AssertGetterT(Func<T> getter)
        {
            AssertGetterT(new Func<object>(() => getter()));
        }
        #endregion getter

        #region setter
        [Test]
        public void GetStaticFieldSetterOrNull_WhenNoSuchFieldName_ReturnsNull()
        {
            var setter = typeof(FieldBase<T>).GetStaticFieldSetterOrNull<T>("NoSuchField");
            Assert.That(setter, Is.Null);
        }

        [Test]
        public void GetStaticFieldSetter_WhenNoSuchFieldName_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetStaticFieldSetter<T>("NoSuchField"));
        }

        [Test]
        public void GetStaticFieldSetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = typeof(FieldBase<T>).GetStaticFieldSetterOrNull<T>("StaticFieldT");
            AssertSetterT(setter);
        }

        [Test]
        public void GetStaticFieldSetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = typeof(FieldBase<T>).GetStaticFieldSetter<T>("StaticFieldT");
            AssertSetterT(setter);
        }

        [Test]
        public void GetStaticFieldSetterOrNull_WhenReadOnly_ReturnsNull()
        {
            var setter = typeof(FieldBase<T>).GetStaticFieldSetterOrNull<T>("StaticReadOnlyT");
            Assert.That(setter, Is.Null);
        }

        [Test]
        public void GetStaticFieldSetter_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetStaticFieldSetter<T>("StaticReadOnlyT"));
        }

        [Test]
        public void GetStaticFieldSetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = typeof(FieldBase<T>).GetStaticFieldSetterOrNull<T>("StaticFieldObject");
            AssertSetterObject(setter);
        }

        [Test]
        public void GetStaticFieldSetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = typeof(FieldBase<T>).GetStaticFieldSetter<T>("StaticFieldObject");
            AssertSetterObject(setter);
        }

        [Test]
        public void GetStaticFieldSetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var setter = typeof(FieldBase<T>).GetStaticFieldSetterOrNull<object>("StaticFieldT");
            Assert.That(setter, Is.Null);
        }

        [Test]
        public void GetStaticFieldSetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetStaticFieldSetter<object>("StaticFieldT"));
        }

        private static void AssertSetterT(Action<T> setter)
        {
            setter(_one);
            Assert.That(FieldBase<T>.StaticFieldT, Is.EqualTo(_one));
            setter(_two);
            Assert.That(FieldBase<T>.StaticFieldT, Is.EqualTo(_two));
        }

        private static void AssertSetterObject(Action<T> setter)
        {
            setter(_one);
            Assert.That(FieldBase<T>.StaticFieldObject, Is.EqualTo(_one));
            setter(_two);
            Assert.That(FieldBase<T>.StaticFieldObject, Is.EqualTo(_two));
        }
        #endregion setter

        #region accessor
        [Test]
        public void GetStaticFieldOrNull_WhenNoSuchFieldName_ReturnsNoDelegates()
        {
            var accessor = typeof(FieldBase<T>).GetStaticFieldAccessorOrNull<T>("NoSuchField");
            Assert.That(accessor.Get, Is.Null);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test]
        public void GetStaticField_WhenNoSuchFieldName_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetStaticFieldAccessor<T>("NoSuchField"));
        }

        [Test]
        public void GetStaticFieldOrNull_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = typeof(FieldBase<T>).GetStaticFieldAccessorOrNull<T>("StaticFieldT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }

        [Test]
        public void GetStaticField_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = typeof(FieldBase<T>).GetStaticFieldAccessorOrNull<T>("StaticFieldT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }
        [Test]
        public void GetStaticFieldOrNull_WhenReadOnly_ReturnsGetter()
        {
            var accessor = typeof(FieldBase<T>).GetStaticFieldAccessorOrNull<T>("StaticReadOnlyT");
            Assert.That(accessor.Get(), Is.EqualTo(FieldBase<T>.StaticReadOnlyT));
            Assert.That(accessor.Set, Is.Null);
        }

        [Test]
        public void GetStaticField_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetStaticFieldAccessor<T>("StaticReadOnlyT"));
        }

        [Test]
        public void GetStaticFieldOrNull_WhenWidenedMatch_ReturnsSetter()
        {
            if (typeof(T).IsValueType) return;

            var accessor = typeof(FieldBase<T>).GetStaticFieldAccessorOrNull<T>("StaticFieldObject");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterObject(accessor.Set);
        }

        [Test]
        public void GetStaticField_WhenWidenedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetStaticFieldAccessor<T>("StaticFieldObject"));
        }

        [Test]
        public void GetStaticFieldOrNull_WhenNarrowedMatch_ReturnsGetter()
        {
            if (typeof(T).IsValueType) return;
            var accessor = typeof(FieldBase<T>).GetStaticFieldAccessorOrNull<object>("StaticFieldT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test]
        public void GetStaticField_WhenNarrowedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetStaticFieldAccessor<object>("StaticFieldT"));
        }
        #endregion accessor
    }
}