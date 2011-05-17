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
using System.Text;

namespace SharpCut
{
    /// <summary>
    /// Responsable for building the delegates for methods.
    /// </summary>
    /// <typeparam name="T">Type of the delegate.</typeparam>
    /// <author>Kenneth Xu</author>
    internal class MethodDelegateBuilder<T> where T : class
    {
        #region Constants
        private const BindingFlags _allStaticMethod =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod;

        private const BindingFlags _allInstanceMethod =
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod;
        #endregion

        private readonly bool _failFast;
        private readonly string _methodName;
        private readonly Type _targetType;
        private readonly object _targetObject;
        private readonly BindingFlags _bindingAttr;
        private Type _returnType;
        private Type[] _parameterTypes;

        internal Predicate<MethodInfo> MethodFilter { get; set; }
        internal string MethodFilterMessage { get; set; }

        public MethodDelegateBuilder(object targetObject, Type targetType, string methodName, bool failFast)
            : this(targetObject, targetType, methodName, failFast, _allInstanceMethod)
        {
        }

        public MethodDelegateBuilder(Type targetType, string methodName, bool failFast, bool isInstanceMethod)
            : this(null, targetType, methodName, failFast, isInstanceMethod ? _allInstanceMethod : _allStaticMethod)
        {
        }

        internal MethodDelegateBuilder(object targetObject, Type targetType, string methodName, bool failFast, BindingFlags bindingAttr)
        {
            Utils.AssertIsDelegate(typeof(T));

            _targetObject = targetObject;
            _targetType = targetType;
            _methodName = methodName;
            _failFast = failFast;
            _bindingAttr = bindingAttr;
        }

        public T CreateInvoker()
        {
            return CreateInvoker(false);
        }

        public T CreateInvoker(bool nonVirtual)
        {
            var method = GetMethod();
            if (method == null) return null;
            try
            {
                return Utils.MethodToDelegate(method, typeof(T), _targetObject, nonVirtual) as T;
            }
            catch (ArgumentException ex)
            {
                if (!_failFast) return null;
                throw new MissingMethodException(BuildExceptionMessage(), ex);
            }
        }

        private MethodInfo GetMethod()
        {
            MethodInfo invokeMethod = typeof(T).GetMethod("Invoke");
            ParameterInfo[] parameters = invokeMethod.GetParameters();
            _returnType = invokeMethod.ReturnType;
            bool instanceToStatic = (_targetObject == null && _bindingAttr == _allInstanceMethod);
            if (instanceToStatic)
            {
                if (parameters.Length == 0)
                {
                    throw new InvalidOperationException(string.Format(
                                                            "Delegate {0} has no parameter. It is required to have at least one parameter that is assignable from target type.",
                                                            typeof(T)));
                }
                Type instanceType = parameters[0].ParameterType;
                if (!_targetType.IsAssignableFrom(instanceType))
                {
                    if (!_failFast) return null;
                    throw new MissingMethodException(string.Format(
                                                         "Target type {0} is not assignable to the first parameter of delegate {1}.",
                                                         _targetType, instanceType));
                }
            }

            int offset = instanceToStatic ? 1 : 0;
            Type[] types = Utils.ParameterToTypeArray(parameters, offset);
            _parameterTypes = types;

            var method = _targetType.GetMethod(_methodName, _bindingAttr, null, _parameterTypes, null);
            var methodFilter = MethodFilter;
            if (method != null && methodFilter != null && !methodFilter(method))
            {
                method = null;
            }
            if (method == null && _failFast)
            {
                throw new MissingMethodException(BuildExceptionMessage());
            }
            return method;
        }

        private string BuildExceptionMessage()
        {
            StringBuilder sb = new StringBuilder()
                .Append("No matching method found in the type ")
                .Append(_targetType)
                .Append(" for signature ")
                .Append(_returnType).Append(" ")
                .Append(_methodName).Append("(");
            sb.AppendArrayCommaSeparated(_parameterTypes);
            sb.Append(") with binding flags: ").Append(_bindingAttr);
            if (MethodFilter != null)
            {
                sb.Append(" with filter ").Append(MethodFilterMessage ?? MethodFilter.ToString());
            }
            sb.Append(".");
            return sb.ToString();
        }
    }
}