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
namespace SharpCut.UnitTests.Property
{
    /// <summary>
    /// One of sample classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <author>Kenneth Xu</author>
    internal class PropertyBase<T>
    {
        #region Static Properties
        public static T StaticPropertyT { get; set; }

        public static object StaticPropertyObject { get; set; }

        public static T StaticReadOnlyT
        {
            get { return StaticPropertyT; }
        }

        private static T StaticWriteOnlyT
        {
            set { StaticPropertyT = value; }
        }

        private static object StaticReadOnlyObject
        {
            get { return StaticPropertyObject; }
        }

        public static object StaticWriteOnlyObject
        {
            set { StaticPropertyObject = value; }
        }

        #endregion Static Properties

        #region  Instance Properties

        public T InstancePropertyT { get; set; }

        public virtual T VirtualPropertyT
        {
            get { return InstancePropertyT; }
            set { InstancePropertyT = value; }
        }

        public object InstancePropertyObject { get; set; }

        public T InstanceReadOnlyT
        {
            get { return InstancePropertyT; }
        }

        public T InstanceWriteOnlyT
        {
            set { InstancePropertyT = value; }
        }

        public object InstanceReadOnlyObject
        {
            get { return InstancePropertyObject; }
        }

        public object InstanceWriteOnlyObject
        {
            set { InstancePropertyObject = value; }
        }

        #endregion Instance Properties
    }
}