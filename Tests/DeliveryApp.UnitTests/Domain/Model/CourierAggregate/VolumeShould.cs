using DeliveryApp.Core.Domain.Model.CourierAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregate;

public class VolumeShould
{
    /// <summary>
    ///     Проверяем корректное создание при допустимых значения в литрах
    /// </summary>
    /// <param name="liters">литры</param>
    [Theory]
    [InlineData(1)]
    public void HaveValidLitersFromCtor(int liters)
    {
        //Arrange
        //Act
        var volume = Volume.Create(liters);

        //Assert
        volume.IsSuccess.Should().BeTrue();
        volume.Value.Liters.Should().Be(liters);
    }

    /// <summary>
    ///     Проверяем ошибку создания при значениях в литрах
    /// </summary>
    /// <param name="liters">литры</param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ThrowExceptionWhenCoordinatesAreInvalid(int liters)
    {
        //Arrange
        //Act
        var volume = Volume.Create(liters);
        //Assert
        volume.IsFailure.Should().BeTrue();
    }


    /// <summary>
    ///     Проверяем что Volume равен другому Volume с идентичными литрами
    /// </summary>
    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    public void BeEqualToTheSame(int liters)
    {
        //Arrange
        var first = Volume.Create(liters).Value;
        var second = Volume.Create(liters).Value;
        //Act
        var eq = first.Equals(second);
        //Assert
        Assert.True(eq);
        Assert.True(first == second);
    }

    /// <summary>
    ///     Проверяем что Volume НЕ равен другому Volume с разными литрами
    /// </summary>
    [Theory]
    [InlineData(2, 6)]
    [InlineData(10, 1)]
    public void BeNotEqualToDifferent(int l1, int l2)
    {
        //Arrange
        var first = Volume.Create(l1).Value;
        var second = Volume.Create(l2).Value;
        //Act
        var eq = first.Equals(second);
        //Assert
        Assert.False(eq);
        Assert.False(first == second);
    }
}