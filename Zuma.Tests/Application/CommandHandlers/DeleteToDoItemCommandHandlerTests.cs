using FluentAssertions;
using Moq;
using Zuma.Application.CommandHandlers;
using Zuma.Application.Commands;
using Zuma.Domain.Interfaces.IRepositories;

namespace Zuma.Tests.Application.CommandHandlers
{
    public class DeleteToDoItemCommandHandlerTests
    {
        private readonly Mock<IToDoItemRepository> _repositoryMock;
        private readonly DeleteToDoItemCommandHandler _handler;

        public DeleteToDoItemCommandHandlerTests()
        {
            _repositoryMock = new Mock<IToDoItemRepository>();
            _handler = new DeleteToDoItemCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenToDoItemIsDeleted()
        {
            // Arrange
            var request = new DeleteToDoCommandRequest
            {
                ToDoItemId = 1
            };

            _repositoryMock.Setup(r => r.DeleteToDoItem(It.IsAny<int>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("ToDo item deleted successfully.");
        }
    }
}
