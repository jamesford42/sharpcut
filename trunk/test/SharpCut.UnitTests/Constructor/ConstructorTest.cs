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

namespace SharpCut.UnitTests.Constructor
{
    /// <summary>
    /// Test cases for constructor related extension methods in <see cref="Reflections"/>.
    /// </summary>
    /// <author>Kenneth Xu</author>
    [TestFixture(true)]
    [TestFixture(false)]
    public class ConstructorTest
    {
        private readonly Type _type;

        const string _sampleString = "sample string";
        private const int _sampleInt = 998;
        private readonly object _sampleObject = "sample object";
        private const long _sampleLong = 3948493838383L;

        public ConstructorTest(bool isStruct)
        {
            _type = isStruct ? typeof(StructSample) : typeof(ClassSample);
        }

        [Test] public void GetConstructorDelegateOrNull_WhenDelegateReturnVoid_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                () => _type.GetConstructorDelegateOrNull<Action<string>>());
        }

        [Test] public void GetConstructorDelegate_WhenDelegateReturnVoid_ThrowsException()
        {
            Assert.Throws<InvalidOperationException>(
                () => _type.GetConstructorDelegate<Action<string>>());
        }

        [Test] public void GetConstructorDelegateOrNull_WhenDelegateReturnUnmatch_ReturnsNull()
        {
            var constructor = _type.GetConstructorDelegateOrNull<Func<string>>();
            Assert.That(constructor, Is.Null);
        }

        [Test] public void GetConstructorDelegate_WhenDelegateReturnUnmatch_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _type.GetConstructorDelegate<Func<string>>());
        }

        [Test] public void GetConstructorOrNull_WhenDelegateParameterUnmatch_ReturnsNull()
        {
            var constructor = _type.GetConstructorDelegateOrNull<Func<object, ISample>>();
            Assert.That(constructor, Is.Null);
        }

        [Test] public void GetConstructor_WhenDelegateParameterUnmatch_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _type.GetConstructorDelegate<Func<object, ISample>>());
        }

        // this test should ideally return working delegate
        [Test] public void GetConstructorOrNull_WhenDelegateNeedsBoxing_ReturnsNull()
        {
            var constructor = _type.GetConstructorDelegateOrNull<Func<string, int, int, ISample>>();
            Assert.That(constructor, Is.Null);
        }

        // this test should ideally return working delegate
        [Test]
        public void GetConstructor_WhenDelegateNeedsBoxing_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _type.GetConstructorDelegate<Func<string, int, int, ISample>>());
        }

        // this test should ideally return working delegate
        [Test]
        public void GetConstructorOrNull_WhenDelegateNeedsImplicitTypeConvertion_ReturnsNull()
        {
            var constructor = _type.GetConstructorDelegateOrNull<Func<string, int, object, int, ISample>>();
            Assert.That(constructor, Is.Null);
        }

        // this test should ideally return working delegate
        [Test] public void GetConstructor_WhenDelegateNeedsImplicitTypeConvertion_ThrowsException()
        {
            Assert.Throws<MissingMemberException>(
                () => _type.GetConstructorDelegate<Func<string, int, object, int, ISample>>());
        }

        [Test] public void GetConstructorOrNull_WhenDelegateHasNoParameter_ReturnsWorkingDelegate()
        {
            var constructor = _type.GetConstructorOrNull<ISample>();
            var item = constructor();
            Assert.That(item.GetType(), Is.EqualTo(_type));
        }
        
        [Test] public void GetConstructor_WhenDelegateHasNoParameter_ReturnsWorkingDelegate()
        {
            var constructor = _type.GetConstructor<ISample>();
            var item = constructor();
            Assert.That(item.GetType(), Is.EqualTo(_type));
        }

        [Test] public void GetConstructorOrNull_WhenDelegateHasOneParameter_ReturnsWorkingDelegate()
        {
            var constructor = _type.GetConstructorOrNull<string, ISample>();
            var item = constructor(_sampleString);
            Assert.That(item.GetType(), Is.EqualTo(_type));
            Assert.That(item.Param1, Is.EqualTo(_sampleString));
        }
        
        [Test] public void GetConstructor_WhenDelegateHasOneParameter_ReturnsWorkingDelegate()
        {
            var constructor = _type.GetConstructor<string, ISample>();
            var item = constructor(_sampleString);
            Assert.That(item.GetType(), Is.EqualTo(_type));
            Assert.That(item.Param1, Is.EqualTo(_sampleString));
        }

        [Test] public void GetConstructorOrNull_WhenDelegateHasTwoParameters_ReturnsWorkingDelegate()
        {
            var constructor = _type.GetConstructorOrNull<string, int, ISample>();
            var item = constructor(_sampleString, _sampleInt);
            Assert.That(item.GetType(), Is.EqualTo(_type));
            Assert.That(item.Param1, Is.EqualTo(_sampleString));
            Assert.That(item.Param2, Is.EqualTo(_sampleInt));
        }
        
        [Test] public void GetConstructor_WhenDelegateHasTwoParameters_ReturnsWorkingDelegate()
        {
            var constructor = _type.GetConstructor<string, int, ISample>();
            var item = constructor(_sampleString, _sampleInt);
            Assert.That(item.GetType(), Is.EqualTo(_type));
            Assert.That(item.Param1, Is.EqualTo(_sampleString));
            Assert.That(item.Param2, Is.EqualTo(_sampleInt));
        }

        [Test] public void GetConstructorOrNull_WhenDelegateHasThreeParameters_ReturnsWorkingDelegate()
        {
            var constructor = _type.GetConstructorOrNull<string, int, object, ISample>();
            var item = constructor(_sampleString, _sampleInt, _sampleObject);
            Assert.That(item.GetType(), Is.EqualTo(_type));
            Assert.That(item.Param1, Is.EqualTo(_sampleString));
            Assert.That(item.Param2, Is.EqualTo(_sampleInt));
            Assert.That(item.Param3, Is.EqualTo(_sampleObject));
        }
        
        [Test] public void GetConstructor_WhenDelegateHasThreeParameters_ReturnsWorkingDelegate()
        {
            var constructor = _type.GetConstructor<string, int, object, ISample>();
            var item = constructor(_sampleString, _sampleInt, _sampleObject);
            Assert.That(item.GetType(), Is.EqualTo(_type));
            Assert.That(item.Param1, Is.EqualTo(_sampleString));
            Assert.That(item.Param2, Is.EqualTo(_sampleInt));
            Assert.That(item.Param3, Is.EqualTo(_sampleObject));
        }

        [Test] public void GetConstructorOrNull_WhenDelegateHasFourParameters_ReturnsWorkingDelegate()
        {
            var constructor = _type.GetConstructorOrNull<string, int, object, long, ISample>();
            var item = constructor(_sampleString, _sampleInt, _sampleObject, _sampleLong);
            Assert.That(item.GetType(), Is.EqualTo(_type));
            Assert.That(item.Param1, Is.EqualTo(_sampleString));
            Assert.That(item.Param2, Is.EqualTo(_sampleInt));
            Assert.That(item.Param3, Is.EqualTo(_sampleObject));
            Assert.That(item.Param4, Is.EqualTo(_sampleLong));
        }
        
        [Test] public void GetConstructor_WhenDelegateHasFourParameters_ReturnsWorkingDelegate()
        {
            var constructor = _type.GetConstructor<string, int, object, long, ISample>();
            var item = constructor(_sampleString, _sampleInt, _sampleObject, _sampleLong);
            Assert.That(item.GetType(), Is.EqualTo(_type));
            Assert.That(item.Param1, Is.EqualTo(_sampleString));
            Assert.That(item.Param2, Is.EqualTo(_sampleInt));
            Assert.That(item.Param3, Is.EqualTo(_sampleObject));
            Assert.That(item.Param4, Is.EqualTo(_sampleLong));
        }
    }
}