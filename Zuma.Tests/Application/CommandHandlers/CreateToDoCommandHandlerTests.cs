using FluentAssertions;
using Moq;
using Zuma.Application.CommandHandlers;
using Zuma.Application.Commands;
using Zuma.Domain.Interfaces.IRepositories;

namespace Zuma.Tests.Application.CommandHandlers
{
    public class CreateToDoCommandHandlerTests
    {
        private readonly Mock<IToDoItemRepository> _repositoryMock;
        private readonly CreateToDoCommandHandler _handler;

        public CreateToDoCommandHandlerTests()
        {
            _repositoryMock = new Mock<IToDoItemRepository>();
            _handler = new CreateToDoCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenToDoItemIsCreated()
        {
            // Arrange
            var request = new CreateToDoCommandRequest
            {
                Title = "Test Title",
                Description = "Test Description",
                status = Domain.Enums.ToDoStatus.JustMade
            };

            _repositoryMock.Setup(r => r.CreateToDoItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("ToDo item created successfully.");
        }
    }
}
