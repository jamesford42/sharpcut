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
    /// Test cases for untargeted instance property related extension methods in <see cref="Reflections"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>Kenneth Xu</author>
    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))]
    public class InstancePropertyTest<T>
    {
        static readonly T _one = (T)Convert.ChangeType(1, typeof (T));
        static readonly T _two = (T)Convert.ChangeType(2, typeof (T));

        PropertyBase<T> _base;

        [SetUp] public void SetUp()
        {
            _base = new PropertyBase<T>();
        }

        #region getter
        [Test] public void GetInstancePropertyGetterOrNull_WhenNoSuchPropertyName_ReturnsNull()
        {
            var getter = typeof (PropertyBase<T>).GetInstancePropertyGetterOrNull<PropertyBase<T>, T>("NoSuchProperty");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetInstancePropertyGetter_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof (PropertyBase<T>).GetInstancePropertyGetter<PropertyBase<T>, T>("NoSuchProperty"));
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = typeof(PropertyBase<T>).GetInstancePropertyGetterOrNull<PropertyBase<T>, T>("InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = typeof(PropertyBase<T>).GetInstancePropertyGetter<PropertyBase<T>, T>("InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = typeof(PropertyBase<T>).GetInstancePropertyGetterOrNull<PropertyBase<T>, T>("InstanceReadOnlyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetter_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = typeof(PropertyBase<T>).GetInstancePropertyGetter<PropertyBase<T>, T>("InstanceReadOnlyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenWriteOnly_ReturnsNull()
        {
            var getter = typeof(PropertyBase<T>).GetInstancePropertyGetterOrNull<PropertyBase<T>, T>("InstanceWriteOnlyT");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetInstancePropertyGetter_WhenWriteOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetInstancePropertyGetter<PropertyBase<T>, T>("InstanceWriteOnlyT"));
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = typeof(PropertyBase<T>).GetInstancePropertyGetterOrNull<PropertyBase<T>, object>("InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = typeof(PropertyBase<T>).GetInstancePropertyGetter<PropertyBase<T>, object>("InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var getter = typeof(PropertyBase<T>).GetInstancePropertyGetterOrNull<PropertyBase<T>, T>("InstancePropertyObject");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetInstancePropertyGetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetInstancePropertyGetter<PropertyBase<T>, T>("InstancePropertyObject"));
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenPropertyVirtual_ReturnsDelegateCallsSubProperty()
        {
            var getter = typeof(PropertyBase<T>).GetInstancePropertyGetterOrNull<PropertyBase<T>, T>("VirtualPropertyT");
            AssertVirtualGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetter_WhenPropertyVirtual_ReturnsDelegateCallsSubProperty()
        {
            var getter = typeof(PropertyBase<T>).GetInstancePropertyGetter<PropertyBase<T>, T>("VirtualPropertyT");
            AssertVirtualGetterT(getter);
        }

        private void AssertVirtualGetterT(Func<PropertyBase<T>, T> getter)
        {
            var sub = new PropertySub<T>();
            _base = sub;
            _base.InstancePropertyT = _one;
            sub.InstancePropertyT = _two;
            Assert.That(getter(_base), Is.EqualTo(_two));
            _base.InstancePropertyT = _two;
            sub.InstancePropertyT = _one;
            Assert.That(getter(_base), Is.EqualTo(_one));
        }

        private void AssertGetterT(Func<PropertyBase<T>, object> getter)
        {
            _base.InstancePropertyT = _one;
            Assert.That(getter(_base), Is.EqualTo(_one));
            _base.InstancePropertyT = _two;
            Assert.That(getter(_base), Is.EqualTo(_two));
        }

        private void AssertGetterT(Func<PropertyBase<T>, T> getter)
        {
            AssertGetterT(new Func<PropertyBase<T>, object>(x => getter(x)));
        }
        #endregion getter

        #region setter
        [Test] public void GetInstancePropertySetterOrNull_WhenNoSuchPropertyName_ReturnsNull()
        {
            var setter = typeof(PropertyBase<T>).GetInstancePropertySetterOrNull<PropertyBase<T>, T>("NoSuchProperty");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetInstancePropertySetter_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetInstancePropertySetter<PropertyBase<T>, T>("NoSuchProperty"));
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = typeof(PropertyBase<T>).GetInstancePropertySetterOrNull<PropertyBase<T>, T>("InstancePropertyT");
            AssertSetterT(setter);
        }

        [Test] public void GetInstancePropertySetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = typeof(PropertyBase<T>).GetInstancePropertySetter<PropertyBase<T>, T>("InstancePropertyT");
            AssertSetterT(setter);
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenReadOnly_ReturnsNull()
        {
            var setter = typeof(PropertyBase<T>).GetInstancePropertySetterOrNull<PropertyBase<T>, T>("InstanceReadOnlyT");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetInstancePropertySetter_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetInstancePropertySetter<PropertyBase<T>, T>("InstanceReadOnlyT"));
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenWriteOnly_ReturnsWorkingDelegate()
        {
            var setter = typeof(PropertyBase<T>).GetInstancePropertySetterOrNull<PropertyBase<T>, T>("InstanceWriteOnlyT");
            AssertSetterT(setter);
        }

        [Test] public void GetInstancePropertySetter_WhenWriteOnly_ReturnsWorkingDelegate()
        {
            var setter = typeof(PropertyBase<T>).GetInstancePropertySetter<PropertyBase<T>, T>("InstanceWriteOnlyT");
            AssertSetterT(setter);
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = typeof(PropertyBase<T>).GetInstancePropertySetterOrNull<PropertyBase<T>, T>("InstancePropertyObject");
            AssertSetterObject(setter);
        }

        [Test] public void GetInstancePropertySetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = typeof(PropertyBase<T>).GetInstancePropertySetter<PropertyBase<T>, T>("InstancePropertyObject");
            AssertSetterObject(setter);
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var setter = typeof(PropertyBase<T>).GetInstancePropertySetterOrNull<PropertyBase<T>, object>("InstancePropertyT");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetInstancePropertySetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetInstancePropertySetter<PropertyBase<T>, object>("InstancePropertyT"));
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenPropertyVirtual_ReturnDelegateCallsSubProperty()
        {
            var setter = typeof(PropertyBase<T>).GetInstancePropertySetterOrNull<PropertyBase<T>, T>("VirtualPropertyT");
            AssertVirtualSetterT(setter);
        }

        [Test] public void GetInstancePropertySetter_WhenPropertyVirtual_ReturnDelegateCallsSubProperty()
        {
            var setter = typeof(PropertyBase<T>).GetInstancePropertySetter<PropertyBase<T>, T>("VirtualPropertyT");
            AssertVirtualSetterT(setter);
        }

        private void AssertVirtualSetterT(Action<PropertyBase<T>, T> setter)
        {
            var sub = new PropertySub<T>();
            _base = sub;
            sub.InstancePropertyT = _one;
            _base.InstancePropertyT = _one;
            setter(_base, _two);
            Assert.That(sub.InstancePropertyT, Is.EqualTo(_two));
            Assert.That(_base.InstancePropertyT, Is.EqualTo(_one));
        }

        private void AssertSetterT(Action<PropertyBase<T>, T> setter)
        {
            setter(_base, _one);
            Assert.That(_base.InstancePropertyT, Is.EqualTo(_one));
            setter(_base, _two);
            Assert.That(_base.InstancePropertyT, Is.EqualTo(_two));
        }

        private void AssertSetterObject(Action<PropertyBase<T>, T> setter)
        {
            setter(_base, _one);
            Assert.That(_base.InstancePropertyObject, Is.EqualTo(_one));
            setter(_base, _two);
            Assert.That(_base.InstancePropertyObject, Is.EqualTo(_two));
        }
        #endregion setter

        #region accessor
        [Test] public void GetInstancePropertyAccessorOrNull_WhenNoSuchPropertyName_ReturnsNoDelegates()
        {
            var accessor = typeof(PropertyBase<T>).GetInstancePropertyAccessorOrNull<PropertyBase<T>, T>("NoSuchProperty");
            Assert.That(accessor.Get, Is.Null);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetInstancePropertyAccessor_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetInstancePropertyAccessor<PropertyBase<T>, T>("NoSuchProperty"));
        }

        [Test] public void GetInstancePropertyAccessorOrNull_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = typeof(PropertyBase<T>).GetInstancePropertyAccessorOrNull<PropertyBase<T>, T>("InstancePropertyT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }

        [Test] public void GetInstancePropertyAccessor_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = typeof(PropertyBase<T>).GetInstancePropertyAccessorOrNull<PropertyBase<T>, T>("InstancePropertyT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }
        [Test] public void GetInstancePropertyAccessorOrNull_WhenReadOnly_ReturnsGetter()
        {
            var accessor = typeof(PropertyBase<T>).GetInstancePropertyAccessorOrNull<PropertyBase<T>, T>("InstanceReadOnlyT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetInstancePropertyAccessor_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetInstancePropertyAccessor<PropertyBase<T>, T>("InstanceReadOnlyT"));
        }

        [Test] public void GetInstancePropertyAccessorOrNull_WhenWriteOnly_ReturnsSetter()
        {
            var accessor = typeof(PropertyBase<T>).GetInstancePropertyAccessorOrNull<PropertyBase<T>, T>("InstanceWriteOnlyT");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterT(accessor.Set);
        }

        [Test] public void GetInstancePropertyAccessor_WhenWriteOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetInstancePropertyAccessor<PropertyBase<T>, T>("InstanceWriteOnlyT"));
        }

        [Test] public void GetInstancePropertyAccessorOrNull_WhenWidenedMatch_ReturnsSetter()
        {
            if (typeof(T).IsValueType) return;

            var accessor = typeof(PropertyBase<T>).GetInstancePropertyAccessorOrNull<PropertyBase<T>, T>("InstancePropertyObject");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterObject(accessor.Set);
        }

        [Test] public void GetInstancePropertyAccessor_WhenWidenedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetInstancePropertyAccessor<PropertyBase<T>, T>("InstancePropertyObject"));
        }

        [Test] public void GetInstancePropertyAccessorOrNull_WhenNarrowedMatch_ReturnsGetter()
        {
            if (typeof(T).IsValueType) return;
            var accessor = typeof(PropertyBase<T>).GetInstancePropertyAccessorOrNull<PropertyBase<T>, object>("InstancePropertyT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetInstancePropertyAccessor_WhenNarrowedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingMemberException>(
                () => typeof(PropertyBase<T>).GetInstancePropertyAccessor<PropertyBase<T>, object>("InstancePropertyT"));
        }

        [Test] public void GetInstancePropertyAccessorOrNull_WhenPropertyVirtual_ReturnsAccessorCallsSubProperty()
        {
            var accessor = typeof(PropertyBase<T>).GetInstancePropertyAccessorOrNull<PropertyBase<T>, T>("VirtualPropertyT");
            AssertVirtualGetterT(accessor.Get);
            AssertVirtualSetterT(accessor.Set);
        }

        [Test] public void GetInstancePropertyAccessor_WhenPropertyVirtual_ReturnsAccessorCallsSubProperty()
        {
            var accessor = typeof(PropertyBase<T>).GetInstancePropertyAccessor<PropertyBase<T>, T>("VirtualPropertyT");
            AssertVirtualGetterT(accessor.Get);
            AssertVirtualSetterT(accessor.Set);
        }
        #endregion accessor
    }
}