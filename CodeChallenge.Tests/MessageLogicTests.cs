using System;
using System.Threading.Tasks;
using CodeChallenge.Api.Logic;
using CodeChallenge.Api.Repositories;
using CodeChallenge.Models;
using FluentAssertions;
using Moq;
using Xunit;

namespace CodeChallenge.Tests
{
    public class MessageLogicTests
    {
        private readonly Mock<IMessageRepository> _repositoryMock;
        private readonly MessageLogic _logic;
        private readonly Guid _organizationId = Guid.NewGuid();

        public MessageLogicTests()
        {
            _repositoryMock = new Mock<IMessageRepository>();
            _logic = new MessageLogic(_repositoryMock.Object);
        }

        [Fact]
        public async Task CreateMessageAsync_SuccessfulCreation()
        {
            var message = new Message
            {
                Title = "Valid Title",
                Content = "This is a valid message content"
            };

            _repositoryMock
                .Setup(r => r.GetByTitleAsync(_organizationId, message.Title))
                .ReturnsAsync((Message?)null);

            _repositoryMock
                .Setup(r => r.CreateAsync(It.IsAny<Message>()))
                .ReturnsAsync(message);

            var result = await _logic.CreateMessageAsync(_organizationId, message);

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateMessageAsync_DuplicateTitle_ReturnsValidationError()
        {
            var message = new Message
            {
                Title = "Duplicate Title",
                Content = "This is a valid message content"
            };

            _repositoryMock
                .Setup(r => r.GetByTitleAsync(_organizationId, message.Title))
                .ReturnsAsync(new Message());

            var result = await _logic.CreateMessageAsync(_organizationId, message);

            result.IsInvalid.Should().BeTrue();
        }

        [Fact]
        public async Task CreateMessageAsync_InvalidContentLength_ReturnsValidationError()
        {
            var message = new Message
            {
                Title = "Valid Title",
                Content = "short"
            };

            var result = await _logic.CreateMessageAsync(_organizationId, message);

            result.IsInvalid.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateMessageAsync_NonExistentMessage_ReturnsNotFound()
        {
            var message = new Message
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Content = "This is a valid message content"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(_organizationId, message.Id))
                .ReturnsAsync((Message?)null);

            var result = await _logic.UpdateMessageAsync(_organizationId, message);

            result.IsNotFound.Should().BeTrue();
        }

        [Fact]
        public async Task UpdateMessageAsync_InactiveMessage_ReturnsValidationError()
        {
            var existing = new Message
            {
                Id = Guid.NewGuid(),
                IsActive = false
            };

            var message = new Message
            {
                Id = existing.Id,
                Title = "Updated Title",
                Content = "This is a valid message content"
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(_organizationId, message.Id))
                .ReturnsAsync(existing);

            var result = await _logic.UpdateMessageAsync(_organizationId, message);

            result.IsInvalid.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteMessageAsync_NonExistentMessage_ReturnsNotFound()
        {
            _repositoryMock
                .Setup(r => r.GetByIdAsync(_organizationId, It.IsAny<Guid>()))
                .ReturnsAsync((Message?)null);

            var result = await _logic.DeleteMessageAsync(_organizationId, Guid.NewGuid());

            result.IsNotFound.Should().BeTrue();
        }
    }
}
