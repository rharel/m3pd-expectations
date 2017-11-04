using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Agency.State;
using System;
using System.Linq;

namespace rharel.M3PD.Agency.System.Tests
{
    [TestFixture]
    public sealed class ModuleBundleTest
    {
        private static readonly RAPModule RAP = new RAPStub();
        private static readonly CAPModule CAP = new CAPStub();
        private static readonly SUModule SU = new SUStub();
        private static readonly ASModule AS = new ASStub();
        private static readonly ATModule AT = new ATStub();
        private static readonly ARModule AR = new ARStub();

        private ModuleBundle.Builder _builder;
        private ModuleBundle _bundle;

        [SetUp]
        public void Setup()
        {
            _builder = new ModuleBundle.Builder();
            _bundle = new ModuleBundle.Builder()
                .WithRecentActivityPerceptionBy(RAP)
                .WithCurrentActivityPerceptionBy(CAP)
                .WithStateUpdateBy(SU)
                .WithActionSelectionBy(AS)
                .WithActionTimingBy(AT)
                .WithActionRealizationBy(AR)
                .Build();
        }

        [Test]
        public void Test_Builder_WithNullModule()
        {
            Assert.Throws<ArgumentNullException>(
                () => _builder.WithRecentActivityPerceptionBy(null)
            );
            Assert.Throws<ArgumentNullException>(
                () => _builder.WithCurrentActivityPerceptionBy(null)
            );
            Assert.Throws<ArgumentNullException>(
                () => _builder.WithStateUpdateBy(null)
            );
            Assert.Throws<ArgumentNullException>(
                () => _builder.WithActionSelectionBy(null)
            );
            Assert.Throws<ArgumentNullException>(
                () => _builder.WithActionTimingBy(null)
            );
            Assert.Throws<ArgumentNullException>(
                () => _builder.WithActionRealizationBy(null)
            );
        }
        [Test]
        public void Test_Builder_WithInitializedModule()
        {
            var state = new Mock<StateAccessor>().Object;

            var RAP_stub = new RAPStub();
            var CAP_stub = new CAPStub();
            var SU_stub = new SUStub();
            var AS_stub = new ASStub();
            var AT_stub = new ATStub();
            var AR_stub = new ARStub();

            RAP_stub.Initialize(state);
            CAP_stub.Initialize(state);
            SU_stub.Initialize(state);
            AS_stub.Initialize(state);
            AT_stub.Initialize(state);
            AR_stub.Initialize(state);

            Assert.Throws<ArgumentException>(
                () => _builder.WithRecentActivityPerceptionBy(RAP_stub)
            );
            Assert.Throws<ArgumentException>(
                () => _builder.WithCurrentActivityPerceptionBy(CAP_stub)
            );
            Assert.Throws<ArgumentException>(
                () => _builder.WithStateUpdateBy(SU_stub)
            );
            Assert.Throws<ArgumentException>(
                () => _builder.WithActionSelectionBy(AS_stub)
            );
            Assert.Throws<ArgumentException>(
                () => _builder.WithActionTimingBy(AT_stub)
            );
            Assert.Throws<ArgumentException>(
                () => _builder.WithActionRealizationBy(AR_stub)
            );
        }

        [Test]
        public void Test_Constructor()
        {
            Assert.AreSame(RAP, _bundle.RecentActivityPerception);
            Assert.AreSame(CAP, _bundle.CurrentActivityPerception);
            Assert.AreSame(SU, _bundle.StateUpdate);
            Assert.AreSame(AS, _bundle.ActionSelection);
            Assert.AreSame(AT, _bundle.ActionTiming);
            Assert.AreSame(AR, _bundle.ActionRealization);
        }

        [Test]
        public void Test_Enumeration()
        {
            var list = _bundle.ToList();

            Assert.AreEqual(6, list.Count);

            Assert.IsTrue(list.Contains(_bundle.RecentActivityPerception));
            Assert.IsTrue(list.Contains(_bundle.CurrentActivityPerception));
            Assert.IsTrue(list.Contains(_bundle.StateUpdate));
            Assert.IsTrue(list.Contains(_bundle.ActionSelection));
            Assert.IsTrue(list.Contains(_bundle.ActionTiming));
            Assert.IsTrue(list.Contains(_bundle.ActionRealization));
        }
    }
}
