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
    /// Test cases for untargeted instance field related extension methods in <see cref="Reflections"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>Kenneth Xu</author>
    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))]
    public class InstanceFieldTest<T>
    {
        static readonly T _one = (T)Convert.ChangeType(1, typeof(T));
        static readonly T _two = (T)Convert.ChangeType(2, typeof(T));

        FieldBase<T> _base;

        [SetUp]
        public void SetUp()
        {
            _base = new FieldBase<T>();
        }

        #region getter
        [Test]
        public void GetInstanceFieldGetterOrNull_WhenNoSuchFieldName_ReturnsNull()
        {
            var getter = typeof(FieldBase<T>).GetInstanceFieldGetterOrNull<FieldBase<T>, T>("NoSuchField");
            Assert.That(getter, Is.Null);
        }

        [Test]
        public void GetInstanceFieldGetter_WhenNoSuchFieldName_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetInstanceFieldGetter<FieldBase<T>, T>("NoSuchField"));
        }

        [Test]
        public void GetInstanceFieldGetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = typeof(FieldBase<T>).GetInstanceFieldGetterOrNull<FieldBase<T>, T>("InstanceFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetInstanceFieldGetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = typeof(FieldBase<T>).GetInstanceFieldGetter<FieldBase<T>, T>("InstanceFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetInstanceFieldGetterOrNull_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = typeof(FieldBase<T>).GetInstanceFieldGetterOrNull<FieldBase<T>, T>("InstanceReadOnlyT");
            AssertReadOnlyGetterT(getter);
        }

        private void AssertReadOnlyGetterT(Func<FieldBase<T>, T> getter)
        {
            Assert.That(getter(_base), Is.EqualTo(default(T)));
            Assert.That(getter(new FieldBase<T>(_two, null)), Is.EqualTo(_two));
        }

        [Test]
        public void GetInstanceFieldGetter_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = typeof(FieldBase<T>).GetInstanceFieldGetter<FieldBase<T>, T>("InstanceReadOnlyT");
            AssertReadOnlyGetterT(getter);
        }

        [Test]
        public void GetInstanceFieldGetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = typeof(FieldBase<T>).GetInstanceFieldGetterOrNull<FieldBase<T>, object>("InstanceFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetInstanceFieldGetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = typeof(FieldBase<T>).GetInstanceFieldGetter<FieldBase<T>, object>("InstanceFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetInstanceFieldGetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var getter = typeof(FieldBase<T>).GetInstanceFieldGetterOrNull<FieldBase<T>, T>("InstanceFieldObject");
            Assert.That(getter, Is.Null);
        }

        [Test]
        public void GetInstanceFieldGetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetInstanceFieldGetter<FieldBase<T>, T>("InstanceFieldObject"));
        }

        private void AssertGetterT(Func<FieldBase<T>, object> getter)
        {
            _base.InstanceFieldT = _one;
            Assert.That(getter(_base), Is.EqualTo(_one));
            _base.InstanceFieldT = _two;
            Assert.That(getter(_base), Is.EqualTo(_two));
        }

        private void AssertGetterT(Func<FieldBase<T>, T> getter)
        {
            AssertGetterT(new Func<FieldBase<T>, object>(x => getter(x)));
        }
        #endregion getter

        #region setter
        [Test]
        public void GetInstanceFieldSetterOrNull_WhenNoSuchFieldName_ReturnsNull()
        {
            var setter = typeof(FieldBase<T>).GetInstanceFieldSetterOrNull<FieldBase<T>, T>("NoSuchField");
            Assert.That(setter, Is.Null);
        }

        [Test]
        public void GetInstanceFieldSetter_WhenNoSuchFieldName_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetInstanceFieldSetter<FieldBase<T>, T>("NoSuchField"));
        }

        [Test]
        public void GetInstanceFieldSetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = typeof(FieldBase<T>).GetInstanceFieldSetterOrNull<FieldBase<T>, T>("InstanceFieldT");
            AssertSetterT(setter);
        }

        [Test]
        public void GetInstanceFieldSetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = typeof(FieldBase<T>).GetInstanceFieldSetter<FieldBase<T>, T>("InstanceFieldT");
            AssertSetterT(setter);
        }

        [Test]
        public void GetInstanceFieldSetterOrNull_WhenReadOnly_ReturnsNull()
        {
            var setter = typeof(FieldBase<T>).GetInstanceFieldSetterOrNull<FieldBase<T>, T>("InstanceReadOnlyT");
            Assert.That(setter, Is.Null);
        }

        [Test]
        public void GetInstanceFieldSetter_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetInstanceFieldSetter<FieldBase<T>, T>("InstanceReadOnlyT"));
        }

        [Test]
        public void GetInstanceFieldSetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = typeof(FieldBase<T>).GetInstanceFieldSetterOrNull<FieldBase<T>, T>("InstanceFieldObject");
            AssertSetterObject(setter);
        }

        [Test]
        public void GetInstanceFieldSetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = typeof(FieldBase<T>).GetInstanceFieldSetter<FieldBase<T>, T>("InstanceFieldObject");
            AssertSetterObject(setter);
        }

        [Test]
        public void GetInstanceFieldSetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var setter = typeof(FieldBase<T>).GetInstanceFieldSetterOrNull<FieldBase<T>, object>("InstanceFieldT");
            Assert.That(setter, Is.Null);
        }

        [Test]
        public void GetInstanceFieldSetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetInstanceFieldSetter<FieldBase<T>, object>("InstanceFieldT"));
        }

        private void AssertSetterT(Action<FieldBase<T>, T> setter)
        {
            setter(_base, _one);
            Assert.That(_base.InstanceFieldT, Is.EqualTo(_one));
            setter(_base, _two);
            Assert.That(_base.InstanceFieldT, Is.EqualTo(_two));
        }

        private void AssertSetterObject(Action<FieldBase<T>, T> setter)
        {
            setter(_base, _one);
            Assert.That(_base.InstanceFieldObject, Is.EqualTo(_one));
            setter(_base, _two);
            Assert.That(_base.InstanceFieldObject, Is.EqualTo(_two));
        }
        #endregion setter

        #region accessor
        [Test]
        public void GetInstanceFieldAccessorOrNull_WhenNoSuchFieldName_ReturnsNoDelegates()
        {
            var accessor = typeof(FieldBase<T>).GetInstanceFieldAccessorOrNull<FieldBase<T>, T>("NoSuchField");
            Assert.That(accessor.Get, Is.Null);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test]
        public void GetInstanceFieldAccessor_WhenNoSuchFieldName_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetInstanceFieldAccessor<FieldBase<T>, T>("NoSuchField"));
        }

        [Test]
        public void GetInstanceFieldAccessorOrNull_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = typeof(FieldBase<T>).GetInstanceFieldAccessorOrNull<FieldBase<T>, T>("InstanceFieldT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }

        [Test]
        public void GetInstanceFieldAccessor_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = typeof(FieldBase<T>).GetInstanceFieldAccessorOrNull<FieldBase<T>, T>("InstanceFieldT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }
        [Test]
        public void GetInstanceFieldAccessorOrNull_WhenReadOnly_ReturnsGetter()
        {
            var accessor = typeof(FieldBase<T>).GetInstanceFieldAccessorOrNull<FieldBase<T>, T>("InstanceReadOnlyT");
            AssertReadOnlyGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test]
        public void GetInstanceFieldAccessor_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetInstanceFieldAccessor<FieldBase<T>, T>("InstanceReadOnlyT"));
        }

        [Test]
        public void GetInstanceFieldAccessorOrNull_WhenWidenedMatch_ReturnsSetter()
        {
            if (typeof(T).IsValueType) return;

            var accessor = typeof(FieldBase<T>).GetInstanceFieldAccessorOrNull<FieldBase<T>, T>("InstanceFieldObject");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterObject(accessor.Set);
        }

        [Test]
        public void GetInstanceFieldAccessor_WhenWidenedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetInstanceFieldAccessor<FieldBase<T>, T>("InstanceFieldObject"));
        }

        [Test]
        public void GetInstanceFieldAccessorOrNull_WhenNarrowedMatch_ReturnsGetter()
        {
            if (typeof(T).IsValueType) return;
            var accessor = typeof(FieldBase<T>).GetInstanceFieldAccessorOrNull<FieldBase<T>, object>("InstanceFieldT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test]
        public void GetInstanceFieldAccessor_WhenNarrowedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingFieldException>(
                () => typeof(FieldBase<T>).GetInstanceFieldAccessor<FieldBase<T>, object>("InstanceFieldT"));
        }
        #endregion accessor
    }
}