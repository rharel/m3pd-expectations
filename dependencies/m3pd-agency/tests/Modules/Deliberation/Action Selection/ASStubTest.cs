using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.State;

namespace rharel.M3PD.Agency.Modules.Tests
{
    [TestFixture]
    public sealed class ASStubTest
    {
        private static readonly StateAccessor STATE = (
            new Mock<StateAccessor>().Object
        );

        private ASStub _stub;    

        [SetUp]
        public void Setup()
        {
            _stub = new ASStub();
            _stub.Initialize(STATE);
        }

        [Test]
        public void Test_SelectMove()
        {
            Assert.IsTrue(_stub.SelectMove().IsIdle());
        }
    }
}
