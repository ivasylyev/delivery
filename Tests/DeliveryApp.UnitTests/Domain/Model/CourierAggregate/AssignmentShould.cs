using System;
using DeliveryApp.Core.Domain.Model;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregate;

public class AssignmentShould
{
    /// <summary>
    ///     Проверяем корректное значения свойств назначения после создания
    /// </summary>
    [Fact]
    public void HaveCorrectPropertiesAfterCreation()
    {
        //Arrange
        var orderId = Guid.NewGuid();
        var location = Location.Create(1, 2);
        var volume = Volume.Create(5);
        var assignment = Assignment.Create(orderId, location.Value, volume.Value);

        //Act
        //Assert
        assignment.IsSuccess.Should().BeTrue();
        assignment.Value.Status.Should().BeEquivalentTo(Status.Assigned);
        assignment.Value.Volume.Should().BeEquivalentTo(volume.Value);
        assignment.Value.Location.Should().BeEquivalentTo(location.Value);
        assignment.Value.OrderId.Should().Be(orderId);
    }

    /// <summary>
    /// Проверяем невозможность завершить назначение если курьер дальше 1-й клетки от заказчика
    /// </summary>
    [Fact]
    public void NotAllowCompleteIfCourierTooFar()
    {
        //Arrange
        var orderId = Guid.NewGuid();
        var location = Location.Create(1, 2);
        var volume = Volume.Create(5);
        var assignment = Assignment.Create(orderId, location.Value, volume.Value);

        //Act
        var completed = assignment.Value.Complete(Location.Create(2, 2).Value);
        //Assert
        completed.IsSuccess.Should().BeTrue();
        assignment.Value.Status.Should().BeEquivalentTo(Status.Completed);
    }

    /// <summary>
    /// Проверяем невозможность заверишть уже завершенный заказ
    /// </summary>
    [Fact]
    public void NotAllowCompleteIfAlreadyCompeted()
    {
        //Arrange
        var orderId = Guid.NewGuid();
        var location = Location.Create(1, 2);
        var volume = Volume.Create(5);
        var assignment = Assignment.Create(orderId, location.Value, volume.Value);

        //Act
        var completed1 = assignment.Value.Complete(location.Value);
        var completed2 = assignment.Value.Complete(location.Value);
        //Assert
        completed1.IsSuccess.Should().BeTrue();
        completed2.IsSuccess.Should().BeFalse();
    }

}