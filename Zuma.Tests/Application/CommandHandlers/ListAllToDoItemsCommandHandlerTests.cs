using FluentAssertions;
using Moq;
using Zuma.Application.CommandHandlers;
using Zuma.Application.Commands;
using Zuma.Domain.Interfaces.IRepositories;
using Zuma.Domain.Entities;
using Zuma.Domain.Enums;

namespace Zuma.Tests.Application.CommandHandlers
{
    public class ListAllToDoItemsCommandHandlerTests
    {
        private readonly Mock<IToDoItemRepository> _repositoryMock;
        private readonly ListAllToDoItemsCommandHandler _handler;

        public ListAllToDoItemsCommandHandlerTests()
        {
            _repositoryMock = new Mock<IToDoItemRepository>();
            _handler = new ListAllToDoItemsCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnAllToDoItems_WhenStatusIsNull()
        {
            // Arrange
            var todoItems = new List<ListAllToDoItemsDataDto>
            {
                new ListAllToDoItemsDataDto { Id = 1, Title = "Task 1", Description = "Desc 1", Status = ToDoStatus.JustMade },
                new ListAllToDoItemsDataDto { Id = 2, Title = "Task 2", Description = "Desc 2", Status = ToDoStatus.Done }
            };

            _repositoryMock.Setup(r => r.ListAllToDoItems(It.IsAny<ToDoStatus?>()))
                .ReturnsAsync(todoItems);

            var request = new ListAllToDoItemsCommandRequest();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Result.Should().HaveCount(2);
        }

        [Fact]
        public async Task Handle_ShouldReturnFilteredToDoItems_WhenStatusIsProvided()
        {
            // Arrange
            var todoItems = new List<ToDoItem>
            {
                new ToDoItem { Id = 1, Title = "Task 1", Description = "Desc 1", Status = ToDoStatus.JustMade }
            };

            

            _repositoryMock.Setup(r => r.ListAllToDoItems(ToDoStatus.JustMade))
                .ReturnsAsync(new List<ListAllToDoItemsDataDto>());

            var request = new ListAllToDoItemsCommandRequest { Status = ToDoStatus.JustMade };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Result.Should().OnlyContain(x => x.Status == ToDoStatus.JustMade);
        }
    }
}
