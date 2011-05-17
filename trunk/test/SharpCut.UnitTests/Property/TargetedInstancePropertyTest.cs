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
    /// Test cases for targeted instance property related extension methods in <see cref="Reflections"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>Kenneth Xu</author>
    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))]
    public class TargetedInstancePropertyTest<T>
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
            var getter = _base.GetInstancePropertyGetterOrNull<T>("NoSuchProperty");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetInstancePropertyGetter_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertyGetter<T>("NoSuchProperty"));
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = _base.GetInstancePropertyGetterOrNull<T>("InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = _base.GetInstancePropertyGetter<T>("InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = _base.GetInstancePropertyGetterOrNull<T>("InstanceReadOnlyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetter_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = _base.GetInstancePropertyGetter<T>("InstanceReadOnlyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenWriteOnly_ReturnsNull()
        {
            var getter = _base.GetInstancePropertyGetterOrNull<T>("InstanceWriteOnlyT");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetInstancePropertyGetter_WhenWriteOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertyGetter<T>("InstanceWriteOnlyT"));
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = _base.GetInstancePropertyGetterOrNull<object>("InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = _base.GetInstancePropertyGetter<object>("InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var getter = _base.GetInstancePropertyGetterOrNull<T>("InstancePropertyObject");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetInstancePropertyGetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertyGetter<T>("InstancePropertyObject"));
        }

        [Test] public void GetInstancePropertyGetterOrNull_WhenPropertyVirtual_ReturnsDelegateCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var getter = sub.GetInstancePropertyGetterOrNull<T>("VirtualPropertyT");
            AssertVirtualGetterT(sub, getter);
        }

        [Test] public void GetInstancePropertyGetter_WhenPropertyVirtual_ReturnsDelegateCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var getter = sub.GetInstancePropertyGetter<T>("VirtualPropertyT");
            AssertVirtualGetterT(sub, getter);
        }

        private void AssertVirtualGetterT(PropertySub<T> sub, Func<T> getter)
        {
            _base = sub;
            _base.InstancePropertyT = _one;
            sub.InstancePropertyT = _two;
            Assert.That(getter(), Is.EqualTo(_two));
            _base.InstancePropertyT = _two;
            sub.InstancePropertyT = _one;
            Assert.That(getter(), Is.EqualTo(_one));
        }

        private void AssertGetterT(Func<object> getter)
        {
            _base.InstancePropertyT = _one;
            Assert.That(getter(), Is.EqualTo(_one));
            _base.InstancePropertyT = _two;
            Assert.That(getter(), Is.EqualTo(_two));
        }

        private void AssertGetterT(Func<T> getter)
        {
            AssertGetterT(new Func<object>(() => getter()));
        }
        #endregion getter

        #region setter
        [Test] public void GetInstancePropertySetterOrNull_WhenNoSuchPropertyName_ReturnsNull()
        {
            var setter = _base.GetInstancePropertySetterOrNull<T>("NoSuchProperty");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetInstancePropertySetter_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertySetter<T>("NoSuchProperty"));
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = _base.GetInstancePropertySetterOrNull<T>("InstancePropertyT");
            AssertSetterT(setter);
        }

        [Test] public void GetInstancePropertySetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = _base.GetInstancePropertySetter<T>("InstancePropertyT");
            AssertSetterT(setter);
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenReadOnly_ReturnsNull()
        {
            var setter = _base.GetInstancePropertySetterOrNull<T>("InstanceReadOnlyT");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetInstancePropertySetter_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertySetter<T>("InstanceReadOnlyT"));
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenWriteOnly_ReturnsWorkingDelegate()
        {
            var setter = _base.GetInstancePropertySetterOrNull<T>("InstanceWriteOnlyT");
            AssertSetterT(setter);
        }

        [Test] public void GetInstancePropertySetter_WhenWriteOnly_ReturnsWorkingDelegate()
        {
            var setter = _base.GetInstancePropertySetter<T>("InstanceWriteOnlyT");
            AssertSetterT(setter);
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = _base.GetInstancePropertySetterOrNull<T>("InstancePropertyObject");
            AssertSetterObject(setter);
        }

        [Test] public void GetInstancePropertySetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = _base.GetInstancePropertySetter<T>("InstancePropertyObject");
            AssertSetterObject(setter);
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var setter = _base.GetInstancePropertySetterOrNull<object>("InstancePropertyT");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetInstancePropertySetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertySetter<object>("InstancePropertyT"));
        }

        [Test] public void GetInstancePropertySetterOrNull_WhenPropertyVirtual_ReturnDelegateCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var setter = sub.GetInstancePropertySetterOrNull<T>("VirtualPropertyT");
            AssertVirtualSetterT(sub, setter);
        }

        [Test] public void GetInstancePropertySetter_WhenPropertyVirtual_ReturnDelegateCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var setter = sub.GetInstancePropertySetter<T>("VirtualPropertyT");
            AssertVirtualSetterT(sub, setter);
        }

        private void AssertVirtualSetterT(PropertySub<T> sub, Action<T> setter)
        {
            _base = sub;
            sub.InstancePropertyT = _one;
            _base.InstancePropertyT = _one;
            setter(_two);
            Assert.That(sub.InstancePropertyT, Is.EqualTo(_two));
            Assert.That(_base.InstancePropertyT, Is.EqualTo(_one));
        }

        private void AssertSetterT(Action<T> setter)
        {
            setter(_one);
            Assert.That(_base.InstancePropertyT, Is.EqualTo(_one));
            setter(_two);
            Assert.That(_base.InstancePropertyT, Is.EqualTo(_two));
        }

        private void AssertSetterObject(Action<T> setter)
        {
            setter(_one);
            Assert.That(_base.InstancePropertyObject, Is.EqualTo(_one));
            setter(_two);
            Assert.That(_base.InstancePropertyObject, Is.EqualTo(_two));
        }
        #endregion setter

        #region accessor
        [Test] public void GetInstancePropertyAccessorOrNull_WhenNoSuchPropertyName_ReturnsNoDelegates()
        {
            var accessor = _base.GetInstancePropertyAccessorOrNull<T>("NoSuchProperty");
            Assert.That(accessor.Get, Is.Null);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetInstancePropertyAccessor_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertyAccessor<T>("NoSuchProperty"));
        }

        [Test] public void GetInstancePropertyAccessorOrNull_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = _base.GetInstancePropertyAccessorOrNull<T>("InstancePropertyT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }

        [Test] public void GetInstancePropertyAccessor_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = _base.GetInstancePropertyAccessorOrNull<T>("InstancePropertyT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }
        [Test] public void GetInstancePropertyAccessorOrNull_WhenReadOnly_ReturnsGetter()
        {
            var accessor = _base.GetInstancePropertyAccessorOrNull<T>("InstanceReadOnlyT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetInstancePropertyAccessor_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertyAccessor<T>("InstanceReadOnlyT"));
        }

        [Test] public void GetInstancePropertyAccessorOrNull_WhenWriteOnly_ReturnsSetter()
        {
            var accessor = _base.GetInstancePropertyAccessorOrNull<T>("InstanceWriteOnlyT");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterT(accessor.Set);
        }

        [Test] public void GetInstancePropertyAccessor_WhenWriteOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertyAccessor<T>("InstanceWriteOnlyT"));
        }

        [Test] public void GetInstancePropertyAccessorOrNull_WhenWidenedMatch_ReturnsSetter()
        {
            if (typeof(T).IsValueType) return;

            var accessor = _base.GetInstancePropertyAccessorOrNull<T>("InstancePropertyObject");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterObject(accessor.Set);
        }

        [Test] public void GetInstancePropertyAccessor_WhenWidenedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertyAccessor<T>("InstancePropertyObject"));
        }

        [Test] public void GetInstancePropertyAccessorOrNull_WhenNarrowedMatch_ReturnsGetter()
        {
            if (typeof(T).IsValueType) return;
            var accessor = _base.GetInstancePropertyAccessorOrNull<object>("InstancePropertyT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetInstancePropertyAccessor_WhenNarrowedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingMemberException>(
                () => _base.GetInstancePropertyAccessor<object>("InstancePropertyT"));
        }

        [Test] public void GetInstancePropertyAccessorOrNull_WhenPropertyVirtual_ReturnsAccessorCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var accessor = sub.GetInstancePropertyAccessorOrNull<T>("VirtualPropertyT");
            AssertVirtualGetterT(sub, accessor.Get);
            AssertVirtualSetterT(sub, accessor.Set);
        }

        [Test] public void GetInstancePropertyAccessor_WhenPropertyVirtual_ReturnsAccessorCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var accessor = sub.GetInstancePropertyAccessor<T>("VirtualPropertyT");
            AssertVirtualGetterT(sub, accessor.Get);
            AssertVirtualSetterT(sub, accessor.Set);
        }
        #endregion accessor
    }
}