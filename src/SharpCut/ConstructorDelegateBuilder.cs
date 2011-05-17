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
    /// Responsable for building delegates for constructor
    /// </summary>
    /// <author>Kenneth Xu</author>
    internal class ConstructorDelegateBuilder
    {
        internal static TDelegate CreateConstructorDelegate<TDelegate>(Type type, bool failSafe)
            where TDelegate : class
        {
            typeof(TDelegate).AssertIsDelegate();
            MethodInfo invokeMethod = typeof(TDelegate).GetMethod("Invoke");
            ParameterInfo[] parameters = invokeMethod.GetParameters();

            var returnType = invokeMethod.ReturnType;
            if (returnType == typeof(void))
            {
                throw new InvalidOperationException(
                    String.Format(
                        "Delegate {0} has no return type. It is required that delegate return type must not be void.",
                        typeof (TDelegate)));
            }
            if (!returnType.IsAssignableFrom(type))
            {
                if (failSafe) return null;
                throw new MissingMemberException(
                    String.Format(
                        "Target type {0} is not assignable to the return type of delegate {1}.",
                        type, returnType));
            }

            Type[] types = Utils.ParameterToTypeArray(parameters, 0);
            const BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var constructor = type.GetConstructor(bindingAttr, null, types, null);
            if (constructor == null)
            {
                if (failSafe) return null;
                throw new MissingMemberException(BuildExceptionMessage(type, types));
            }
            var dynamicMethod = constructor.CreateDynamicMethod();
            try
            {
                return dynamicMethod.CreateDelegate(typeof (TDelegate)) as TDelegate;
            }
            catch(ArgumentException e)
            {
                if (failSafe) return null;
                throw new MissingMemberException(
                    BuildExceptionMessage(type, types), e);
            }
        }

        private static string BuildExceptionMessage(Type targetType, Type[] parameterTypes)
        {
            StringBuilder sb = new StringBuilder()
                .Append("No matching constructor found in the type ")
                .Append(targetType)
                .Append(" for parameter list (");
            sb.AppendArrayCommaSeparated(parameterTypes);
            sb.Append(").");
            return sb.ToString();
        }
    }
}