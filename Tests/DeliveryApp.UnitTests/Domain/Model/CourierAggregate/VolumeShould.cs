using DeliveryApp.Core.Domain.Model;
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
        eq.Should().BeTrue();
        first.Should().BeEquivalentTo(second);
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
        eq.Should().BeFalse();
        first.Should().NotBeEquivalentTo(second);
    }

    /// <summary>
    ///     Проверяем сложение Volume
    /// </summary>
    [Theory]
    [InlineData(2, 6, 8)]
    [InlineData(1, 1, 2)]
    public void CalculateSum(int l1, int l2, int ld)
    {
        //Arrange
        var first = Volume.Create(l1).Value;
        var second = Volume.Create(l2).Value;
        //Act
        var sum = first + second;
        //Assert
        sum.Liters.Should().Be(ld);
    }

    /// <summary>
    ///     Проверяем вычитаение Volume
    /// </summary>
    [Theory]
    [InlineData(6, 1, 5)]
    [InlineData(10, 4, 6)]
    public void CalculateDifference(int l1, int l2, int ld)
    {
        //Arrange
        var first = Volume.Create(l1).Value;
        var second = Volume.Create(l2).Value;
        //Act
        var sum = first - second;
        //Assert
        sum.Liters.Should().Be(ld);
    }

    /// <summary>
    ///     Проверяем меньше или равно Volume
    /// </summary>
    [Theory]
    [InlineData(6, 1, false)]
    [InlineData(10, 4, false)]
    [InlineData(5, 5, true)]
    [InlineData(1, 5, true)]
    public void CompareLessOrEqual(int l1, int l2, bool res)
    {
        //Arrange
        var first = Volume.Create(l1).Value;
        var second = Volume.Create(l2).Value;
        //Act
        var lessOrEqual = first <= second;
        //Assert
        lessOrEqual.Should().Be(res);
    }

    /// <summary>
    ///     Проверяем больше или равно Volume
    /// </summary>
    [Theory]
    [InlineData(6, 1, true)]
    [InlineData(10, 4, true)]
    [InlineData(5, 5, true)]
    [InlineData(1, 5, false)]
    public void CompareMoreOrEqual(int l1, int l2, bool res)
    {
        //Arrange
        var first = Volume.Create(l1).Value;
        var second = Volume.Create(l2).Value;
        //Act
        var lessOrEqual = first >= second;
        //Assert
        lessOrEqual.Should().Be(res);
    }

    /// <summary>
    ///     Проверяем меньше или равно Volume
    /// </summary>
    [Theory]
    [InlineData(6, 1, false)]
    [InlineData(10, 4, false)]
    [InlineData(5, 5, false)]
    [InlineData(1, 5, true)]
    public void CompareLess(int l1, int l2, bool res)
    {
        //Arrange
        var first = Volume.Create(l1).Value;
        var second = Volume.Create(l2).Value;
        //Act
        var lessOrEqual = first < second;
        //Assert
        lessOrEqual.Should().Be(res);
    }

    /// <summary>
    ///     Проверяем больше или равно Volume
    /// </summary>
    [Theory]
    [InlineData(6, 1, true)]
    [InlineData(10, 4, true)]
    [InlineData(5, 5, false)]
    [InlineData(1, 5, false)]
    public void CompareMore(int l1, int l2, bool res)
    {
        //Arrange
        var first = Volume.Create(l1).Value;
        var second = Volume.Create(l2).Value;
        //Act
        var lessOrEqual = first > second;
        //Assert
        lessOrEqual.Should().Be(res);
    }
}