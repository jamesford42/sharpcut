using System;
using NUnit.Framework;

namespace SharpCut.UnitTests
{
    [TestFixture(typeof(int))]
    [TestFixture(typeof(string))]
    public class AccessorTest<T>
    {
        static readonly T _one = (T)Convert.ChangeType(1, typeof(T));
        static readonly T _two = (T)Convert.ChangeType(2, typeof(T));

        private class Sample { public T Value; }

        [Test] public void ValueGetter_CallsGetDelegate()
        {
            var sample = new Sample();
            var accessor = new Accessor<T> {Get = (() => sample.Value)};
            sample.Value = _one;
            Assert.That(accessor.Value, Is.EqualTo(_one));
            sample.Value = _two;
            Assert.That(accessor.Value, Is.EqualTo(_two));
        }

        [Test] public void ValueSetter_CallsSetDelegate()
        {
            var sample = new Sample();
            var accessor = new Accessor<T> { Set = (x => sample.Value = x) };
            accessor.Value = _one;
            Assert.That(sample.Value, Is.EqualTo(_one));
            accessor.Value = _two;
            Assert.That(sample.Value, Is.EqualTo(_two));
        }

        [Test] public void IndexerGetter_CallsGetDelegate()
        {
            var accessor = new Accessor<Sample, T> {Get = x => x.Value};
            var s1 = new Sample { Value = _one };
            var s2 = new Sample { Value = _two };
            Assert.That(accessor[s1], Is.EqualTo(_one));
            Assert.That(accessor[s2], Is.EqualTo(_two));
        }

        [Test] public void IndexerSetter_CallsSetDelegate()
        {
            var accessor = new Accessor<Sample, T> {Set = (s,v) => s.Value = v};
            var s1 = new Sample { Value = _one };
            var s2 = new Sample { Value = _two };
            accessor[s1] = _two;
            accessor[s2] = _one;
            Assert.That(s1.Value, Is.EqualTo(_two));
            Assert.That(s2.Value, Is.EqualTo(_one));
        }
    }
}