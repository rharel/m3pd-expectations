using Moq;
using NUnit.Framework;
using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Agency.State;
using System;

namespace rharel.M3PD.Agency.System.Tests
{
    [TestFixture]
    public sealed class AgencySystemTest
    {
        private static readonly DialogueMove TARGET_MOVE = (
            new Mock<DialogueMove>().Object
        ); 

        private sealed class MockRAPModule: RAPModule
        {
            public Mock<RAPModule> Mock { get; private set; } = (
                new Mock<RAPModule>()
            );

            public int InvocationCount { get; private set; } = 0;
            public RecentActivity SubmittedReport { get; private set; }

            protected override void ComposeActivityReport(
                RecentActivity report)
            {
                var mock_report = Mock.Object.PerceiveActivity();
                
                foreach (var @event in mock_report.Events)
                {
                    report.Add(@event);
                }

                ++ InvocationCount;
                SubmittedReport = report;

            }
        }
        private sealed class MockCAPModule: CAPModule
        {
            public Mock<CAPModule> Mock { get; private set; } = (
                new Mock<CAPModule>()
            );

            public int InvocationCount { get; private set; } = 0;
            public CurrentActivity SubmittedReport { get; private set; }

            protected override void ComposeActivityReport(
                CurrentActivity report)
            {
                var mock_report = Mock.Object.PerceiveActivity();

                foreach (var agent in mock_report.ActiveIDs)
                {
                    report.MarkActive(agent);
                }
                foreach (var agent in mock_report.PassiveIDs)
                {
                    report.MarkPassive(agent);
                }

                ++ InvocationCount;
                SubmittedReport = report;
            }
        }
        private sealed class MockSUModule: SUModule
        {
            public Mock<SUModule> Mock { get; private set; } = (
                new Mock<SUModule>()
            );

            public override void PerformUpdate(
                RecentActivity recent_activity, 
                CurrentActivity current_activity, 
                SystemActivity system_activity,
                StateMutator state_mutator)
            {
                Mock.Object.PerformUpdate(
                    recent_activity, current_activity, system_activity,
                    state_mutator
                );
            }
        }
        private sealed class MockASModule: ASModule
        {
            public Mock<ASModule> Mock { get; private set; } = (
                new Mock<ASModule>()
            );

            public override DialogueMove SelectMove()
            {
                return Mock.Object.SelectMove();
            }
        }
        private sealed class MockATModule: ATModule
        {
            public Mock<ATModule> Mock { get; private set; } = (
                new Mock<ATModule>()
            );

            public override bool IsValidMoveNow(DialogueMove move)
            {
                return Mock.Object.IsValidMoveNow(move);
            }
        }
        private sealed class MockARModule: ARModule
        {
            public Mock<ARModule> Mock { get; private set; } = (
                new Mock<ARModule>()
            );

            public override RealizationStatus RealizeMove(DialogueMove move)
            {
                return Mock.Object.RealizeMove(move);
            }
        }

        private InformationState _state;

        private MockRAPModule _RAP;
        private MockCAPModule _CAP;
        private MockSUModule _SU;
        private MockASModule _AS;
        private MockATModule _AT;
        private MockARModule _AR;
        private ModuleBundle _modules;

        private AgencySystem _system;

