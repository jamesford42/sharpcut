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

namespace SharpCut
{
    /// <summary>
    /// Responsable for building delegates for field
    /// </summary>
    /// <typeparam name="T">Type of the field.</typeparam>
    /// <author>Kenneth Xu</author>
    internal class FieldDelegateBuilder<T>
    {
        #region Constants
        private const BindingFlags _allStaticField =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        private const BindingFlags _allInstanceField =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        #endregion

        private readonly bool _failFast;
        private readonly string _fieldName;
        private readonly Type _targetType;
        private readonly object _targetObject;
        private readonly BindingFlags _bindingAttr;

        public FieldDelegateBuilder(object targetObject, Type targetType, string fieldName, bool failFast)
            : this(targetObject, targetType, fieldName, failFast, _allInstanceField)
        {
        }

        public FieldDelegateBuilder(Type targetType, string fieldName, bool failFast, bool isInstanceMember)
            : this(null, targetType, fieldName, failFast, isInstanceMember ? _allInstanceField : _allStaticField)
        {
        }

        internal FieldDelegateBuilder(object targetObject, Type targetType, string fieldName, bool failFast, BindingFlags bindingAttr)
        {
            _targetObject = targetObject;
            _targetType = targetType;
            _fieldName = fieldName;
            _failFast = failFast;
            _bindingAttr = bindingAttr;
        }

        public Action<TInstance, T> CreateSetter<TInstance>()
        {
            return GetSetterDelegate(typeof(Action<TInstance, T>), typeof(TInstance)) as Action<TInstance, T>;
        }

        public Action<T> CreateSetter()
        {
            return GetSetterDelegate(typeof(Action<T>), null) as Action<T>;
        }

        public Func<TInstance, T> CreateGetter<TInstance>()
        {
            return GetGetterDelegate(typeof(Func<TInstance, T>), typeof(TInstance)) as Func<TInstance, T>;
        }

        public Func<T> CreateGetter()
        {
            return GetGetterDelegate(typeof(Func<T>), null) as Func<T>;
        }

        public Accessor<TInstance, T> CreateAccessor<TInstance>()
        {
            var field = GetField();
            var getter = field == null ? null : FieldtoDelegate(field, typeof(Func<TInstance, T>), typeof(TInstance), true) as Func<TInstance, T>;
            var setter = field == null ? null : GetSetterDelegate(field, typeof(Action<TInstance, T>), typeof(TInstance)) as Action<TInstance, T>;
            return new Accessor<TInstance, T> { Get = getter, Set = setter };
        }

        public Accessor<T> CreateAccessor()
        {
            var field = GetField();
            var getter = field == null ? null : FieldtoDelegate(field, typeof(Func<T>), null, true) as Func<T>;
            var setter = field == null ? null : GetSetterDelegate(field, typeof(Action<T>), null) as Action<T>;
            return new Accessor<T> { Get = getter, Set = setter };
        }

        private Delegate GetGetterDelegate(Type type, Type instanceType)
        {
            var field = GetField();
            if (field == null) return null;
            return FieldtoDelegate(field, type, instanceType, true);
        }

        private Delegate GetSetterDelegate(Type type, Type instanceType)
        {
            var field = GetField();
            if (field == null) return null;
            return GetSetterDelegate(field, type, instanceType);
        }

        private Delegate GetSetterDelegate(FieldInfo field, Type delegateType, Type instanceType)
        {
            if (field.IsInitOnly)
            {
                if (!_failFast) return null;
                throw new MissingFieldException(
                    "Field named " + _fieldName +
                    " on type " + _targetType + " is not writable.");
            }
            return FieldtoDelegate(field, delegateType, instanceType, false);
        }

        private Delegate FieldtoDelegate(FieldInfo field, Type delegateType, Type instanceType, bool isGetter)
        {
            try
            {
                var dynamicMethod = isGetter
                                        ? field.CreateGetterMethod(typeof(T), instanceType)
                                        : field.CreateSetterMethod(typeof(T), instanceType);
                return _targetObject == null
                           ? dynamicMethod.CreateDelegate(delegateType)
                           : dynamicMethod.CreateDelegate(delegateType, _targetObject);
            }
            catch (ArgumentException ex)
            {
                if (!_failFast) return null;
                throw new MissingFieldException(
                    "Field type " + field.FieldType +
                    " does not match requested type " + typeof(T),
                    ex);
            }
        }

        private FieldInfo GetField()
        {
            var field = _targetType.GetField(_fieldName, _bindingAttr);
            if (field == null && _failFast)
            {
                throw new MissingFieldException(
                    "No field named " + _fieldName +
                    " found in the type " + _targetType +
                    " with binding flags: " + _bindingAttr + ".");
            }
            return field;
        }
    }
}