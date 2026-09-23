
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Model.OrderAggregate;

public class OrderStatusShould
{
    /// <summary>
    ///     Проверяем корректное значения статуса
    /// </summary>
    [Fact]
    public void HaveAssignedAndCompleted()
    {
        //Arrange

        //Act
        //Assert
        OrderStatus.Created.Name.Should().Be("created");
        OrderStatus.Assigned.Name.Should().Be("assigned");
        OrderStatus.Completed.Name.Should().Be("completed");
    }

    /// <summary>
    ///     Проверяем неравенство Completed и Assigned
    /// </summary>
    [Fact]
    public void HaveAssignedNotEqualsToCompleted()
    {
        //Arrange

        //Act
        //Assert
        OrderStatus.Assigned.Equals(OrderStatus.Completed).Should().BeFalse();
        OrderStatus.Completed.Equals(OrderStatus.Assigned).Should().BeFalse();
    }

    /// <summary>
    ///     Проверяем неравенство Completed и Created
    /// </summary>
    [Fact]
    public void HaveCreatedNotEqualsToCompleted()
    {
        //Arrange

        //Act
        //Assert
        OrderStatus.Assigned.Equals(OrderStatus.Completed).Should().BeFalse();
        OrderStatus.Completed.Equals(OrderStatus.Created).Should().BeFalse();
    }
}