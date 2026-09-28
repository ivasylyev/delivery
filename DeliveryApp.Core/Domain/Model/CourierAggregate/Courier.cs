using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Ddd;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Errs;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

public class Courier : Aggregate<Guid>
{
    [ExcludeFromCodeCoverage]
    private Courier()
    {
    }

    private Courier(Location location, string name) : this()
    {
        Id = Guid.NewGuid();

        Location = location;
        Name = name;

        MaxVolume = Volume.Create(20).Value;
        Assignments = new List<Assignment>();
    }

    public string Name { get; private set; }
    public Location Location { get; private set; }
    public Volume MaxVolume { get; }
    public List<Assignment> Assignments { get; }

    public static Result<Courier, Error> Create(Location location, string name)
    {
        if (location == null)
            return GeneralErrors.ValueIsRequired(nameof(location));
        if (string.IsNullOrEmpty(name))
            return GeneralErrors.ValueIsRequired(nameof(name));

        return new Courier(location, name);
    }

    public Result<bool, Error> CanAssign(Order order)
    {
        if (order == null)
            return GeneralErrors.ValueIsRequired(nameof(order));
        
        var sum = order.Volume;
        foreach (var assignment in Assignments)
        {
            sum  += assignment.Volume;
        }

        return sum <= MaxVolume;
    }

    public UnitResult<Error> AssignOrder(Order order)
    {
        if (order == null)
            return GeneralErrors.ValueIsRequired(nameof(order));
        if (CanAssign(order).Value)
        {
            var assignment = Assignment.Create(order.Id, order.Location, order.Volume).Value;
            Assignments.Add(assignment);
        }

        return UnitResult.Success<Error>();
    }

    public Result<bool, Error> CompleteOrder(Order order)
    {
        if (order == null)
            return GeneralErrors.ValueIsRequired(nameof(order));
        var assignment = Assignments.FirstOrDefault(a => a.OrderId == order.Id);
        if (assignment == null)
            return new Error("order.must.be.assigned.to.courier", "Can not complete order which is not assigned to courier");
        
        var completed = assignment.Complete(Location);

        return completed;
    }
    public UnitResult<Error> MoveTo(Location location)
    {
        if (location == null)
            return GeneralErrors.ValueIsRequired(nameof(location));

        var distance = Location.DistanceTo(location);
        if (distance > 1)
            return new Error("courier.cannot.move.further.then.one.from.current.location", "The courier cannot move further then one from current location.");

        Location = location;

        return UnitResult.Success<Error>();
    }
}