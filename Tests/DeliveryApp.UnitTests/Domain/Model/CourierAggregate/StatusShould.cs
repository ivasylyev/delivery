using DeliveryApp.Core.Domain.Model.CourierAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregate;

public class StatusShould
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
        Status.Assigned.Name.Should().Be("assigned");
        Status.Completed.Name.Should().Be("completed");
    }

    /// <summary>
    ///     Проверяем корректное значения статуса
    /// </summary>
    [Fact]
    public void HaveAssignedNotEqualsToCompleted()
    {
        //Arrange

        //Act
        //Assert
        Status.Assigned.Equals(Status.Completed).Should().BeFalse();
        Status.Completed.Equals(Status.Assigned).Should().BeFalse();
    }
}