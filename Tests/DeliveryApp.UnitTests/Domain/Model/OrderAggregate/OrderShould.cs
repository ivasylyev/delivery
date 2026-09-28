using System;
using DeliveryApp.Core.Domain.Model;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregate;

public class OrderShould
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
        var order = Order.Create(orderId, location.Value, volume.Value);

        //Act
        //Assert
        order.IsSuccess.Should().BeTrue();
        order.Value.Status.Should().BeEquivalentTo(OrderStatus.Created);
        order.Value.Volume.Should().BeEquivalentTo(volume.Value);
        order.Value.Location.Should().BeEquivalentTo(location.Value);
        order.Value.Id.Should().Be(orderId);
    }

    /// <summary>
    ///     Проверяем возможность назначить созданный заказ
    /// </summary>
    [Fact]
    public void AllowToAssignCreatedOrder()
    {
        //Arrange
        var order = CreateOrder();

        //Act
        var assign = order.Assign();
        //Assert
        assign.IsSuccess.Should().BeTrue();
        order.Status.Should().BeEquivalentTo(OrderStatus.Assigned);
    }

    /// <summary>
    ///     Проверяем возможность завершить назгначенный заказ
    /// </summary>
    [Fact]
    public void AllowToCompleteAssignedOrder()
    {
        //Arrange
        var order = CreateOrder();
        order.Assign();

        //Act
        var complete = order.Complete();
        //Assert
        complete.IsSuccess.Should().BeTrue();
        order.Status.Should().BeEquivalentTo(OrderStatus.Completed);
    }

    /// <summary>
    ///     Проверяем невозможность завершить заказ если он не назначен
    /// </summary>
    [Fact]
    public void NotAllowToCompleteCreatedOrder()
    {
        //Arrange
        var order = CreateOrder();

        //Act
        var complete = order.Complete();
        //Assert
        complete.IsSuccess.Should().BeFalse();
        order.Status.Should().BeEquivalentTo(OrderStatus.Created);
    }


    private static Order CreateOrder()
    {
        var orderId = Guid.NewGuid();
        var location = Location.Create(1, 2);
        var volume = Volume.Create(5);
        var order = Order.Create(orderId, location.Value, volume.Value);
        return order.Value;
    }
}