        [SetUp]
        public void Setup()
        {
            _state = new InformationState.Builder().Build();

            _RAP = new MockRAPModule();
            _CAP = new MockCAPModule();
            _SU = new MockSUModule();
            _AS = new MockASModule();
            _AT = new MockATModule();
            _AR = new MockARModule();

            _modules = new ModuleBundle.Builder()
                .WithRecentActivityPerceptionBy(_RAP)
                .WithCurrentActivityPerceptionBy(_CAP)
                .WithStateUpdateBy(_SU)
                .WithActionSelectionBy(_AS)
                .WithActionTimingBy(_AT)
                .WithActionRealizationBy(_AR)
                .Build();

            _system = new AgencySystem(_state, _modules);

            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns(Idle.Instance);
            _AT.Mock.Setup(x => x.IsValidMoveNow(It.IsAny<DialogueMove>()))
                    .Returns(true);
            _AR.Mock.Setup(x => x.RealizeMove(It.IsAny<DialogueMove>()))
                    .Returns(RealizationStatus.Complete);
        }
        
        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new AgencySystem(null, _modules)
            );
        }

        [Test]
        public void Test_InitialState()
        {
            Assert.IsTrue(_system.RecentMove.Contains(Idle.Instance));
            Assert.AreEqual(Idle.Instance, _system.TargetMove);
            Assert.IsFalse(_system.IsActive);
        }

        [Test]
        public void Test_ModuleInvocation_RecentActivityPerception()
        {
            _system.Step();

            Assert.AreEqual(1, _RAP.InvocationCount);
        }
        [Test]
        public void Test_ModuleInvocation_CurrentActivityPerception()
        {
            _system.Step();

            Assert.AreEqual(1, _CAP.InvocationCount);
        }
        [Test]
        public void Test_ModuleInvocation_StateUpdate()
        {
            var system_activity = new SystemActivity(
                _system.RecentMove,
                _system.TargetMove,
                _system.IsActive
            );

            _system.Step();

            _SU.Mock.Verify(
                x => x.PerformUpdate(
                    _RAP.SubmittedReport, 
                    _CAP.SubmittedReport, 
                    system_activity,
                    _state
                ), 
                Times.Once
            );
        }
        [Test]
        public void Test_ModuleInvocation_ActionSelection()
        {
            _system.Step();

            _AS.Mock.Verify(x => x.SelectMove(), Times.Once);
        }
        [Test]
        public void Test_ModuleInvocation_ActionTiming()
        {
            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns(TARGET_MOVE);

            _system.Step();

            _AT.Mock.Verify(
                x => x.IsValidMoveNow(TARGET_MOVE), 
                Times.Once
            );
        }
        [Test]
        public void Test_ModuleInvocation_ActionRealization()
        {
            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns(TARGET_MOVE);

            _system.Step();

            _AR.Mock.Verify(
                x => x.RealizeMove(TARGET_MOVE), 
                Times.Once
            );
        }

        [Test]
        public void Test_Step_WithNullTargetMove()
        {
            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns((DialogueMove)null);

            Assert.Throws<InvalidOperationException>(() => _system.Step());
        }
        [Test]
        public void Test_Step_WithIdleTargetMove()
        {
            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns(Idle.Instance);

            _system.Step();

            Assert.IsTrue(_system.RecentMove.Contains(Idle.Instance));
            Assert.AreEqual(Idle.Instance, _system.TargetMove);
            Assert.IsFalse(_system.IsActive);
        }
        [Test]
        public void Test_Step_WithInvalidTimingOfTargetMove()
        {
            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns(TARGET_MOVE);
            _AT.Mock.Setup(x => x.IsValidMoveNow(TARGET_MOVE))
                    .Returns(false);

            _system.Step();

            Assert.IsTrue(_system.RecentMove.Contains(Idle.Instance));
            Assert.AreEqual(TARGET_MOVE, _system.TargetMove);
            Assert.IsFalse(_system.IsActive);
        }
        [Test]
        public void Test_Step_WithTargetMoveInProgress()
        {
            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns(TARGET_MOVE);
            _AT.Mock.Setup(x => x.IsValidMoveNow(TARGET_MOVE))
                    .Returns(true);
            _AR.Mock.Setup(x => x.RealizeMove(TARGET_MOVE))
                    .Returns(RealizationStatus.InProgress);

            _system.Step();

            Assert.IsFalse(_system.RecentMove.IsSome());
            Assert.AreEqual(TARGET_MOVE, _system.TargetMove);
            Assert.IsTrue(_system.IsActive);
        }
        [Test]
        public void Test_Step_WithTargetMoveComplete()
        {
            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns(TARGET_MOVE);
            _AT.Mock.Setup(x => x.IsValidMoveNow(TARGET_MOVE))
                    .Returns(true);
            _AR.Mock.Setup(x => x.RealizeMove(TARGET_MOVE))
                    .Returns(RealizationStatus.Complete);

            _system.Step();

            Assert.IsTrue(_system.RecentMove.Contains(TARGET_MOVE));
            Assert.AreEqual(TARGET_MOVE, _system.TargetMove);
            Assert.IsFalse(_system.IsActive);
        }
        [Test]
        public void Test_Step_WithMoveCancellationByInvalidTiming()
        {
            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns(TARGET_MOVE);
            _AT.Mock.Setup(x => x.IsValidMoveNow(TARGET_MOVE))
                    .Returns(true);
            _AR.Mock.Setup(x => x.RealizeMove(TARGET_MOVE))
                    .Returns(RealizationStatus.InProgress);

            _system.Step();

            _AT.Mock.Setup(x => x.IsValidMoveNow(TARGET_MOVE))
                    .Returns(false);

            _system.Step();

            Assert.IsTrue(_system.RecentMove.Contains(Idle.Instance));
            Assert.AreEqual(TARGET_MOVE, _system.TargetMove);
            Assert.IsFalse(_system.IsActive);
        }
        [Test]
        public void Test_Step_WithMoveCancellationByChangeOfTarget()
        {
            var target_move_b = new Mock<DialogueMove>().Object;
            var non_instant_moves = new DialogueMove[]
            {
                TARGET_MOVE,
                target_move_b
            };

            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns(TARGET_MOVE);
            _AR.Mock.Setup(x => x.RealizeMove(It.IsIn(non_instant_moves)))
                    .Returns(RealizationStatus.InProgress);

            _system.Step();

            _AS.Mock.Setup(x => x.SelectMove())
                    .Returns(target_move_b);

            _system.Step();

            Assert.IsFalse(_system.RecentMove.IsSome());
            Assert.AreEqual(target_move_b, _system.TargetMove);
            Assert.IsTrue(_system.IsActive);
        }
    }
}
