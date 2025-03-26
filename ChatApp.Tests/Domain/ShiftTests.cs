using NUnit.Framework;
using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;

namespace ChatApp.Tests.Domain.Entities
{
    [TestFixture]
    public class ShiftTests
    {
        private Shift _shift;

        [SetUp]
        public void Setup()
        {
            _shift = new Shift
            {
                Name = ChatApp.Domain.Enum.Enum.ShiftSchedule.DayShift,
                StartTime = new TimeSpan(9, 0, 0), // 9:00 AM
                EndTime = new TimeSpan(17, 0, 0), // 5:00 PM
                Agents = new List<Agent>()
            };
        }

        [Test]
        public void Shift_ShouldBeActive_IfCurrentTimeWithinRange()
        {
            var testShift = new Shift
            {
                StartTime = DateTime.UtcNow.TimeOfDay.Subtract(TimeSpan.FromHours(1)),
                EndTime = DateTime.UtcNow.TimeOfDay.Add(TimeSpan.FromHours(1))
            };

            Assert.That(testShift.IsActive, Is.True);
        }

        [Test]
        public void Shift_ShouldNotBeActive_IfCurrentTimeOutsideRange()
        {
            var testShift = new Shift
            {
                StartTime = new TimeSpan(22, 0, 0), // 10:00 PM
                EndTime = new TimeSpan(6, 0, 0)   // 6:00 AM
            };

            Assert.That(testShift.IsActive, Is.False);
        }

        [Test]
        public void Shift_ShouldInitializeWithEmptyAgentList()
        {
            Assert.That(_shift.Agents, Is.Not.Null);
            Assert.That(_shift.Agents.Count, Is.EqualTo(0));
        }
    }
}
