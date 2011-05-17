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
namespace SharpCut.UnitTests.Constructor
{
    public class ClassSample : ISample
    {
        private readonly long _param4;
        private readonly object _param3;
        private readonly int _param2;
        private readonly string _param1;

        public ClassSample()
        {
        }

        public ClassSample(string param1)
        {
            _param1 = param1;
        }

        public ClassSample(string param1, int param2) : this(param1)
        {
            _param2 = param2;
        }

        public ClassSample(string param1, int param2, object param3)
            : this(param1, param2)
        {
            _param3 = param3;
        }

        public ClassSample(string param1, int param2, object param3, long param4)
            : this(param1, param2, param3)
        {
            _param4 = param4;
        }

        public string Param1
        {
            get { return _param1; }
        }

        public int Param2
        {
            get { return _param2; }
        }

        public object Param3
        {
            get { return _param3; }
        }

        public long Param4
        {
            get { return _param4; }
        }
    }
}