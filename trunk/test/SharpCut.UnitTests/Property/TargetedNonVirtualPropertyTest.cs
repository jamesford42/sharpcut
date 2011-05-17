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
    /// Test cases for targeted non virtual property related extension methods in <see cref="Reflections"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>Kenneth Xu</author>
    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))]
    public class TargetedNonVirtualPropertyTest<T>
    {
        static readonly T _one = (T)Convert.ChangeType(1, typeof (T));
        static readonly T _two = (T)Convert.ChangeType(2, typeof (T));

        PropertyBase<T> _base;

        [SetUp] public void SetUp()
        {
            _base = new PropertyBase<T>();
        }

        #region getter
        [Test] public void GetNonVirtualPropertyGetterOrNull_WhenNoSuchPropertyName_ReturnsNull()
        {
            var getter = _base.GetNonVirtualPropertyGetterOrNull<T>(typeof(PropertyBase<T>), "NoSuchProperty");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetNonVirtualPropertyGetter_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertyGetter<T>(typeof(PropertyBase<T>), "NoSuchProperty"));
        }

        [Test] public void GetNonVirtualPropertyGetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = _base.GetNonVirtualPropertyGetterOrNull<T>(typeof(PropertyBase<T>), "InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetNonVirtualPropertyGetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var getter = _base.GetNonVirtualPropertyGetter<T>(typeof(PropertyBase<T>), "InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetNonVirtualPropertyGetterOrNull_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = _base.GetNonVirtualPropertyGetterOrNull<T>(typeof(PropertyBase<T>), "InstanceReadOnlyT");
            AssertGetterT(getter);
        }

        [Test] public void GetNonVirtualPropertyGetter_WhenReadOnly_ReturnsWorkingDelegate()
        {
            var getter = _base.GetNonVirtualPropertyGetter<T>(typeof(PropertyBase<T>), "InstanceReadOnlyT");
            AssertGetterT(getter);
        }

        [Test] public void GetNonVirtualPropertyGetterOrNull_WhenWriteOnly_ReturnsNull()
        {
            var getter = _base.GetNonVirtualPropertyGetterOrNull<T>(typeof(PropertyBase<T>), "InstanceWriteOnlyT");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetNonVirtualPropertyGetter_WhenWriteOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertyGetter<T>(typeof(PropertyBase<T>), "InstanceWriteOnlyT"));
        }

        [Test] public void GetNonVirtualPropertyGetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = _base.GetNonVirtualPropertyGetterOrNull<object>(typeof(PropertyBase<T>), "InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetNonVirtualPropertyGetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var getter = _base.GetNonVirtualPropertyGetter<object>(typeof(PropertyBase<T>), "InstancePropertyT");
            AssertGetterT(getter);
        }

        [Test] public void GetNonVirtualPropertyGetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var getter = _base.GetNonVirtualPropertyGetterOrNull<T>(typeof(PropertyBase<T>), "InstancePropertyObject");
            Assert.That(getter, Is.Null);
        }

        [Test] public void GetNonVirtualPropertyGetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertyGetter<T>(typeof(PropertyBase<T>), "InstancePropertyObject"));
        }

        [Test] public void GetNonVirtualPropertyGetterOrNull_WhenPropertyVirtual_ReturnsDelegateCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var getter = sub.GetNonVirtualPropertyGetterOrNull<T>(typeof(PropertyBase<T>), "VirtualPropertyT");
            AssertVirtualGetterT(sub, getter);
        }

        [Test] public void GetNonVirtualPropertyGetter_WhenPropertyVirtual_ReturnsDelegateCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var getter = sub.GetNonVirtualPropertyGetter<T>(typeof(PropertyBase<T>), "VirtualPropertyT");
            AssertVirtualGetterT(sub, getter);
        }

        private void AssertVirtualGetterT(PropertySub<T> sub, Func<T> getter)
        {
            _base = sub;
            _base.InstancePropertyT = _one;
            sub.InstancePropertyT = _two;
            Assert.That(getter(), Is.EqualTo(_one));
            _base.InstancePropertyT = _two;
            sub.InstancePropertyT = _one;
            Assert.That(getter(), Is.EqualTo(_two));
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
        [Test] public void GetNonVirtualPropertySetterOrNull_WhenNoSuchPropertyName_ReturnsNull()
        {
            var setter = _base.GetNonVirtualPropertySetterOrNull<T>(typeof(PropertyBase<T>), "NoSuchProperty");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetNonVirtualPropertySetter_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertySetter<T>(typeof(PropertyBase<T>), "NoSuchProperty"));
        }

        [Test] public void GetNonVirtualPropertySetterOrNull_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = _base.GetNonVirtualPropertySetterOrNull<T>(typeof(PropertyBase<T>), "InstancePropertyT");
            AssertSetterT(setter);
        }

        [Test] public void GetNonVirtualPropertySetter_WhenExactMatch_ReturnsWorkingDelegate()
        {
            var setter = _base.GetNonVirtualPropertySetter<T>(typeof(PropertyBase<T>), "InstancePropertyT");
            AssertSetterT(setter);
        }

        [Test] public void GetNonVirtualPropertySetterOrNull_WhenReadOnly_ReturnsNull()
        {
            var setter = _base.GetNonVirtualPropertySetterOrNull<T>(typeof(PropertyBase<T>), "InstanceReadOnlyT");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetNonVirtualPropertySetter_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertySetter<T>(typeof(PropertyBase<T>), "InstanceReadOnlyT"));
        }

        [Test] public void GetNonVirtualPropertySetterOrNull_WhenWriteOnly_ReturnsWorkingDelegate()
        {
            var setter = _base.GetNonVirtualPropertySetterOrNull<T>(typeof(PropertyBase<T>), "InstanceWriteOnlyT");
            AssertSetterT(setter);
        }

        [Test] public void GetNonVirtualPropertySetter_WhenWriteOnly_ReturnsWorkingDelegate()
        {
            var setter = _base.GetNonVirtualPropertySetter<T>(typeof(PropertyBase<T>), "InstanceWriteOnlyT");
            AssertSetterT(setter);
        }

        [Test] public void GetNonVirtualPropertySetterOrNull_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = _base.GetNonVirtualPropertySetterOrNull<T>(typeof(PropertyBase<T>), "InstancePropertyObject");
            AssertSetterObject(setter);
        }

        [Test] public void GetNonVirtualPropertySetter_WhenWidenedMatch_ReturnsWorkingDelegate()
        {
            if (typeof(T).IsValueType) return;

            var setter = _base.GetNonVirtualPropertySetter<T>(typeof(PropertyBase<T>), "InstancePropertyObject");
            AssertSetterObject(setter);
        }

        [Test] public void GetNonVirtualPropertySetterOrNull_WhenNarrowedMatch_ReturnsNull()
        {
            var setter = _base.GetNonVirtualPropertySetterOrNull<object>(typeof(PropertyBase<T>), "InstancePropertyT");
            Assert.That(setter, Is.Null);
        }

        [Test] public void GetNonVirtualPropertySetter_WhenNarrowedMatch_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertySetter<object>(typeof(PropertyBase<T>), "InstancePropertyT"));
        }

        [Test] public void GetNonVirtualPropertySetterOrNull_WhenPropertyVirtual_ReturnDelegateCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var setter = sub.GetNonVirtualPropertySetterOrNull<T>(typeof(PropertyBase<T>), "VirtualPropertyT");
            AssertVirtualSetterT(sub, setter);
        }

        [Test] public void GetNonVirtualPropertySetter_WhenPropertyVirtual_ReturnDelegateCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var setter = sub.GetNonVirtualPropertySetter<T>(typeof(PropertyBase<T>), "VirtualPropertyT");
            AssertVirtualSetterT(sub, setter);
        }

        private void AssertVirtualSetterT(PropertySub<T> sub, Action<T> setter)
        {
            _base = sub;
            sub.InstancePropertyT = _one;
            _base.InstancePropertyT = _one;
            setter(_two);
            Assert.That(sub.InstancePropertyT, Is.EqualTo(_one));
            Assert.That(_base.InstancePropertyT, Is.EqualTo(_two));
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
        [Test] public void GetNonVirtualPropertyAccessorOrNull_WhenNoSuchPropertyName_ReturnsNoDelegates()
        {
            var accessor = _base.GetNonVirtualPropertyAccessorOrNull<T>(typeof(PropertyBase<T>), "NoSuchProperty");
            Assert.That(accessor.Get, Is.Null);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetNonVirtualPropertyAccessor_WhenNoSuchPropertyName_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertyAccessor<T>(typeof(PropertyBase<T>), "NoSuchProperty"));
        }

        [Test] public void GetNonVirtualPropertyAccessorOrNull_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = _base.GetNonVirtualPropertyAccessorOrNull<T>(typeof(PropertyBase<T>), "InstancePropertyT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }

        [Test] public void GetNonVirtualPropertyAccessor_WhenExactMatch_ReturnsBothDelegates()
        {
            var accessor = _base.GetNonVirtualPropertyAccessorOrNull<T>(typeof(PropertyBase<T>), "InstancePropertyT");
            AssertGetterT(accessor.Get);
            AssertSetterT(accessor.Set);
        }
        [Test] public void GetNonVirtualPropertyAccessorOrNull_WhenReadOnly_ReturnsGetter()
        {
            var accessor = _base.GetNonVirtualPropertyAccessorOrNull<T>(typeof(PropertyBase<T>), "InstanceReadOnlyT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetNonVirtualPropertyAccessor_WhenReadOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertyAccessor<T>(typeof(PropertyBase<T>), "InstanceReadOnlyT"));
        }

        [Test] public void GetNonVirtualPropertyAccessorOrNull_WhenWriteOnly_ReturnsSetter()
        {
            var accessor = _base.GetNonVirtualPropertyAccessorOrNull<T>(typeof(PropertyBase<T>), "InstanceWriteOnlyT");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterT(accessor.Set);
        }

        [Test] public void GetNonVirtualPropertyAccessor_WhenWriteOnly_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertyAccessor<T>(typeof(PropertyBase<T>), "InstanceWriteOnlyT"));
        }

        [Test] public void GetNonVirtualPropertyAccessorOrNull_WhenWidenedMatch_ReturnsSetter()
        {
            if (typeof(T).IsValueType) return;

            var accessor = _base.GetNonVirtualPropertyAccessorOrNull<T>(typeof(PropertyBase<T>), "InstancePropertyObject");
            Assert.That(accessor.Get, Is.Null);
            AssertSetterObject(accessor.Set);
        }

        [Test] public void GetNonVirtualPropertyAccessor_WhenWidenedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertyAccessor<T>(typeof(PropertyBase<T>), "InstancePropertyObject"));
        }

        [Test] public void GetNonVirtualPropertyAccessorOrNull_WhenNarrowedMatch_ReturnsGetter()
        {
            if (typeof(T).IsValueType) return;
            var accessor = _base.GetNonVirtualPropertyAccessorOrNull<object>(typeof(PropertyBase<T>), "InstancePropertyT");
            AssertGetterT(accessor.Get);
            Assert.That(accessor.Set, Is.Null);
        }

        [Test] public void GetNonVirtualPropertyAccessor_WhenNarrowedMatch_ThrowsException()
        {
            if (typeof(T).IsValueType) return;
            Assert.Throws<MissingMemberException>(
                () => _base.GetNonVirtualPropertyAccessor<object>(typeof(PropertyBase<T>), "InstancePropertyT"));
        }

        [Test] public void GetNonVirtualPropertyAccessorOrNull_WhenPropertyVirtual_ReturnsAccessorCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var accessor = sub.GetNonVirtualPropertyAccessorOrNull<T>(typeof(PropertyBase<T>), "VirtualPropertyT");
            AssertVirtualGetterT(sub, accessor.Get);
            AssertVirtualSetterT(sub, accessor.Set);
        }

        [Test] public void GetNonVirtualPropertyAccessor_WhenPropertyVirtual_ReturnsAccessorCallsSubProperty()
        {
            var sub = new PropertySub<T>();
            var accessor = sub.GetNonVirtualPropertyAccessor<T>(typeof(PropertyBase<T>), "VirtualPropertyT");
            AssertVirtualGetterT(sub, accessor.Get);
            AssertVirtualSetterT(sub, accessor.Set);
        }
        #endregion accessor
    }
}