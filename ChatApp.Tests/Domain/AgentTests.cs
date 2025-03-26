using NUnit.Framework;
using ChatApp.Domain.Entities;
using ChatApp.Domain.Enum;
using static ChatApp.Domain.Enum.Enum;

namespace ChatApp.Tests.Domain.Entities
{
    [TestFixture]
    public class AgentTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void MaxConcurrency_ShouldBeCalculatedCorrectly()
        {
            var juniorAgent = new Agent { AgentLevel = AgentLevel.Junior };
            Assert.That(juniorAgent.MaxConcurrency, Is.EqualTo(4));

            var midLevelAgent = new Agent { AgentLevel = AgentLevel.MidLevel };
            Assert.That(midLevelAgent.MaxConcurrency, Is.EqualTo(6));

            var seniorAgent = new Agent { AgentLevel = AgentLevel.Senior };
            Assert.That(seniorAgent.MaxConcurrency, Is.EqualTo(8));

            var teamLeadAgent = new Agent { AgentLevel = AgentLevel.TeamLead };
            Assert.That(teamLeadAgent.MaxConcurrency, Is.EqualTo(5));
        }

        [Test]
        public void IsCurrentlyOnShift_ShouldReturnFalse_WhenShiftIsNull()
        {
            var agent = new Agent { CurrentShift = null };
            Assert.That(agent.IsCurrentlyOnShift, Is.False);
        }

        [Test]
        public void IsActive_ShouldReturnTrue_WhenOverflowAgent()
        {
            var agent = new Agent { CurrentShift = null };
            Assert.That(agent.IsActive, Is.True);
        }
    }

}