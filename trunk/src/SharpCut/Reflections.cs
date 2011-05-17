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
    /// Utility methods for common reflection tasks.
    /// </summary>
    /// <author>Kenneth Xu</author>
    public static class Reflections
    {
        #region Static Field

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TValue}"/> that can be used to get the value of
        /// static field with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The field type must be compatible with
        /// <typeparamref name="TValue"/>. Returns null if the field doesn't
        /// exist.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{TValue}"/> or null when
        /// the field doesn't exist.
        /// </returns>
        /// <seealso cref="GetStaticFieldGetter{TValue}"/>
        /// <seealso cref="GetStaticFieldSetterOrNull{TValue}"/>
        public static Func<TValue> GetStaticFieldGetterOrNull<TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, false, false).CreateGetter();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TValue}"/> that can be used to get the value of
        /// static field with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The field type must be compatible with
        /// <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When the field doesn't exist.
        /// </exception>
        /// <seealso cref="GetStaticFieldGetterOrNull{TValue}"/>
        /// <seealso cref="GetStaticFieldSetter{TValue}"/>
        public static Func<TValue> GetStaticFieldGetter<TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, true, false).CreateGetter();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{TValue}"/> that can be used to set the value of
        /// static field with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The field type must be compatible with
        /// <typeparamref name="TValue"/>. Returns null if the field doesn't
        /// exist.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{TValue}"/> or null when
        /// the field doesn't exist.
        /// </returns>
        /// <seealso cref="GetStaticFieldSetter{TValue}"/>
        /// <seealso cref="GetStaticFieldGetterOrNull{TValue}"/>
        public static Action<TValue> GetStaticFieldSetterOrNull<TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, false, false).CreateSetter();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{TValue}"/> that can be used to set the value of
        /// static field with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The field type must be compatible with
        /// <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When the field doesn't exist.
        /// </exception>
        /// <seealso cref="GetStaticFieldSetterOrNull{TValue}"/>
        /// <seealso cref="GetStaticFieldGetter{TValue}"/>
        public static Action<TValue> GetStaticFieldSetter<TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, true, false).CreateSetter();
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{TValue}"/> that
        /// can be used to access the value of static field with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// field type must be compatible with <typeparamref name="TValue"/>.
        /// The <see cref="Accessor{T}.Get"/>, or <see cref="Accessor{T}.Set"/>,
        /// or both could be null when there is no matching field getter
        /// and/or setter was found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{TValue}"/> for field.
        /// </returns>
        /// <seealso cref="GetStaticFieldAccessor{TValue}"/>
        public static Accessor<TValue> GetStaticFieldAccessorOrNull<TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, false, false).CreateAccessor();
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{TValue}"/> that
        /// can be used to access the value of static field with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// field type must be compatible with <typeparamref name="TValue"/>
        /// and has both getter and setter.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{TValue}"/> for field.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching field or the field missing getter
        /// or setter.
        /// </exception>
        /// <seealso cref="GetStaticFieldAccessorOrNull{TValue}"/>
        public static Accessor<TValue> GetStaticFieldAccessor<TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, true, false).CreateAccessor();
        }

        #endregion Static Field

        #region Instance Field Untargeted
        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T, TValue}"/> that can be used to get the value of
        /// instance field with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The field type must be compatible with
        /// <typeparamref name="TValue"/>. Returns null if no match is found.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to get
        /// the field value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{T, TValue}"/> or null when
        /// no matching field.
        /// </returns>
        /// <seealso cref="GetInstanceFieldGetter{T, TValue}"/>
        /// <seealso cref="GetInstanceFieldSetterOrNull{T, TValue}"/>
        public static Func<T, TValue> GetInstanceFieldGetterOrNull<T, TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, false, true).CreateGetter<T>();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T, TValue}"/> that can be used to get the value of
        /// instance field with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The field type must be compatible with
        /// <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to get
        /// the field value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{T, TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching field.
        /// </exception>
        /// <seealso cref="GetInstanceFieldGetterOrNull{T, TValue}"/>
        /// <seealso cref="GetInstanceFieldSetter{T, TValue}"/>
        public static Func<T, TValue> GetInstanceFieldGetter<T, TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, true, true).CreateGetter<T>();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{T, TValue}"/> that can be used to set the value
        /// of instance field with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The field type must be compatible with
        /// <typeparamref name="TValue"/>. Returns null if no match is found.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to set
        /// the field value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{T, TValue}"/> or null when
        /// no matching field.
        /// </returns>
        /// <seealso cref="GetInstanceFieldSetter{T, TValue}"/>
        /// <seealso cref="GetInstanceFieldGetterOrNull{T, TValue}"/>
        public static Action<T, TValue> GetInstanceFieldSetterOrNull<T, TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, false, true).CreateSetter<T>();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{T, TValue}"/> that can be used to set the value
        /// of instance field with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The field type must be compatible with
        /// <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to set
        /// the field value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{T, TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching field.
        /// </exception>
        /// <seealso cref="GetInstanceFieldSetterOrNull{T, TValue}"/>
        /// <seealso cref="GetInstanceFieldGetter{T, TValue}"/>
        public static Action<T, TValue> GetInstanceFieldSetter<T, TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, true, true).CreateSetter<T>();
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{T, TValue}"/> that
        /// can be used to access the value of instance field with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// field type must be compatible with <typeparamref name="TValue"/>.
        /// The <see cref="Accessor{T}.Get"/>, or <see cref="Accessor{T}.Set"/>,
        /// or both could be null when there is no matching field was found.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can use the result accessor to set and/or
        /// get the field value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{T, TValue}"/> for field.
        /// </returns>
        /// <seealso cref="GetInstanceFieldAccessor{T,TValue}"/>
        public static Accessor<T, TValue> GetInstanceFieldAccessorOrNull<T, TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, false, true).CreateAccessor<T>();
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{T, TValue}"/> that
        /// can be used to access the value of instance field with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// field type must be compatible with <typeparamref name="TValue"/>
        /// and the field is not read only.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can use the result accessor to set and/or
        /// get the field value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible field.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{T, TValue}"/> for field.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching field or the field is read only.
        /// </exception>
        /// <seealso cref="GetInstanceFieldAccessorOrNull{T,TValue}"/>
        public static Accessor<T, TValue> GetInstanceFieldAccessor<T, TValue>(this Type type, string name)
        {
            return new FieldDelegateBuilder<TValue>(type, name, true, true).CreateAccessor<T>();
        }
        #endregion Instance Field Untargeted

        #region Instance Field Targeted
        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TValue}"/> that can be used to get the value of
        /// instance field with given <paramref name="name"/> on the given 
        /// instance <paramref name="obj"/>. The field type must be compatible
        /// with <typeparamref name="TValue"/>. Returns null if no match is
        /// found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result field getter is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{TValue}"/> or null when
        /// no matching field.
        /// </returns>
        /// <seealso cref="GetInstanceFieldGetter{TValue}"/>
        /// <seealso cref="GetInstanceFieldSetterOrNull{TValue}"/>
        public static Func<TValue> GetInstanceFieldGetterOrNull<TValue>(this object obj, string name)
        {
            return new FieldDelegateBuilder<TValue>(obj, obj.GetType(), name, false).CreateGetter();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TValue}"/> that can be used to get the value of
        /// instance field with given <paramref name="name"/> on the given
        /// instance <paramref name="obj"/>. The field type must be compatible
        /// with <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result field getter is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching field.
        /// </exception>
        /// <seealso cref="GetInstanceFieldGetterOrNull{TValue}"/>
        /// <seealso cref="GetInstanceFieldSetter{TValue}"/>
        public static Func<TValue> GetInstanceFieldGetter<TValue>(this object obj, string name)
        {
            return new FieldDelegateBuilder<TValue>(obj, obj.GetType(), name, true).CreateGetter();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{TValue}"/> that can be used to set the value of
        /// instance field with given <paramref name="name"/> on the given
        /// instance <paramref name="obj"/>. The field type must be compatible
        /// with <typeparamref name="TValue"/>. Returns null if no match is
        /// found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result field setter is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{TValue}"/> or null when
        /// no matching field or field is readonly.
        /// </returns>
        /// <seealso cref="GetInstanceFieldSetter{TValue}"/>
        /// <seealso cref="GetInstanceFieldGetterOrNull{TValue}"/>
        public static Action<TValue> GetInstanceFieldSetterOrNull<TValue>(this object obj, string name)
        {
            return new FieldDelegateBuilder<TValue>(obj, obj.GetType(), name, false).CreateSetter();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{TValue}"/> that can be used to set the value of
        /// instance field with given <paramref name="name"/> on the given
        /// instance <paramref name="obj"/>. The field type must be compatible
        /// with <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result field setter is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching field or field is read only.
        /// </exception>
        /// <seealso cref="GetInstanceFieldSetterOrNull{TValue}"/>
        /// <seealso cref="GetInstanceFieldGetter{TValue}"/>
        public static Action<TValue> GetInstanceFieldSetter<TValue>(this object obj, string name)
        {
            return new FieldDelegateBuilder<TValue>(obj, obj.GetType(), name, true).CreateSetter();
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{TValue}"/> that
        /// can be used to access the value of instance field with given
        /// <paramref name="name"/> on the given instance <paramref name="obj"/>. The
        /// field type must be compatible with <typeparamref name="TValue"/>.
        /// The <see cref="Accessor{T}.Get"/>, or <see cref="Accessor{T}.Set"/>,
        /// or both could be null when there is no matching field was found or
        /// the field is read only.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result field accessor is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{TValue}"/> for field.
        /// </returns>
        /// <seealso cref="GetInstanceFieldAccessor{TValue}"/>
        public static Accessor<TValue> GetInstanceFieldAccessorOrNull<TValue>(this object obj, string name)
        {
            return new FieldDelegateBuilder<TValue>(obj, obj.GetType(), name, false).CreateAccessor();
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{TValue}"/> that
        /// can be used to access the value of instance field with given
        /// <paramref name="name"/> on the given instance <paramref name="obj"/>. The
        /// field type must be compatible with <typeparamref name="TValue"/>
        /// and has both getter and setter.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the field value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result field accessor is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the field.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{TValue}"/> for field.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching field or the field is read only.
        /// </exception>
        /// <seealso cref="GetInstanceFieldAccessorOrNull{TValue}"/>
        public static Accessor<TValue> GetInstanceFieldAccessor<TValue>(this object obj, string name)
        {
            return new FieldDelegateBuilder<TValue>(obj, obj.GetType(), name, true).CreateAccessor();
        }
        #endregion Instance Field Targeted

        #region Static Property

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TValue}"/> that can be used to get the value of
        /// static property with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The property type must be compatible with
        /// <typeparamref name="TValue"/>. Returns null if no match is found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{TValue}"/> or null when
        /// no matching property getter.
        /// </returns>
        /// <seealso cref="GetStaticPropertyGetter{TValue}"/>
        /// <seealso cref="GetStaticPropertySetterOrNull{TValue}"/>
        public static Func<TValue> GetStaticPropertyGetterOrNull<TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, false, false).CreateGetter(false);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TValue}"/> that can be used to get the value of
        /// static property with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The property type must be compatible
        /// with <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property getter.
        /// </exception>
        /// <seealso cref="GetStaticPropertyGetterOrNull{TValue}"/>
        /// <seealso cref="GetStaticPropertySetter{TValue}"/>
        public static Func<TValue> GetStaticPropertyGetter<TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, true, false).CreateGetter(false);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{TValue}"/> that can be used to set the value of
        /// static property with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The property type must be compatible
        /// with <typeparamref name="TValue"/>. Returns null no match is found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{TValue}"/> or null when
        /// no matching property setter.
        /// </returns>
        /// <seealso cref="GetStaticPropertySetter{TValue}"/>
        /// <seealso cref="GetStaticPropertyGetterOrNull{TValue}"/>
        public static Action<TValue> GetStaticPropertySetterOrNull<TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, false, false).CreateSetter(false);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{TValue}"/> that can be used to set the value of
        /// static property with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The property type must be compatible with
        /// <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property setter.
        /// </exception>
        /// <seealso cref="GetStaticPropertySetterOrNull{TValue}"/>
        /// <seealso cref="GetStaticPropertyGetter{TValue}"/>
        public static Action<TValue> GetStaticPropertySetter<TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, true, false).CreateSetter(false);
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{TValue}"/> that
        /// can be used to access the value of static property with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>.
        /// The <see cref="Accessor{T}.Get"/>, or <see cref="Accessor{T}.Set"/>,
        /// or both could be null when there is no matching property getter
        /// and/or setter was found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{TValue}"/> for property.
        /// </returns>
        /// <seealso cref="GetStaticPropertyAccessor{TValue}"/>
        public static Accessor<TValue> GetStaticPropertyAccessorOrNull<TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, false, false).CreateAccessor(false);
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{TValue}"/> that
        /// can be used to access the value of static property with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>
        /// and has both getter and setter.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{TValue}"/> for property.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property or the property missing getter
        /// or setter.
        /// </exception>
        /// <seealso cref="GetStaticPropertyAccessorOrNull{TValue}"/>
        public static Accessor<TValue> GetStaticPropertyAccessor<TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, true, false).CreateAccessor(false);
        }

        #endregion Static Property

        #region Instance Property Untargeted
        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T, TValue}"/> that can be used to get the value of
        /// instance property with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The property type must be compatible with
        /// <typeparamref name="TValue"/>. Returns null if no match is found.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to get
        /// the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{T, TValue}"/> or null when
        /// no matching property getter.
        /// </returns>
        /// <seealso cref="GetInstancePropertyGetter{T, TValue}"/>
        /// <seealso cref="GetInstancePropertySetterOrNull{T, TValue}"/>
        public static Func<T, TValue> GetInstancePropertyGetterOrNull<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, false, true).CreateGetter<T>(false);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T, TValue}"/> that can be used to get the value of
        /// instance property with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The property type must be compatible with
        /// <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to get
        /// the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{T, TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property getter.
        /// </exception>
        /// <seealso cref="GetInstancePropertyGetterOrNull{T, TValue}"/>
        /// <seealso cref="GetInstancePropertySetter{T, TValue}"/>
        public static Func<T, TValue> GetInstancePropertyGetter<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, true, true).CreateGetter<T>(false);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{T, TValue}"/> that can be used to set the value
        /// of instance property with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The property type must be compatible with
        /// <typeparamref name="TValue"/>. Returns null if no match is found.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to set
        /// the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{T, TValue}"/> or null when
        /// no matching property setter.
        /// </returns>
        /// <seealso cref="GetInstancePropertySetter{T, TValue}"/>
        /// <seealso cref="GetInstancePropertyGetterOrNull{T, TValue}"/>
        public static Action<T, TValue> GetInstancePropertySetterOrNull<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, false, true).CreateSetter<T>(false);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{T, TValue}"/> that can be used to set the value
        /// of instance property with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The property type must be compatible with
        /// <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to set
        /// the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{T, TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property setter.
        /// </exception>
        /// <seealso cref="GetInstancePropertySetterOrNull{T, TValue}"/>
        /// <seealso cref="GetInstancePropertyGetter{T, TValue}"/>
        public static Action<T, TValue> GetInstancePropertySetter<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, true, true).CreateSetter<T>(false);
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{T, TValue}"/> that
        /// can be used to access the value of instance property with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>.
        /// The <see cref="Accessor{T}.Get"/>, or <see cref="Accessor{T}.Set"/>,
        /// or both could be null when there is no matching property getter
        /// and/or setter was found.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can use the result accessor to set and/or
        /// get the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{T, TValue}"/> for property.
        /// </returns>
        /// <seealso cref="GetInstancePropertyAccessor{T,TValue}"/>
        public static Accessor<T, TValue> GetInstancePropertyAccessorOrNull<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, false, true).CreateAccessor<T>(false);
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{T, TValue}"/> that
        /// can be used to access the value of instance property with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>
        /// and has both getter and setter.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can use the result accessor to set and/or
        /// get the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{T, TValue}"/> for property.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property or the property missing getter
        /// or setter.
        /// </exception>
        /// <seealso cref="GetInstancePropertyAccessorOrNull{T,TValue}"/>
        public static Accessor<T, TValue> GetInstancePropertyAccessor<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, true, true).CreateAccessor<T>(false);
        }
        #endregion Instance Property Untargeted

        #region Non Virtaul Property Untargeted
        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T, TValue}"/> that can be used to make non
        /// virtual read from the value of instance property with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>.
        /// Returns null if no match is found.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to get
        /// the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{T, TValue}"/> or null when
        /// no matching property getter.
        /// </returns>
        /// <seealso cref="GetNonVirtualPropertyGetter{T, TValue}"/>
        /// <seealso cref="GetNonVirtualPropertySetterOrNull{T, TValue}"/>
        public static Func<T, TValue> GetNonVirtualPropertyGetterOrNull<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, false, true).CreateGetter<T>(true);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T, TValue}"/> that can be used to make non
        /// virtual read from the value of instance property with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to get
        /// the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{T, TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property getter.
        /// </exception>
        /// <seealso cref="GetNonVirtualPropertyGetterOrNull{T, TValue}"/>
        /// <seealso cref="GetNonVirtualPropertySetter{T, TValue}"/>
        public static Func<T, TValue> GetNonVirtualPropertyGetter<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, true, true).CreateGetter<T>(true);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{T, TValue}"/> that can be used to make non
        /// virtual write to the value of instance property with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>.
        /// Returns null if no match is found.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to set
        /// the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{T, TValue}"/> or null when
        /// no matching property setter.
        /// </returns>
        /// <seealso cref="GetNonVirtualPropertySetter{T, TValue}"/>
        /// <seealso cref="GetNonVirtualPropertyGetterOrNull{T, TValue}"/>
        public static Action<T, TValue> GetNonVirtualPropertySetterOrNull<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, false, true).CreateSetter<T>(true);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{T, TValue}"/> that can be used to make non
        /// virtual write to the value of instance property with given
        /// <paramref name="name"/> of given <paramref name="type"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can be passed to the result delegate to set
        /// the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{T, TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property setter.
        /// </exception>
        /// <seealso cref="GetNonVirtualPropertySetterOrNull{T, TValue}"/>
        /// <seealso cref="GetNonVirtualPropertyGetter{T, TValue}"/>
        public static Action<T, TValue> GetNonVirtualPropertySetter<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, true, true).CreateSetter<T>(true);
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{T, TValue}"/>
        /// that can be used to make non virtual access to the value of
        /// instance property with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The property type must be compatible with
        /// <typeparamref name="TValue"/>. The <see cref="Accessor{T}.Get"/>,
        /// or <see cref="Accessor{T}.Set"/>, or both could be null when there
        /// is no matching property getter and/or setter was found.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can use the result accessor to set and/or
        /// get the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{T, TValue}"/> for property.
        /// </returns>
        /// <seealso cref="GetNonVirtualPropertyAccessor{T,TValue}"/>
        public static Accessor<T, TValue> GetNonVirtualPropertyAccessorOrNull<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, false, true).CreateAccessor<T>(true);
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{T, TValue}"/>
        /// that can be used to make non virtual access to the value of
        /// instance property with given <paramref name="name"/> of given
        /// <paramref name="type"/>. The property type must be compatible with
        /// <typeparamref name="TValue"/> and has both getter and setter.
        /// </summary>
        /// <typeparam name="T">
        /// Type of the object that can use the result accessor to set and/or
        /// get the property value.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{T, TValue}"/> for property.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property or the property missing getter
        /// or setter.
        /// </exception>
        /// <seealso cref="GetNonVirtualPropertyAccessorOrNull{T,TValue}"/>
        public static Accessor<T, TValue> GetNonVirtualPropertyAccessor<T, TValue>(this Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(type, name, true, true).CreateAccessor<T>(true);
        }
        #endregion Non Virtual Property Untargeted

        #region Instance Property Targeted
        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TValue}"/> that can be used to get the value of
        /// instance property with given <paramref name="name"/> on the given 
        /// instance <paramref name="obj"/>. The property type must be
        /// compatible with <typeparamref name="TValue"/>. Returns null if no
        /// match is found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{TValue}"/> or null when
        /// no matching property getter.
        /// </returns>
        /// <seealso cref="GetInstancePropertyGetter{TValue}"/>
        /// <seealso cref="GetInstancePropertySetterOrNull{TValue}"/>
        public static Func<TValue> GetInstancePropertyGetterOrNull<TValue>(this object obj, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, obj.GetType(), name, false).CreateGetter(false);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TValue}"/> that can be used to get the value of
        /// instance property with given <paramref name="name"/> on the given
        /// instance <paramref name="obj"/>. The property type must be
        /// compatible with <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property getter.
        /// </exception>
        /// <seealso cref="GetInstancePropertyGetterOrNull{TValue}"/>
        /// <seealso cref="GetInstancePropertySetter{TValue}"/>
        public static Func<TValue> GetInstancePropertyGetter<TValue>(this object obj, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, obj.GetType(), name, true).CreateGetter(false);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{TValue}"/> that can be used to set the value of
        /// instance property with given <paramref name="name"/> on the given
        /// instance <paramref name="obj"/>. The property type must be
        /// compatible with <typeparamref name="TValue"/>. Returns null if no
        /// match is found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{TValue}"/> or null when
        /// no matching property setter.
        /// </returns>
        /// <seealso cref="GetInstancePropertySetter{TValue}"/>
        /// <seealso cref="GetInstancePropertyGetterOrNull{TValue}"/>
        public static Action<TValue> GetInstancePropertySetterOrNull<TValue>(this object obj, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, obj.GetType(), name, false).CreateSetter(false);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{TValue}"/> that can be used to set the value of
        /// instance property with given <paramref name="name"/> on the given
        /// instance <paramref name="obj"/>. The property type must be
        /// compatible with <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property setter.
        /// </exception>
        /// <seealso cref="GetInstancePropertySetterOrNull{TValue}"/>
        /// <seealso cref="GetInstancePropertyGetter{TValue}"/>
        public static Action<TValue> GetInstancePropertySetter<TValue>(this object obj, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, obj.GetType(), name, true).CreateSetter(false);
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{TValue}"/> that
        /// can be used to access the value of instance property with given
        /// <paramref name="name"/> on the given instance <paramref name="obj"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>.
        /// The <see cref="Accessor{T}.Get"/>, or <see cref="Accessor{T}.Set"/>,
        /// or both could be null when there is no matching property getter
        /// and/or setter was found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{TValue}"/> for property.
        /// </returns>
        /// <seealso cref="GetInstancePropertyAccessor{TValue}"/>
        public static Accessor<TValue> GetInstancePropertyAccessorOrNull<TValue>(this object obj, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, obj.GetType(), name, false).CreateAccessor(false);
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{TValue}"/> that
        /// can be used to access the value of instance property with given
        /// <paramref name="name"/> on the given instance <paramref name="obj"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>
        /// and has both getter and setter.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{TValue}"/> for property.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property or the property missing getter
        /// or setter.
        /// </exception>
        /// <seealso cref="GetInstancePropertyAccessorOrNull{TValue}"/>
        public static Accessor<TValue> GetInstancePropertyAccessor<TValue>(this object obj, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, obj.GetType(), name, true).CreateAccessor(false);
        }
        #endregion Instance Property Targeted

        #region Non Virtaul Property Targeted
        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TValue}"/> that can be used to make non virtual
        /// read from the value of instance property with given
        /// <paramref name="name"/> defined in given <paramref name="type"/> on
        /// the given instance <paramref name="obj"/>. The property type must
        /// be compatible with <typeparamref name="TValue"/>. Returns null if
        /// no match is found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{TValue}"/> or null when
        /// no matching property getter.
        /// </returns>
        /// <seealso cref="GetNonVirtualPropertyGetter{TValue}"/>
        /// <seealso cref="GetNonVirtualPropertySetterOrNull{TValue}"/>
        public static Func<TValue> GetNonVirtualPropertyGetterOrNull<TValue>(this object obj, Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, type, name, false).CreateGetter(true);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TValue}"/> that can be used to make non virtual
        /// read from the value of instance property with given
        /// <paramref name="name"/> defined in given <paramref name="type"/> on
        /// the given instance <paramref name="obj"/>. The property type must
        /// be compatible with <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Func{TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property getter.
        /// </exception>
        /// <seealso cref="GetNonVirtualPropertyGetterOrNull{TValue}"/>
        /// <seealso cref="GetNonVirtualPropertySetter{TValue}"/>
        public static Func<TValue> GetNonVirtualPropertyGetter<TValue>(this object obj, Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, type, name, true).CreateGetter(true);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{TValue}"/> that can be used to make non virtual
        /// write to the value of instance property with given
        /// <paramref name="name"/> defined in given <paramref name="type"/> on
        /// the given instance <paramref name="obj"/>. The property type must
        /// be compatible with <typeparamref name="TValue"/>. Returns null no
        /// match is found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{TValue}"/> or null when
        /// no matching property setter.
        /// </returns>
        /// <seealso cref="GetNonVirtualPropertySetter{TValue}"/>
        /// <seealso cref="GetNonVirtualPropertyGetterOrNull{TValue}"/>
        public static Action<TValue> GetNonVirtualPropertySetterOrNull<TValue>(this object obj, Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, type, name, false).CreateSetter(true);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Action{TValue}"/> that can be used to make non virtual
        /// write to the value of instance property with given
        /// <paramref name="name"/> defined in given <paramref name="type"/> on
        /// the given instance <paramref name="obj"/>. The property type must
        /// be compatible with <typeparamref name="TValue"/>.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// A delegate of type <see cref="Action{TValue}"/>.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property setter.
        /// </exception>
        /// <seealso cref="GetNonVirtualPropertySetterOrNull{TValue}"/>
        /// <seealso cref="GetNonVirtualPropertyGetter{TValue}"/>
        public static Action<TValue> GetNonVirtualPropertySetter<TValue>(this object obj, Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, type, name, true).CreateSetter(true);
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{TValue}"/> that
        /// can be used to make non virtual access to the value of instance
        /// property with given <paramref name="name"/> defined in given
        /// <paramref name="type"/> on the given instance <paramref name="obj"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>.
        /// The <see cref="Accessor{T}.Get"/>, or <see cref="Accessor{T}.Set"/>,
        /// or both could be null when there is no matching property getter
        /// and/or setter was found.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{TValue}"/> for property.
        /// </returns>
        /// <seealso cref="GetNonVirtualPropertyAccessor{TValue}"/>
        public static Accessor<TValue> GetNonVirtualPropertyAccessorOrNull<TValue>(this object obj, Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, type, name, false).CreateAccessor(true);
        }

        /// <summary>
        /// Extension method to obtain an <see cref="Accessor{TValue}"/> that
        /// can be used to make non virtual access to the value of instance
        /// property with given <paramref name="name"/> defined in given
        /// <paramref name="type"/> on the given instance <paramref name="obj"/>. The
        /// property type must be compatible with <typeparamref name="TValue"/>
        /// and has both getter and setter.
        /// </summary>
        /// <typeparam name="TValue">
        /// Type of a the property value.
        /// </typeparam>
        /// <param name="obj">
        /// The instance of object with which the result property accessor is 
        /// associated.
        /// </param>
        /// <param name="type">
        /// The type to locate the compatible property.
        /// </param>
        /// <param name="name">
        /// The name of the property.
        /// </param>
        /// <returns>
        /// An accessor of type <see cref="Accessor{TValue}"/> for property.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching property or the property missing getter
        /// or setter.
        /// </exception>
        /// <seealso cref="GetNonVirtualPropertyAccessorOrNull{TValue}"/>
        public static Accessor<TValue> GetNonVirtualPropertyAccessor<TValue>(this object obj, Type type, string name)
        {
            return new PropertyDelegateBuilder<TValue>(obj, type, name, true).CreateAccessor(true);
        }
        #endregion Non Virtual Property Targeted

        #region Constructor

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <typeparamref name="TDelegate"/> that can be used to invoke the
        /// constructor of the given <paramref name="type"/>. The parameter
        /// list of the delegate must match the parameters of the constructor.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of the delegate that is compatible with a constructor.
        /// </typeparam>
        /// <param name="type">
        /// The actaully type to be constructed by the result delegate.
        /// </param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// Or null when there is no matching constructor on <paramref name="type"/>.
        /// </returns>
        public static TDelegate GetConstructorDelegateOrNull<TDelegate>(this Type type)
            where TDelegate : class
        {
            return ConstructorDelegateBuilder.CreateConstructorDelegate<TDelegate>(type, true);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <typeparamref name="TDelegate"/> that can be used to invoke the
        /// constructor of the given <paramref name="type"/>. The parameter
        /// list of the delegate must match the parameters of the constructor.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of the delegate that is compatible with a constructor.
        /// </typeparam>
        /// <param name="type">
        /// The actaully type to be constructed by the result delegate.
        /// </param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching constructor on <paramref name="type"/>.
        /// </exception>
        public static TDelegate GetConstructorDelegate<TDelegate>(this Type type)
            where TDelegate : class
        {
            return ConstructorDelegateBuilder.CreateConstructorDelegate<TDelegate>(type, false);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TResult}"/> that can be used to invoke the default
        /// constructor of the given <paramref name="type"/>.
        /// </summary>
        /// <typeparam name="TResult">
        /// Type of the delegate's return value, which needs to be compatible
        /// with <paramref name="type"/>.
        /// </typeparam>
        /// <param name="type">The actual type to be constructed.</param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// Or null when there is no matching constructor on <paramref name="type"/>.
        /// </returns>
        public static Func<TResult> GetConstructorOrNull<TResult>(this Type type)
        {
            return GetConstructorDelegateOrNull<Func<TResult>>(type);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{TResult}"/> that can be used to invoke the default
        /// constructor of the given <paramref name="type"/>.
        /// </summary>
        /// <typeparam name="TResult">
        /// Type of the delegate's return value, which needs to be compatible
        /// with <paramref name="type"/>.
        /// </typeparam>
        /// <param name="type">The actual type to be constructed.</param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching constructor on <paramref name="type"/>.
        /// </exception>
        public static Func<TResult> GetConstructor<TResult>(this Type type)
        {
            return GetConstructorDelegate<Func<TResult>>(type);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T,TResult}"/> that can be used to invoke the
        /// constructor of the given <paramref name="type"/>. The parameter
        /// the delegate must match the parameter of the constructor.
        /// </summary>
        /// <typeparam name="T">Type of first parameter.</typeparam>
        /// <typeparam name="TResult">
        /// Type of the delegate's return value, which needs to be compatible
        /// with <paramref name="type"/>.
        /// </typeparam>
        /// <param name="type">The actual type to be constructed.</param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// Or null when there is no matching constructor on <paramref name="type"/>.
        /// </returns>
        public static Func<T, TResult> GetConstructorOrNull<T, TResult>(this Type type)
        {
            return GetConstructorDelegateOrNull<Func<T, TResult>>(type);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T,TResult}"/> that can be used to invoke the
        /// constructor of the given <paramref name="type"/>. The parameter
        /// the delegate must match the parameter of the constructor.
        /// </summary>
        /// <typeparam name="T">Type of first parameter.</typeparam>
        /// <typeparam name="TResult">
        /// Type of the delegate's return value, which needs to be compatible
        /// with <paramref name="type"/>.
        /// </typeparam>
        /// <param name="type">The actual type to be constructed.</param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching constructor on <paramref name="type"/>.
        /// </exception>
        public static Func<T, TResult> GetConstructor<T, TResult>(this Type type)
        {
            return GetConstructorDelegate<Func<T, TResult>>(type);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T1,T2,TResult}"/> that can be used to invoke the
        /// constructor of the given <paramref name="type"/>. The parameter
        /// list of the delegate must match the parameters of the constructor.
        /// </summary>
        /// <typeparam name="T1">Type of first parameter.</typeparam>
        /// <typeparam name="T2">Type of second parameter.</typeparam>
        /// <typeparam name="TResult">
        /// Type of the delegate's return value, which needs to be compatible
        /// with <paramref name="type"/>.
        /// </typeparam>
        /// <param name="type">The actual type to be constructed.</param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// Or null when there is no matching constructor on <paramref name="type"/>.
        /// </returns>
        public static Func<T1, T2, TResult> GetConstructorOrNull<T1, T2, TResult>(this Type type)
        {
            return GetConstructorDelegateOrNull<Func<T1, T2, TResult>>(type);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T1,T2,TResult}"/> that can be used to invoke the
        /// constructor of the given <paramref name="type"/>. The parameter
        /// list of the delegate must match the parameters of the constructor.
        /// </summary>
        /// <typeparam name="T1">Type of first parameter.</typeparam>
        /// <typeparam name="T2">Type of second parameter.</typeparam>
        /// <typeparam name="TResult">
        /// Type of the delegate's return value, which needs to be compatible
        /// with <paramref name="type"/>.
        /// </typeparam>
        /// <param name="type">The actual type to be constructed.</param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching constructor on <paramref name="type"/>.
        /// </exception>
        public static Func<T1, T2, TResult> GetConstructor<T1, T2, TResult>(this Type type)
        {
            return GetConstructorDelegate<Func<T1, T2, TResult>>(type);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T1,T2,T3,TResult}"/> that can be used to invoke
        /// the constructor of the given <paramref name="type"/>. The parameter
        /// list of the delegate must match the parameters of the constructor.
        /// </summary>
        /// <typeparam name="T1">Type of first parameter.</typeparam>
        /// <typeparam name="T2">Type of second parameter.</typeparam>
        /// <typeparam name="T3">Type of third parameter.</typeparam>
        /// <typeparam name="TResult">
        /// Type of the delegate's return value, which needs to be compatible
        /// with <paramref name="type"/>.
        /// </typeparam>
        /// <param name="type">The actual type to be constructed.</param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// Or null when there is no matching constructor on <paramref name="type"/>.
        /// </returns>
        public static Func<T1, T2, T3, TResult> GetConstructorOrNull<T1, T2, T3, TResult>(this Type type)
        {
            return GetConstructorDelegateOrNull<Func<T1, T2, T3, TResult>>(type);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T1,T2,T3,TResult}"/> that can be used to invoke
        /// the constructor of the given <paramref name="type"/>. The parameter
        /// list of the delegate must match the parameters of the constructor.
        /// </summary>
        /// <typeparam name="T1">Type of first parameter.</typeparam>
        /// <typeparam name="T2">Type of second parameter.</typeparam>
        /// <typeparam name="T3">Type of third parameter.</typeparam>
        /// <typeparam name="TResult">
        /// Type of the delegate's return value, which needs to be compatible
        /// with <paramref name="type"/>.
        /// </typeparam>
        /// <param name="type">The actual type to be constructed.</param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching constructor on <paramref name="type"/>.
        /// </exception>
        public static Func<T1, T2, T3, TResult> GetConstructor<T1, T2, T3, TResult>(this Type type)
        {
            return GetConstructorDelegate<Func<T1, T2, T3, TResult>>(type);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T1,T2,T3,T4,TResult}"/> that can be used to invoke
        /// the constructor of the given <paramref name="type"/>. The parameter
        /// list of the delegate must match the parameters of the constructor.
        /// </summary>
        /// <typeparam name="T1">Type of first parameter.</typeparam>
        /// <typeparam name="T2">Type of second parameter.</typeparam>
        /// <typeparam name="T3">Type of third parameter.</typeparam>
        /// <typeparam name="T4">Type of fourth parameter.</typeparam>
        /// <typeparam name="TResult">
        /// Type of the delegate's return value, which needs to be compatible
        /// with <paramref name="type"/>.
        /// </typeparam>
        /// <param name="type">The actual type to be constructed.</param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// Or null when there is no matching constructor on <paramref name="type"/>.
        /// </returns>
        public static Func<T1, T2, T3, T4, TResult> GetConstructorOrNull<T1, T2, T3, T4, TResult>(this Type type)
        {
            return GetConstructorDelegateOrNull<Func<T1, T2, T3, T4, TResult>>(type);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type
        /// <see cref="Func{T1,T2,T3,T4,TResult}"/> that can be used to invoke
        /// the constructor of the given <paramref name="type"/>. The parameter
        /// list of the delegate must match the parameters of the constructor.
        /// </summary>
        /// <typeparam name="T1">Type of first parameter.</typeparam>
        /// <typeparam name="T2">Type of second parameter.</typeparam>
        /// <typeparam name="T3">Type of third parameter.</typeparam>
        /// <typeparam name="T4">Type of fourth parameter.</typeparam>
        /// <typeparam name="TResult">
        /// Type of the delegate's return value, which needs to be compatible
        /// with <paramref name="type"/>.
        /// </typeparam>
        /// <param name="type">The actual type to be constructed.</param>
        /// <returns>
        /// A delegate that can be used to invoke the corresponding constructor.
        /// </returns>
        /// <exception cref="MissingMemberException">
        /// When there is no matching constructor on <paramref name="type"/>.
        /// </exception>
        public static Func<T1, T2, T3, T4, TResult> GetConstructor<T1, T2, T3, T4, TResult>(this Type type)
        {
            return GetConstructorDelegate<Func<T1, T2, T3, T4, TResult>>(type);
        }

        #endregion

        #region Method

        /// <summary>
        /// Extension method to obtain a delegate of type 
        /// <typeparamref name="TDelegate"/> that can be used to call the 
        /// static method with given method <paramref name="name"/> from given
        /// <paramref name="type"/>. The method signature must be compatible
        /// the parameter and return type of <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="type">
        /// The type to locate the compatible method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/> or null when
        /// no matching method if found.
        /// </returns>
        /// <seealso cref="GetStaticInvoker{TDelegate}"/>
        public static TDelegate GetStaticInvokerOrNull<TDelegate>(this Type type, string name)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(type, name, false, false).CreateInvoker();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type 
        /// <typeparamref name="TDelegate"/> that can be used to call the 
        /// static method with given method <paramref name="name"/> from given
        /// <paramref name="type"/>. The method signature must be compatible with 
        /// the parameter and return type of <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="type">
        /// The type to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/>.
        /// </returns>
        /// <exception name="MissingMethodException">
        /// When there is no matching method found.
        /// </exception>
        /// <seealso cref="GetStaticInvokerOrNull{TDelegate}"/>
        public static TDelegate GetStaticInvoker<TDelegate>(this Type type, string name)
            where TDelegate : class 
        {
            return new MethodDelegateBuilder<TDelegate>(type, name, true, false).CreateInvoker();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type 
        /// <typeparamref name="TDelegate"/> that can be used to call the 
        /// instance method with given method <paramref name="name"/> from given
        /// <paramref name="type"/>. The first parameter type of <c>TDelegate</c> 
        /// must be assignable to the given <paramref name="type"/>. The rest
        /// parameters and return type of <c>TDelegate</c> must be compatible with 
        /// the signature of the method.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="type">
        /// The type to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/> or null when
        /// no matching method if found.
        /// </returns>
        /// <seealso cref="GetInstanceInvoker{TDelegate}(Type,string)"/>
        /// <seealso cref="GetInstanceInvokerOrNull{TDelegate}(object,string)"/>
        public static TDelegate GetInstanceInvokerOrNull<TDelegate>(this Type type, string name)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(type, name, false, true).CreateInvoker();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type 
        /// <typeparamref name="TDelegate"/> that can be used to call the 
        /// instance method with given method <paramref name="name"/> from given
        /// <paramref name="type"/>. The first parameter type of <c>TDelegate</c> 
        /// must be assignable to the given <paramref name="type"/>. The rest
        /// parameters and return type of <c>TDelegate</c> must be compatible with 
        /// the signature of the method.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="type">
        /// The type to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/> or null when
        /// no matching method if found.
        /// </returns>
        /// <exception name="MissingMethodException">
        /// When there is no matching method found.
        /// </exception>
        /// <seealso cref="GetInstanceInvokerOrNull{TDelegate}(Type,string)"/>
        /// <seealso cref="GetInstanceInvoker{TDelegate}(object,string)"/>
        public static TDelegate GetInstanceInvoker<TDelegate>(this Type type, string name)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(type, name, true, true).CreateInvoker();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type 
        /// <typeparamref name="TDelegate"/> that can be used to call the 
        /// instance method with given method <paramref name="name"/> on specific
        /// <paramref name="obj">object</paramref>. The method signature must be 
        /// compatible with the signature of <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="obj">
        /// The object instance to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/> or null when
        /// no matching method if found.
        /// </returns>
        /// <seealso cref="GetInstanceInvoker{TDelegate}(object,string)"/>
        /// <seealso cref="GetInstanceInvokerOrNull{TDelegate}(Type,string)"/>
        public static TDelegate GetInstanceInvokerOrNull<TDelegate>(this object obj, string name)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(obj, obj.GetType(), name, false).CreateInvoker();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type 
        /// <typeparamref name="TDelegate"/> that can be used to call the 
        /// instance method with given method <paramref name="name"/> on specific
        /// <paramref name="obj">object</paramref>. The method signature must be 
        /// compatible with the signature of <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="obj">
        /// The object instance to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/> or null when
        /// no matching method if found.
        /// </returns>
        /// <exception name="MissingMethodException">
        /// When there is no matching method found.
        /// </exception>
        /// <seealso cref="GetInstanceInvoker{TDelegate}(Type,string)"/>
        /// <seealso cref="GetInstanceInvokerOrNull{TDelegate}(object,string)"/>
        public static TDelegate GetInstanceInvoker<TDelegate>(this object obj, string name)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(obj, obj.GetType(), name, true).CreateInvoker();
        }

        /// <summary>
        /// Extension method to obtain a delegate of type specified by parameter
        /// <typeparamref name="TDelegate"/> that can be used to make non virtual
        /// call to instance method with given method <paramref name="name"/> on 
        /// given <paramref name="type"/>. The first parameter type of <c>TDelegate</c> 
        /// must be assignable to the given <paramref name="type"/>. The rest
        /// parameters and return type of <c>TDelegate</c> must be compatible with 
        /// the signature of the method.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="type">
        /// The type to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/> or null when
        /// no matching method if found.
        /// </returns>
        /// <seealso cref="GetNonVirtualInvoker{TDelegate}(Type,string)"/>
        /// <seealso cref="GetNonVirtualInvokerOrNull{TDelegate}(object,Type,string)"/>
        public static TDelegate GetNonVirtualInvokerOrNull<TDelegate>(this Type type, string name)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(type, name, false, true).CreateInvoker(true);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type specified by parameter
        /// <typeparamref name="TDelegate"/> that can be used to make non virtual
        /// call to instance method with given method <paramref name="name"/> on 
        /// given <paramref name="type"/>. The first parameter type of <c>TDelegate</c> 
        /// must be assignable to the given <paramref name="type"/>. The rest
        /// parameters and return type of <c>TDelegate</c> must be compatible with 
        /// the signature of the method.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="type">
        /// The type to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/>.
        /// </returns>
        /// <exception name="MissingMethodException">
        /// When there is no matching method found.
        /// </exception>
        /// <seealso cref="GetNonVirtualInvokerOrNull{TDelegate}(Type,string)"/>
        /// <seealso cref="GetNonVirtualInvoker{TDelegate}(object,Type,string)"/>
        public static TDelegate GetNonVirtualInvoker<TDelegate>(this Type type, string name)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(type, name, true, true).CreateInvoker(true);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type specified by parameter
        /// <typeparamref name="TDelegate"/> that can be used to make non virtual
        /// call on the specific <paramref name="obj">object</paramref> to the
        /// instance method of given <paramref name="name"/> defined in the given 
        /// <paramref name="type"/> or its ancestor. The method signature must be 
        /// compatible with the signature of <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="obj">
        /// The object instance to invoke the method.
        /// </param>
        /// <param name="type">
        /// The type to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/> or null when
        /// no matching method if found.
        /// </returns>
        /// <seealso cref="GetNonVirtualInvoker{TDelegate}(object,Type,string)"/>
        /// <seealso cref="GetNonVirtualInvokerOrNull{TDelegate}(Type,string)"/>
        public static TDelegate GetNonVirtualInvokerOrNull<TDelegate>(this object obj, Type type, string name)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(obj, type, name, false).CreateInvoker(true);
        }

        /// <summary>
        /// Extension method to obtain a delegate of type specified by parameter
        /// <typeparamref name="TDelegate"/> that can be used to make non virtual
        /// call on the specific <paramref name="obj">object</paramref> to the
        /// instance method of given <paramref name="name"/> defined in the given 
        /// <paramref name="type"/> or its ancestor. The method signature must be 
        /// compatible with the signature of <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="obj">
        /// The object instance to invoke the method.
        /// </param>
        /// <param name="type">
        /// The type to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/> or null when
        /// no matching method if found.
        /// </returns>
        /// <exception name="MissingMethodException">
        /// When there is no matching method found.
        /// </exception>
        /// <seealso cref="GetNonVirtualInvoker{TDelegate}(Type,string)"/>
        /// <seealso cref="GetNonVirtualInvokerOrNull{TDelegate}(object,Type,string)"/>
        public static TDelegate GetNonVirtualInvoker<TDelegate>(this object obj, Type type, string name)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(obj, type, name, true).CreateInvoker(true);
        }

        /// <summary>
        /// This is a more general purpose method to obtain a delegate of type
        /// specified by parameter <typeparamref name="TDelegate"/> that can 
        /// be used to call on the specific <paramref name="obj">object</paramref> 
        /// to the method of given <paramref name="name"/> defined in the given 
        /// <paramref name="type"/> or its ancestor. The method signature must be 
        /// compatible with the signature of <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="obj">
        /// The object instance to invoke the method or null for static methods
        /// and open instance methods.
        /// </param>
        /// <param name="type">
        /// The type to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <param name="bindingAttr">
        /// A bitmask comprised of one or more <see cref="BindingFlags"/> that 
        /// specify how the search is conducted.  -or- Zero, to return null.
        /// </param>
        /// <param name="filter">
        /// The additional filter to include/exclude methods.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/> or null when
        /// no matching method if found.
        /// </returns>
        public static TDelegate GetInvokerOrNull<TDelegate>(object obj, Type type, string name, BindingFlags bindingAttr, Predicate<MethodInfo> filter)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(obj, type, name, false, bindingAttr)
                       {
                           MethodFilter = filter, 
                       }.CreateInvoker();
        }

        /// <summary>
        /// This is a more general purpose method to obtain a delegate of type
        /// specified by parameter <typeparamref name="TDelegate"/> that can 
        /// be used to call on the specific <paramref name="obj">object</paramref> 
        /// to the method of given <paramref name="name"/> defined in the given 
        /// <paramref name="type"/> or its ancestor. The method signature must be 
        /// compatible with the signature of <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <typeparam name="TDelegate">
        /// Type of a .Net delegate.
        /// </typeparam>
        /// <param name="obj">
        /// The object instance to invoke the method or null for static methods
        /// and open instance methods.
        /// </param>
        /// <param name="type">
        /// The type to find the method.
        /// </param>
        /// <param name="name">
        /// The name of the method.
        /// </param>
        /// <param name="bindingAttr">
        /// A bitmask comprised of one or more <see cref="BindingFlags"/> that 
        /// specify how the search is conducted.  -or- Zero, to return null.
        /// </param>
        /// <param name="filter">
        /// The additional filter to include/exclude methods.
        /// </param>
        /// <param name="filterMessage">
        /// The description of the additional filter criteria that will be
        /// included in the exception message when no matching method found.
        /// </param>
        /// <returns>
        /// A delegate of type <typeparamref name="TDelegate"/>.
        /// </returns>
        /// <exception name="MissingMethodException">
        /// When there is no matching method found.
        /// </exception>
        public static TDelegate GetInvoker<TDelegate>(object obj, Type type, string name, BindingFlags bindingAttr, Predicate<MethodInfo> filter, string filterMessage)
            where TDelegate : class
        {
            return new MethodDelegateBuilder<TDelegate>(obj, type, name, true, bindingAttr)
            {
                MethodFilter = filter,
                MethodFilterMessage = filterMessage
            }.CreateInvoker();
        }

        #endregion
    }
}
