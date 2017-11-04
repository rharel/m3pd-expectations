using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.State;
using rharel.M3PD.Common.Collections;

namespace rharel.M3PD.Agency.Modules.Tests
{
    [TestFixture]
    public sealed class CAPStubTest
    {
        private static readonly StateAccessor STATE = (
            new Mock<StateAccessor>().Object
        );

        private CAPStub _stub;

        [SetUp]
        public void Setup()
        {
            _stub = new CAPStub();
            _stub.Initialize(STATE);
        }

        [Test]
        public void Test_PerceiveActivity()
        {
            CurrentActivity report = _stub.PerceiveActivity();

            Assert.IsTrue(report.PassiveIDs.IsEmpty());
            Assert.IsTrue(report.ActiveIDs.IsEmpty());
        }
    }
}
