using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.Model;

public class Location : ValueObject
{
    //В реальном проекте размеры должны инжектится из конфига или базы. В учебном проекте допустимо задать их в виде константю

    /// <summary>
    ///     Минимально возможная для установки координата X
    /// </summary>
    public const int MinX = 1;

    /// <summary>
    ///     Минимально возможная для установки координата Y
    /// </summary>
    public const int MinY = 1;

    /// <summary>
    ///     Максимально  возможная для установки координата X
    /// </summary>
    public const int MaxX = 10;

    /// <summary>
    ///     Максимально  возможная для установки координата Y
    /// </summary>
    public const int MaxY = 10;

    /// <summary>
    ///     Конструктор приватный чтобы запретить создавать пустой объект
    /// </summary>
    private Location()
    {
    }

    /// <summary>
    ///     Координата на доске, которая состоит из X (горизонталь) и Y (вертикаль)
    /// </summary>
    /// <param name="x">горизонталь</param>
    /// <param name="y">вертикаль</param>
    public Location(int x, int y) : this()
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(x, MinX);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(x, MaxX);
        ArgumentOutOfRangeException.ThrowIfLessThan(y, MinY);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(y, MaxY);

        X = x;
        Y = y;
    }

    /// <summary>
    ///     координата по горизонтали
    /// </summary>
    public int X { get; }

    /// <summary>
    ///     координата по вертикали
    /// </summary>
    public int Y { get; }

    /// <summary>
    ///     Рассчитывает расстояние до другого Location
    /// </summary>
    /// <param name="another">Location, до которого рассчитывается расстояние</param>
    /// <returns>Расстояние до другого Location </returns>
    public int DistanceTo(Location another)
    {
        ArgumentNullException.ThrowIfNull(another);

        var distance = Math.Abs(X - another.X) + Math.Abs(Y - another.Y);
        return distance;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}