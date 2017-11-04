using NUnit.Framework;
using System;

namespace rharel.M3PD.Agency.State.Tests
{
    [TestFixture]
    public sealed class StateComponentTest
    {
        private static readonly string VALUE = "mock value";

        private StateComponent<string> _component;

        [SetUp]
        public void Setup()
        {
            _component = new StateComponent<string>(VALUE);
        }

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new StateComponent<object>(null)
            );
        }
        [Test]
        public void Test_Consructor()
        {
            Assert.AreEqual(typeof(string), _component.Type);
            Assert.AreSame(VALUE, _component.Value);
        }

        [Test]
        public void Test_Equality()
        {
            var original = _component;
            var good_copy = new StateComponent<object>(VALUE);
            var flawed_value_copy = new StateComponent<object>(new object());

            Assert.AreNotEqual(original, null);
            Assert.AreNotEqual(original, "incompatible type");
            Assert.AreNotEqual(original, flawed_value_copy);

            Assert.AreEqual(original, original);
            Assert.AreEqual(original, good_copy);
        }
        [Test]
        public void Test_Equality_WhenCast()
        {
            Assert.AreEqual(
                new StateComponent<object>(VALUE),
                new StateComponent<string>(VALUE)
            );
        }
    }
}
