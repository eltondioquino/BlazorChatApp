using NUnit.Framework;
using ChatApp.Domain.Entities;
using System;

namespace ChatApp.Tests.Domain.Entities
{
    [TestFixture]
    public class ChatSessionTests
    {
        private ChatSession _chatSession;

        [SetUp]
        public void Setup()
        {
            _chatSession = new ChatSession
            {
                UserName = "TestUser",
                Message = "Hello, this is a test message.",
                AssignedAgentId = null,
                CreatedAt = DateTime.UtcNow,
                LastPolledAt = null,
                LastActiveTime = DateTime.UtcNow,
                Status = ChatApp.Domain.Enum.Enum.ChatStatus.Pending
            };
        }

        [Test]
        public void ChatSession_ShouldHaveUniqueChatSessionId()
        {
            var anotherChatSession = new ChatSession();
            Assert.That(_chatSession.ChatSessionId, Is.Not.EqualTo(anotherChatSession.ChatSessionId));
        }

        [Test]
        public void ChatSession_DefaultStatus_ShouldBePending()
        {
            Assert.That(_chatSession.Status, Is.EqualTo(ChatApp.Domain.Enum.Enum.ChatStatus.Pending));
        }

        [Test]
        public void ChatSession_CreatedAt_ShouldBeSetToUtcNow()
        {
            var now = DateTime.UtcNow;
            Assert.That(_chatSession.CreatedAt, Is.EqualTo(now).Within(TimeSpan.FromSeconds(1)));
        }

        [Test]
        public void ChatSession_LastPolledAt_ShouldBeNullByDefault()
        {
            Assert.That(_chatSession.LastPolledAt, Is.Null);
        }
    }
}
