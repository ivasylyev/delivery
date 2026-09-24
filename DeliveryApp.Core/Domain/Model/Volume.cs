using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Errs;

namespace DeliveryApp.Core.Domain.Model;

public class Volume : ValueObject
{
    [ExcludeFromCodeCoverage]
    private Volume()
    {
    }

    private Volume(int liters) : this()
    {
        Liters = liters;
    }

    public int Liters { get; }

    public static Result<Volume, Error> Create(int liters)
    {
        if (liters <= 0)
            return GeneralErrors.ValueMustBeGreaterThan(nameof(liters), liters, 0);
        return new Volume(liters);
    }

    public static Volume operator +(Volume v1, Volume v2)
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);

        return Create(v1.Liters + v2.Liters).Value;
    }

    public static Volume operator -(Volume v1, Volume v2)
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);
        if (v1.Liters <= v2.Liters)
            throw new InvalidOperationException("The minuend must be greater than the subtrahend.");

        return Create(v1.Liters - v2.Liters).Value;
    }

    public static bool operator <(Volume v1, Volume v2)
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);

        return v1.Liters < v2.Liters;
    }

    public static bool operator >(Volume v1, Volume v2)
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);

        return v1.Liters > v2.Liters;
    }

    public static bool operator <=(Volume v1, Volume v2)
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);

        return v1.Liters <= v2.Liters;
    }

    public static bool operator >=(Volume v1, Volume v2)
    {
        ArgumentNullException.ThrowIfNull(v1);
        ArgumentNullException.ThrowIfNull(v2);

        return v1.Liters >= v2.Liters;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Liters;
    }

    public override string ToString()
    {
        return $"Liters:{Liters}";
    }
}