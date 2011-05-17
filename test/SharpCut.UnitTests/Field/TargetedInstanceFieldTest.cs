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
    /// Test cases for targeted instance field related extension methods in <see cref="Reflections"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>Kenneth Xu</author>
    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))]
    public class TargetedInstanceFieldTest<T>
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
            var getter = _base.GetInstanceFieldGetterOrNull<T>("NoSuchField");
            Assert.That(getter, Is.Null);
        }

        [Test]
        public void GetInstanceFieldGetter_WhenNoSuchFieldName_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => _base.GetInstanceFieldGetter<T>("NoSuchField"));
        }

        [Test]
        public void GetInstanceFieldGetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = _base.GetInstanceFieldGetterOrNull<T>("InstanceFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetInstanceFieldGetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = _base.GetInstanceFieldGetter<T>("InstanceFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetInstanceFieldGetterOrNull_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = _base.GetInstanceFieldGetterOrNull<T>("InstanceReadOnlyT");
            Assert.That(getter(), Is.EqualTo(default(T)));
            getter = new FieldBase<T>(_two, null).GetInstanceFieldGetterOrNull<T>("InstanceReadOnlyT");
            Assert.That(getter(), Is.EqualTo(_two));
        }

        [Test]
        public void GetInstanceFieldGetter_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = _base.GetInstanceFieldGetter<T>("InstanceReadOnlyT");
            Assert.That(getter(), Is.EqualTo(default(T)));
            getter = new FieldBase<T>(_two, null).GetInstanceFieldGetter<T>("InstanceReadOnlyT");
            Assert.That(getter(), Is.EqualTo(_two));
        }

        [Test]
        public void GetInstanceFieldGetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = _base.GetInstanceFieldGetterOrNull<object>("InstanceFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetInstanceFieldGetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = _base.GetInstanceFieldGetter<object>("InstanceFieldT");
            AssertGetterT(getter);
        }

        [Test]
        public void GetInstanceFieldGetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var getter = _base.GetInstanceFieldGetterOrNull<T>("InstanceFieldObject");
            Assert.That(getter, Is.Null);
        }

        [Test]
        public void GetInstanceFieldGetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => _base.GetInstanceFieldGetter<T>("InstanceFieldObject"));
        }

        private void AssertGetterT(Func<object> getter)
        {
            _base.InstanceFieldT = _one;
            Assert.That(getter(), Is.EqualTo(_one));
            _base.InstanceFieldT = _two;
            Assert.That(getter(), Is.EqualTo(_two));
        }

        private void AssertGetterT(Func<T> getter)
        {
            AssertGetterT(new Func<object>(() => getter()));
        }
        #endregion getter

        #region setter
        [Test]
        public void GetInstanceFieldSetterOrNull_WhenNoSuchFieldName_ReturnsNull()
        {
            var setter = _base.GetInstanceFieldSetterOrNull<T>("NoSuchField");
            Assert.That(setter, Is.Null);
        }

        [Test]
        public void GetInstanceFieldSetter_WhenNoSuchFieldName_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => _base.GetInstanceFieldSetter<T>("NoSuchField"));
        }

        [Test]
        public void GetInstanceFieldSetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = _base.GetInstanceFieldSetterOrNull<T>("InstanceFieldT");
            AssertSetterT(setter);
        }

        [Test]
        public void GetInstanceFieldSetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = _base.GetInstanceFieldSetter<T>("InstanceFieldT");
            AssertSetterT(setter);
        }

        [Test]
        public void GetInstanceFieldSetterOrNull_WhenReadOnly_ReturnsNull()
        {
            var setter = _base.GetInstanceFieldSetterOrNull<T>("InstanceReadOnlyT");
            Assert.That(setter, Is.Null);
        }

        [Test]
        public void GetInstanceFieldSetter_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => _base.GetInstanceFieldSetter<T>("InstanceReadOnlyT"));
        }

        [Test]
        public void GetInstanceFieldSetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = _base.GetInstanceFieldSetterOrNull<T>("InstanceFieldObject");
            AssertSetterObject(setter);
        }

        [Test]
        public void GetInstanceFieldSetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = _base.GetInstanceFieldSetter<T>("InstanceFieldObject");
            AssertSetterObject(setter);
        }

        [Test]
        public void GetInstanceFieldSetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var setter = _base.GetInstanceFieldSetterOrNull<object>("InstanceFieldT");
            Assert.That(setter, Is.Null);
        }

        [Test]
        public void GetInstanceFieldSetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => _base.GetInstanceFieldSetter<object>("InstanceFieldT"));
        }

        private void AssertSetterT(Action<T> setter)
        {
            setter(_one);
            Assert.That(_base.InstanceFieldT, Is.EqualTo(_one));
            setter(_two);
            Assert.That(_base.InstanceFieldT, Is.EqualTo(_two));
        }

        private void AssertSetterObject(Action<T> setter)
        {
            setter(_one);
            Assert.That(_base.InstanceFieldObject, Is.EqualTo(_one));
            setter(_two);
            Assert.That(_base.InstanceFieldObject, Is.EqualTo(_two));
        }
        #endregion setter

        #region accessor
        [Test]
        public void GetInstanceFieldAccessorOrNull_WhenNoSuchFieldName_ReturnsNoDelegates()
        {
            var accessor = _base.GetInstanceFieldAccessorOrNull<T>("NoSuchField");
            Assert.That(accessor.Get, Is.Null);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test]
        public void GetInstanceFieldAccessor_WhenNoSuchFieldName_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => _base.GetInstanceFieldAccessor<T>("NoSuchField"));
        }

        [Test]
        public void GetInstanceFieldAccessorOrNull_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = _base.GetInstanceFieldAccessorOrNull<T>("InstanceFieldT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }

        [Test]
        public void GetInstanceFieldAccessor_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = _base.GetInstanceFieldAccessorOrNull<T>("InstanceFieldT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }
        [Test]
        public void GetInstanceFieldAccessorOrNull_WhenReadOnly_ReturnsGetter()
        {
            var accessor = _base.GetInstanceFieldAccessorOrNull<T>("InstanceReadOnlyT");
            Assert.That(accessor.Set, Is.Null);
            Assert.That(accessor.Get(), Is.EqualTo(default(T)));
            accessor = new FieldBase<T>(_two, null).GetInstanceFieldAccessorOrNull<T>("InstanceReadOnlyT");
            Assert.That(accessor.Get(), Is.EqualTo(_two));
        }

        [Test]
        public void GetInstanceFieldAccessor_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingFieldException>(
                () => _base.GetInstanceFieldAccessor<T>("InstanceReadOnlyT"));
        }

        [Test]
        public void GetInstanceFieldAccessorOrNull_WhenWidenedMatch_ReturnsSetter()
        {
            if (typeof(T).IsValueType) return;

            var accessor = _base.GetInstanceFieldAccessorOrNull<T>("InstanceFieldObject");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterObject(accessor.Set);
        }

        [Test]
        public void GetInstanceFieldAccessor_WhenWidenedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingFieldException>(
                () => _base.GetInstanceFieldAccessor<T>("InstanceFieldObject"));
        }

        [Test]
        public void GetInstanceFieldAccessorOrNull_WhenNarrowedMatch_ReturnsGetter()
        {
            if (typeof(T).IsValueType) return;
            var accessor = _base.GetInstanceFieldAccessorOrNull<object>("InstanceFieldT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test]
        public void GetInstanceFieldAccessor_WhenNarrowedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingFieldException>(
                () => _base.GetInstanceFieldAccessor<object>("InstanceFieldT"));
        }
        #endregion accessor
    }
}