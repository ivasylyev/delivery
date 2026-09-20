using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Errs;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

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

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Liters;
    }
}