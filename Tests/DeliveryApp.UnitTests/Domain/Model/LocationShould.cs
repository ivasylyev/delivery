using System;
using DeliveryApp.Core.Domain.Model;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Model;

public class LocationShould
{
    /// <summary>
    ///     Проверяем корректное создангие при значениях координат на границе, а также значения внутри допустимого диапазона
    ///     Формально это один класс эквивалентности, но на практике граничные значения лучше проверять отдельно
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 10)]
    [InlineData(5, 2)]
    public void HaveValidCoordinatesGivenFromCtor(int x, int y)
    {
        //Arrange
        //Act
        var location = Location.Create(x, y);

        //Assert
        location.IsSuccess.Should().BeTrue();
        location.Value.X.Should().Be(x);
        location.Value.Y.Should().Be(y);
    }

    /// <summary>
    ///     Проверяем ошибку создания при значениях координат вне допустимого диапазона
    ///     Формально это один класс эквивалентности, но на практике граничные значения +-1, а также отрицательные знчения
    ///     лучше проверять отдельно
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    [Theory]
    [InlineData(0, 5)]
    [InlineData(5, 0)]
    [InlineData(11, 5)]
    [InlineData(5, 11)]
    [InlineData(-1, 3)]
    [InlineData(3, -1)]
    public void ThrowExceptionWhenCoordinatesAreInvalid(int x, int y)
    {
        //Arrange
        //Act
        var result = Location.Create(x, y);
        //Assert
        result.IsFailure.Should().BeTrue();
    }

    /// <summary>
    ///     Проверяем что расстояние Location до самой себя равно нулю
    /// </summary>
    [Fact]
    public void MeasureDistanceToItself()
    {
        //Arrange
        var me = Location.Create(1, 2).Value;
        //Act
        var distanceTo = me.DistanceTo(me);
        //Assert
        Assert.Equal(0, distanceTo);
    }

    /// <summary>
    ///     Проверяем растояние между двумя валидными Location
    /// </summary>
    /// <param name="x1"></param>
    /// <param name="y1"></param>
    /// <param name="x2"></param>
    /// <param name="y2"></param>
    /// <param name="givenDistance"></param>
    [Theory]
    [InlineData(2, 6, 4, 9, 5)]
    [InlineData(1, 1, 10, 10, 18)]
    [InlineData(5, 3, 5, 7, 4)]
    [InlineData(3, 5, 7, 5, 4)]
    public void MeasureDistanceToAnotherLocation(int x1, int y1, int x2, int y2, int givenDistance)
    {
        //Arrange
        var first = Location.Create(x1, y1).Value;
        var second = Location.Create(x2, y2).Value;
        //Act
        var firstDistanceTo = first.DistanceTo(second);
        var secondDistanceTo = second.DistanceTo(first);
        //Assert
        Assert.Equal(givenDistance, firstDistanceTo);
        Assert.Equal(givenDistance, secondDistanceTo);
    }

    /// <summary>
    ///     Проверяем ошибку вычитсления расстояния до Location являющейся NULL
    /// </summary>
    [Fact]
    public void ThrowExceptionWhenMeasureDistanceToNull()
    {
        //Arrange
        var me = Location.Create(1, 2);

        //Act
        //Assert
        Assert.Throws<ArgumentNullException>(() => me.Value.DistanceTo(null!));
    }

    /// <summary>
    ///     Проверяем что Location равен сам себе
    /// </summary>
    [Fact]
    public void BeEqualToItself()
    {
        //Arrange
        var first = Location.Create(1, 2).Value;
        var second = first;
        //Act
        var eq = first.Equals(second);
        //Assert
        Assert.True(eq);
    }

    /// <summary>
    ///     Проверяем что Location равен другому Location с идентичными координатами
    /// </summary>
    [Theory]
    [InlineData(1, 1)]
    [InlineData(10, 10)]
    [InlineData(5, 2)]
    public void BeEqualToTheSame(int x, int y)
    {
        //Arrange
        var first = Location.Create(x, y).Value;
        var second = Location.Create(x, y).Value;
        //Act
        var eq = first.Equals(second);
        //Assert
        Assert.True(eq);
        Assert.True(first == second);
    }

    /// <summary>
    ///     Проверяем что Location НЕ равен другому Location с другими координатами.
    ///     Проверяемые классы эквивалентности: все координаты различны, координаты X различны, координаты Y различны
    /// </summary>
    [Theory]
    [InlineData(2, 6, 4, 9)]
    [InlineData(1, 1, 10, 10)]
    [InlineData(5, 3, 5, 7)]
    [InlineData(3, 5, 7, 5)]
    public void BeNotEqualToDifferent(int x1, int y1, int x2, int y2)
    {
        //Arrange
        var first = Location.Create(x1, y1).Value;
        var second = Location.Create(x2, y2).Value;
        //Act
        var eq = first.Equals(second);
        //Assert
        Assert.False(eq);
        Assert.False(first == second);
    }
}