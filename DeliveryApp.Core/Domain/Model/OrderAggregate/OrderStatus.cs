using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.Model.OrderAggregate;

public class OrderStatus : ValueObject
{
    public static readonly OrderStatus Created = new(nameof(Created).ToLowerInvariant());
    public static readonly OrderStatus Assigned = new(nameof(Assigned).ToLowerInvariant());
    public static readonly OrderStatus Completed = new(nameof(Completed).ToLowerInvariant());

    [ExcludeFromCodeCoverage]
    private OrderStatus()
    {
    }

    private OrderStatus(string name) : this()
    {
        Name = name;
    }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Список всех значений <see cref="OrderStatus"/>
    /// </summary>
    /// <returns>Список значений</returns>
    public IEnumerable<OrderStatus> List()
    {
        yield return Created;
        yield return Assigned;
        yield return Completed;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}
