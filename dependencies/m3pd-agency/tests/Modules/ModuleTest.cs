using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.State;
using System;

namespace rharel.M3PD.Agency.Modules.Tests
{
    [TestFixture]
    public sealed class ModuleTest
    {
        private Mock<StateAccessor> _state = new Mock<StateAccessor>();
        private Mock<Module> _module;

        [SetUp]
        public void Setup()
        {
            _module = new Mock<Module>();
        }

        [Test]
        public void Test_Initialize_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => _module.Object.Initialize(null)
            );
        }
        [Test]
        public void Test_Initialize_WhenAlreadyInitialized()
        {
            _module.Object.Initialize(_state.Object);

            Assert.Throws<InvalidOperationException>(
                () => _module.Object.Initialize(_state.Object)
            );
        }
        [Test]
        public void Test_Initialize()
        {
            _module.Object.Initialize(_state.Object);

            Assert.IsTrue(_module.Object.IsInitialized);
            Assert.AreSame(_state.Object, _module.Object.State);

            _module.Verify(x => x.Setup(), Times.Once);
        }
    }
}
