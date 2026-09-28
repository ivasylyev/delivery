using System;
using System.Linq;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using DeliveryApp.Core.Domain.Services;
using Errs;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Services;

public class DispatcherServiceShould
{
    /// <summary>
    /// Проверяем невозможность назначить заказ, если нет курьеров
    /// </summary>
    [Fact]
    public void ReturnErrorIfCourierListIsEmpty()
    {
        //Arrange
        var order = CreateOrder(1, 2, 5);
        IDispatcherService service = new DispatcherService();

        //Act
        var result = service.FindCourierAndAssignOrder(order.Value, Array.Empty<Courier>());

        //Assert
        result.IsSuccess.Should().BeFalse();
        order.Value.Status.Should().BeEquivalentTo(OrderStatus.Created);
    }

    /// <summary>
    /// Проверяем невозможность назначить заказ, если курьеры полны
    /// </summary>
    [Fact]
    public void ReturnErrorIfCouriersAreFull()
    {
        //Arrange
        var order = CreateOrder(1, 2, 5);

        var order1 = CreateOrder(1, 2, 16);
        var order2 = CreateOrder(1, 2, 16);
        var courier1 = CreateCourier("test_1", 5, 6);
        var courier2 = CreateCourier("test_2", 3, 4);
        courier1.Value.AssignOrder(order1.Value);
        courier2.Value.AssignOrder(order2.Value);
        IDispatcherService service = new DispatcherService();

        //Act
        var result = service.FindCourierAndAssignOrder(order.Value, [courier1.Value, courier2.Value]);

        //Assert
        result.IsSuccess.Should().BeFalse();
        order.Value.Status.Should().BeEquivalentTo(OrderStatus.Created);
    }
    /// <summary>
    /// Проверяем возможность назначить заказ, на ближайшего курьера
    /// </summary>
    [Fact]
    public void AssignOrderToTheNearestCourier()
    {
        //Arrange
        var order = CreateOrder(1, 2, 5);
        var courier1 = CreateCourier("test_1", 5, 6);
        var courier2 = CreateCourier("test_2", 3, 4);
        IDispatcherService service = new DispatcherService();
        //Act
        var result = service.FindCourierAndAssignOrder(order.Value, [courier1.Value, courier2.Value]);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(courier2.Value.Name);
        order.Value.Status.Should().Be(OrderStatus.Assigned);
        result.Value.Assignments.Count.Should().Be(1);
        var assignment = result.Value.Assignments.First();
        assignment.OrderId.Should().Be(order.Value.Id);
        assignment.Status.Should().Be(Status.Assigned);
    }
    /// <summary>
    /// Проверяем возможность назначить заказ, на ближайшего курьера который меньше заполнен
    /// </summary>
    [Fact]
    public void AssignOrderToTheMostEmptyCourier()
    {
        //Arrange
        var order1 = CreateOrder(1, 2, 5);
        var order2 = CreateOrder(1, 2, 5);
        var order3 = CreateOrder(1, 2, 5);
        var courier1 = CreateCourier("test_1", 5, 6);
        var courier2 = CreateCourier("test_2", 5, 6);
        var courier3 = CreateCourier("test_3", 5, 6);
        courier1.Value.AssignOrder(order1.Value);
        courier3.Value.AssignOrder(order3.Value);
        IDispatcherService service = new DispatcherService();
        //Act
        var result = service.FindCourierAndAssignOrder(order2.Value, [courier1.Value, courier2.Value, courier3.Value]);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(courier2.Value.Name);
    }

    private static Result<Courier, Error> CreateCourier(string name, int x, int y)
    {
        var location = Location.Create(x, y);
        var courier = Courier.Create(location.Value, name);
        return courier;
    }


    private static Result<Order, Error> CreateOrder(int x, int y, int liters)
    {
        var orderId = Guid.NewGuid();
        var location = Location.Create(x, y);
        var volume = Volume.Create(liters);
        var order = Order.Create(orderId, location.Value, volume.Value);
        return order;
    }
}