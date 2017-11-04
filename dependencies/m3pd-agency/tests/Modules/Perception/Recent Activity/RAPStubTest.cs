using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.State;
using rharel.M3PD.Common.Collections;

namespace rharel.M3PD.Agency.Modules.Tests
{
    [TestFixture]
    public sealed class RAPStubTest
    {
        private static readonly StateAccessor STATE = (
            new Mock<StateAccessor>().Object
        );

        private RAPStub _stub;

        [SetUp]
        public void Setup()
        {
            _stub = new RAPStub();
            _stub.Initialize(STATE);
        }

        [Test]
        public void Test_PerceiveActivity()
        {
            RecentActivity report = _stub.PerceiveActivity();

            Assert.IsTrue(report.Events.IsEmpty());
        }
    }
}
