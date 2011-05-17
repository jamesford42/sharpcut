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
    /// Responsable for building delegates for properties
    /// </summary>
    /// <typeparam name="T">Type of the property value.</typeparam>
    /// <author>Kenneth Xu</author>
    internal class PropertyDelegateBuilder<T>
    {
        #region Constants
        private const BindingFlags _allStaticProperty =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        private const BindingFlags _allInstanceProperty =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        #endregion

        private readonly bool _failFast;
        private readonly string _propertyName;
        private readonly Type _targetType;
        private readonly object _targetObject;
        private readonly BindingFlags _bindingAttr;

        public PropertyDelegateBuilder(object targetObject, Type targetType, string propertyName, bool failFast)
            : this(targetObject, targetType, propertyName, failFast, _allInstanceProperty)
        {
        }

        public PropertyDelegateBuilder(Type targetType, string propertyName, bool failFast, bool isInstanceMember)
            : this(null, targetType, propertyName, failFast, isInstanceMember ? _allInstanceProperty : _allStaticProperty)
        {
        }

        internal PropertyDelegateBuilder(object targetObject, Type targetType, string propertyName, bool failFast, BindingFlags bindingAttr)
        {
            _targetObject = targetObject;
            _targetType = targetType;
            _propertyName = propertyName;
            _failFast = failFast;
            _bindingAttr = bindingAttr;
        }

        public Action<TInstance, T> CreateSetter<TInstance>(bool nonVirtual)
        {
            return GetSetterDelegate(nonVirtual, typeof(Action<TInstance, T>)) as Action<TInstance, T>;
        }

        public Action<T> CreateSetter(bool nonVirtual)
        {
            return GetSetterDelegate(nonVirtual, typeof(Action<T>)) as Action<T>;
        }

        public Func<TInstance, T> CreateGetter<TInstance>(bool nonVirtual)
        {
            return GetGetterDelegate(nonVirtual, typeof(Func<TInstance, T>)) as Func<TInstance, T>;
        }

        public Func<T> CreateGetter(bool nonVirtual)
        {
            return GetGetterDelegate(nonVirtual, typeof(Func<T>)) as Func<T>;
        }

        public Accessor<TInstance, T> CreateAccessor<TInstance>(bool nonVirtual)
        {
            var property = GetProperty();
            var getter = property == null ? null : GetGetterDelegate(property, typeof(Func<TInstance, T>), nonVirtual) as Func<TInstance, T>;
            var setter = property == null ? null : GetSetterDelegate(property, typeof(Action<TInstance, T>), nonVirtual) as Action<TInstance, T>;
            return new Accessor<TInstance, T> { Get = getter, Set = setter };
        }

        public Accessor<T> CreateAccessor(bool nonVirtual)
        {
            var property = GetProperty();
            var getter = property == null ? null : GetGetterDelegate(property, typeof(Func<T>), nonVirtual) as Func<T>;
            var setter = property == null ? null : GetSetterDelegate(property, typeof(Action<T>), nonVirtual) as Action<T>;
            return new Accessor<T>{Get = getter, Set = setter};
        }

        private Delegate GetGetterDelegate(bool nonVirtual, Type type)
        {
            var property = GetProperty();
            if (property == null) return null;
            return GetGetterDelegate(property, type, nonVirtual);
        }

        private Delegate GetGetterDelegate(PropertyInfo property, Type type, bool nonVirtual)
        {
            var getterMethod = property.GetGetMethod(true);
            if (getterMethod == null)
            {
                if (!_failFast) return null;
                throw new MissingMemberException(
                    "Property named " + _propertyName + 
                    " on type " + _targetType + " is not readable.");
            }
            return MethodToDelegate(property, getterMethod, type, nonVirtual);
        }

        private Delegate MethodToDelegate(PropertyInfo property, MethodInfo method, Type type, bool nonVirtual)
        {
            try{
                return method.MethodToDelegate(type, _targetObject, nonVirtual);
            }
            catch (ArgumentException ex)
            {
                if (!_failFast) return null;
                throw new MissingMemberException(
                    "Property type " + property.PropertyType +
                    " does not match requested type " + typeof(T),
                    ex);
            }
        }

        private Delegate GetSetterDelegate(bool nonVirtual, Type type)
        {
            var property = GetProperty();
            if (property == null) return null;
            return GetSetterDelegate(property, type, nonVirtual);
        }

        private Delegate GetSetterDelegate(PropertyInfo property, Type type, bool nonVirtual)
        {
            var setterMethod = property.GetSetMethod(true);
            if (setterMethod == null)
            {
                if (!_failFast) return null;
                throw new MissingMemberException(
                    "Property named " + _propertyName + 
                    " on type " + _targetType + " is not writable.");
            }
            return MethodToDelegate(property, setterMethod, type, nonVirtual);
        }

        private PropertyInfo GetProperty()
        {
            var property = _targetType.GetProperty(_propertyName, _bindingAttr);
            if (property == null && _failFast)
            {
                throw new MissingMemberException(
                    "No property named " + _propertyName + 
                    " found in the type " + _targetType + 
                    " with binding flags: " + _bindingAttr + ".");
            }
            return property;
        }
    }
}