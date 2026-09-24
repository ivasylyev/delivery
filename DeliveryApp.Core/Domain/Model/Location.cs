using CSharpFunctionalExtensions;
using Errs;

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
    private Location(int x, int y) : this()
    {
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
    ///     Создает объект типа <see cref="Location" />
    /// </summary>
    /// <param name="x">горизонталь</param>
    /// <param name="y">вертикаль</param>
    /// <returns></returns>
    public static Result<Location, Error> Create(int x, int y)
    {
        if (x < MinX || x > MaxX)
            return GeneralErrors.ValueMustBeBetween(nameof(x), x, MinX, MaxX);
        if (y < MinY || y > MaxY)
            return GeneralErrors.ValueMustBeBetween(nameof(y), y, MinY, MaxY);

        return new Location(x, y);
    }

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

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}