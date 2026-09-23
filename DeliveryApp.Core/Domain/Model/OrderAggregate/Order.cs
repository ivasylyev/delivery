using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Ddd;
using Errs;

namespace DeliveryApp.Core.Domain.Model.OrderAggregate;

public class Order : Aggregate<Guid>
{
    [ExcludeFromCodeCoverage]
    private Order()
    {
    }

    private Order(Guid id, Location location, Volume volume) : this()
    {
        Id = id;

        Location = location;
        Volume = volume;

        Status = OrderStatus.Created;
    }

    public Volume Volume { get; private set; }
    public Location Location { get; private set; }
    public OrderStatus Status { get; private set; }

    public static Result<Order, Error> Create(Guid id, Location location, Volume volume)
    {
        if (id == Guid.Empty)
            return new Error("id.must.not.be.empty", "Provided Id must not be Guid.Empty");
        if (location == null)
            return GeneralErrors.ValueIsRequired(nameof(location));
        if (volume == null)
            return GeneralErrors.ValueIsRequired(nameof(volume));

        return new Order(id, location, volume);
    }

    public Result<bool, Error> Assign()
    {
        if (Status == OrderStatus.Assigned)
            return new Error("status.must.not.be.assigned", "Order has already been assigned");
        if (Status != OrderStatus.Created)
            return new Error("status.must.be.created", "Could not assign Order if it's not created");

        Status = OrderStatus.Assigned;
        return true;
    }

    public Result<bool, Error> Complete()
    {
        if (Status == OrderStatus.Completed)
            return new Error("status.must.not.be.completed", "Order has already been completed");
        if (Status != OrderStatus.Assigned)
            return new Error("status.must.be.assigned", "Could not complete Order if it's not assigned");

        Status = OrderStatus.Completed;
        return true;
    }

}