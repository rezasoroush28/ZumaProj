using FluentAssertions;
using Moq;
using Zuma.Application.CommandHandlers;
using Zuma.Application.Commands;
using Zuma.Domain.Interfaces.IRepositories;

namespace Zuma.Tests.Application.CommandHandlers
{
    public class UpdateToDoItemCommandHandlerTests
    {
        private readonly Mock<IToDoItemRepository> _repositoryMock;
        private readonly UpdateToDoItemCommandHandler _handler;

        public UpdateToDoItemCommandHandlerTests()
        {
            _repositoryMock = new Mock<IToDoItemRepository>();
            _handler = new UpdateToDoItemCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenToDoItemIsUpdated()
        {
            // Arrange
            var request = new UpdateToDoItemCommandRequest
            {
                Id = 1,
                Title = "Updated Title",
                Description = "Updated Description",
                Status = Domain.Enums.ToDoStatus.Done
            };

            _repositoryMock.Setup(r => r.UpdateToDoItem(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("ToDo item Updated successfully.");
        }
    }
}
