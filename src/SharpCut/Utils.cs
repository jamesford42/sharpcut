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
using System.Reflection.Emit;
using System.Text;

namespace SharpCut
{
    /// <summary>
    /// Internal utility methods.
    /// </summary>
    /// <author>Kenneth Xu</author>
    internal static class Utils
    {
        internal static Delegate MethodToDelegate(this MethodInfo method, Type delegateType, object targetObject, bool nonVirtual)
        {
            if (nonVirtual && method.IsVirtual)
            {
                var dynamicMethod = CreateDynamicMethod(method);
                return targetObject == null
                           ? dynamicMethod.CreateDelegate(delegateType)
                           : dynamicMethod.CreateDelegate(delegateType, targetObject);
            }
            return targetObject == null
                       ? Delegate.CreateDelegate(delegateType, method)
                       : Delegate.CreateDelegate(delegateType, targetObject, method);
        }

        internal static DynamicMethod CreateDynamicMethod(this MethodInfo method)
        {
            Type[] types = GetParameterTypes(method);

            DynamicMethod dynamicMethod = new DynamicMethod(
                "NonVirtualInvoker_" + method.Name, method.ReturnType, types, method.DeclaringType);
            ILGenerator il = dynamicMethod.GetILGenerator();
            for (int i = 0; i < types.Length; i++) il.Emit(OpCodes.Ldarg, i);
            il.EmitCall(OpCodes.Call, method, null);
            il.Emit(OpCodes.Ret);
            return dynamicMethod;
        }

        internal static DynamicMethod CreateDynamicMethod(this ConstructorInfo constructor)
        {
            Type[] types = GetParameterTypes(constructor);

            DynamicMethod dynamicMethod = new DynamicMethod(
                "Constructor_" + constructor.Name, constructor.DeclaringType, types, constructor.DeclaringType);
            ILGenerator il = dynamicMethod.GetILGenerator();
            for (int i = 0; i < types.Length; i++) il.Emit(OpCodes.Ldarg, i);
            il.Emit(OpCodes.Newobj, constructor);
            il.Emit(OpCodes.Ret);
            return dynamicMethod;
        }

        private static Type[] GetParameterTypes(MethodBase method)
        {
            int offset = (method.IsStatic || method.IsConstructor ? 0 : 1);
            var parameters = method.GetParameters();
            int size = parameters.Length + offset;
            Type[] types = new Type[size];
            if (offset > 0) types[0] = method.DeclaringType;
            for (int i = offset; i < size; i++)
            {
                types[i] = parameters[i - offset].ParameterType;
            }
            return types;
        }

        internal static DynamicMethod CreateGetterMethod(this FieldInfo field, Type fieldType, Type instanceType)
        {
            var isStatic = field.IsStatic;
            var types = isStatic ? Type.EmptyTypes : new[]{field.DeclaringType};
            DynamicMethod dynamicMethod = new DynamicMethod(
                "FieldGetter_" + field.Name, field.FieldType, types, field.DeclaringType);
            ILGenerator il = dynamicMethod.GetILGenerator();
            if (!isStatic) il.Emit(OpCodes.Ldarg, 0);
            if (isStatic) il.Emit(OpCodes.Ldsfld, field);
            else il.Emit(OpCodes.Ldfld, field);
            il.Emit(OpCodes.Ret);
            return dynamicMethod;
        }

        internal static DynamicMethod CreateSetterMethod(this FieldInfo field, Type fieldType, Type instanceType)
        {
            var isStatic = field.IsStatic;
            var types = isStatic
                            ? new[] { field.FieldType }
                            : new[] { field.DeclaringType, field.FieldType };
            DynamicMethod dynamicMethod = new DynamicMethod(
                "FieldSetter_" + field.Name, typeof(void), types, field.DeclaringType);
            ILGenerator il = dynamicMethod.GetILGenerator();
            for (int i = 0; i < types.Length; i++) il.Emit(OpCodes.Ldarg, i);
            if (isStatic) il.Emit(OpCodes.Stsfld, field);
            else il.Emit(OpCodes.Stfld, field);
            il.Emit(OpCodes.Ret);
            return dynamicMethod;
        }

        internal static void AssertIsDelegate(this Type delegateType)
        {
            if (!typeof(MulticastDelegate).IsAssignableFrom(delegateType))
            {
                throw new InvalidOperationException(
                    "Expecting type parameter to be a Delegate type, but got " +
                    delegateType.FullName);
            }
        }

        internal static Type[] ParameterToTypeArray(ParameterInfo[] parameters, int offset)
        {
            int size = parameters.Length - offset;

            Type[] types = new Type[size];
            for (int i = 0; i < size; i++)
            {
                types[i] = parameters[i + offset].ParameterType;
            }
            return types;
        }

        internal static void AppendArrayCommaSeparated(this StringBuilder sb, object[] objects)
        {
            if (objects.Length > 0)
            {
                foreach (Type o in objects)
                {
                    sb.Append(o).Append(", ");
                }
                sb.Length -= 2;
            }
        }
    }
}