using System;
using System.Linq;
using DeliveryApp.Core.Domain.Model;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregate;

public class CourierShould
{
    /// <summary>
    ///     Проверяем корректное значения свойств назначения после создания
    /// </summary>
    [Fact]
    public void HaveCorrectPropertiesAfterCreation()
    {
        //Arrange
        var location = Location.Create(1, 2);
        var name = "Test";
        var courier = Courier.Create(location.Value, name);

        //Act
        //Assert
        courier.IsSuccess.Should().BeTrue();
        courier.Value.Location.Should().BeEquivalentTo(location.Value);
        courier.Value.Name.Should().BeEquivalentTo(name);
    }

    /// <summary>
    ///     Проверяем возможность назначить заказ
    /// </summary>
    [Fact]
    public void AllowToAssignValidOrder()
    {
        //Arrange
        var order = CreateTestOrder(5);
        var location = Location.Create(3, 4);
        var name = "Test";
        var courier = Courier.Create(location.Value, name);

        //Act
        var assign = courier.Value.CanAssign(order);

        //Assert
        assign.IsSuccess.Should().BeTrue();
        assign.Value.Should().BeTrue();
    }

    /// <summary>
    ///     Проверяем назначиение заказа
    /// </summary>
    [Fact]
    public void BeAbleToAssignValidOrder()
    {
        //Arrange
        var order = CreateTestOrder(5);
        var location = Location.Create(3, 4);
        var name = "Test";
        var courier = Courier.Create(location.Value, name);

        //Act
        var assign = courier.Value.AssignOrder(order);
        //Assert
        assign.IsSuccess.Should().BeTrue();
        courier.Value.Assignments.Count.Should().Be(1);
        courier.Value.Assignments.First().Status.Should().Be(Status.Assigned);
    }


    /// <summary>
    ///     Проверяем невозможность назначить слишком большой заказ
    /// </summary>
    [Fact]
    public void NotAllowAssignHugeOrder()
    {
        //Arrange
        // заказ на 50 литров!
        var order = CreateTestOrder(50);
        var location = Location.Create(3, 4);
        var name = "Test";
        var courier = Courier.Create(location.Value, name);

        //Act
        var assign = courier.Value.CanAssign(order);

        //Assert
        assign.IsSuccess.Should().BeTrue();
        assign.Value.Should().BeFalse();
    }

    /// <summary>
    ///     Проверяем завершение заказа если курьер в соседней клетке
    /// </summary>
    [Fact]
    public void BeAbleToAssignAndCompleteValidOrderIdIfHeIsNearby()
    {
        //Arrange
        var order = CreateTestOrder(5);
        var location = Location.Create(1, 1);
        var name = "Test";
        var courier = Courier.Create(location.Value, name);
        courier.Value.AssignOrder(order);

        //Act
        var completeOrder = courier.Value.CompleteOrder(order);
        //Assert
        completeOrder.IsSuccess.Should().BeTrue();
        courier.Value.Assignments.First().Status.Should().Be(Status.Completed);
    }

    /// <summary>
    ///     Проверяем невозможность завершения заказа если курьер слишком далеко
    /// </summary>
    [Fact]
    public void NotBeAbleToAssignAndCompleteValidOrderIdIfHeIsTooFarOrder()
    {
        //Arrange
        var order = CreateTestOrder(5);
        var location = Location.Create(10, 10);
        var name = "Test";
        var courier = Courier.Create(location.Value, name);
        courier.Value.AssignOrder(order);

        //Act
        var completeOrder = courier.Value.CompleteOrder(order);
        //Assert
        completeOrder.IsSuccess.Should().BeFalse();
        courier.Value.Assignments.First().Status.Should().Be(Status.Assigned);
    }


    /// <summary>
    ///     Проверяем перемещение курьера на 1 клетку
    /// </summary>
    [Fact]
    public void BeAbleToMoveOneStep()
    {
        //Arrange
        var start = Location.Create(1, 1);
        var finish = Location.Create(1, 2);
        var name = "Test";
        var courier = Courier.Create(start.Value, name);

        //Act
        var move = courier.Value.MoveTo(finish.Value);
        //Assert
        move.IsSuccess.Should().BeTrue();
        courier.Value.Location.Should().Be(finish.Value);
    }

    /// <summary>
    ///     Проверяем невозможность перемещения курьера более чем на 1 клетку
    /// </summary>
    [Fact]
    public void NotBeAbleToMoveTwoSteps()
    {
        //Arrange
        var start = Location.Create(1, 1);
        var finish = Location.Create(2, 2);
        var name = "Test";
        var courier = Courier.Create(start.Value, name);

        //Act
        var move = courier.Value.MoveTo(finish.Value);
        //Assert
        move.IsSuccess.Should().BeFalse();
        courier.Value.Location.Should().Be(start.Value);
    }

    private static Order CreateTestOrder(int liters)
    {
        var orderId = Guid.NewGuid();
        var orderLocation = Location.Create(1, 2);
        var volume = Volume.Create(liters);
        var order = Order.Create(orderId, orderLocation.Value, volume.Value);
        return order.Value;
    }
}