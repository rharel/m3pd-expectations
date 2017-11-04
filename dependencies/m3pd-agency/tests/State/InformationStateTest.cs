using NUnit.Framework;
using System;

namespace rharel.M3PD.Agency.State.Tests
{
    [TestFixture]
    public sealed class InformationStateTest
    {
        private static readonly string ID = "mock id";
        private static readonly string VALUE = "mock value";

        private InformationState _state;

        [SetUp]
        public void Setup()
        {
            _state = new InformationState.Builder()
                .WithComponent(ID, VALUE)
                .Build();
        }

        [Test]
        public void Test_ComponentIDs()
        {
            Assert.AreEqual(1, _state.ComponentIDs.Count);
            Assert.IsTrue(_state.ComponentIDs.Contains(ID));
        }

        [Test]
        public void Test_Get_WithInvalidArgs()
        {
            Assert.Throws<ArgumentException>(
                () => _state.Get<string>($"wrong {ID}")
            );
            Assert.Throws<ArgumentException>(
                () => _state.Get<int>(ID)
            );
        }
        [Test]
        public void Test_Get()
        {
            Assert.AreEqual(VALUE, _state.Get<string>(ID));
        }

        [Test]
        public void Test_Set_WithInvalidArgs()
        {
            Assert.Throws<ArgumentException>(
                () => _state.Set($"wrong {ID}", $"new {VALUE}")
            );
            Assert.Throws<ArgumentException>(
                () => _state.Set<int>(ID, 0)
            );
        }
        [Test]
        public void Test_Set()
        {
            var new_value = $"new {VALUE}";
            _state.Set(ID, new_value);

            Assert.AreEqual(new_value, _state.Get<string>(ID));
        }
    }
}
