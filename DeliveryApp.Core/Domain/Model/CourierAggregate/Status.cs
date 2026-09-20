using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

public class Status : ValueObject
{
    public static Status Assigned = new(nameof(Assigned).ToLowerInvariant());
    public static Status Completed = new(nameof(Completed).ToLowerInvariant());

    [ExcludeFromCodeCoverage]
    private Status()
    {
    }

    private Status(string name) : this()
    {
        Name = name;
    }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Список всех значений <see cref="Status"/>
    /// </summary>
    /// <returns>Список значений</returns>
    public IEnumerable<Status> List()
    {
        yield return Assigned;
        yield return Completed;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}