using CSharpFunctionalExtensions;
using Errs;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

public class Assignment : Entity<Guid>
{
    private Assignment()
    {
    }

    private Assignment(Guid orderId, Location location, Volume volume) : this()
    {
        Id = Guid.NewGuid();

        Location = location;
        OrderId = orderId;
        Volume = volume;

        Status = Status.Assigned;
    }

    public Guid OrderId { get; private set; }
    public Volume Volume { get; private set; }
    public Location Location { get; private set; }
    public Status Status { get; private set; }

    public static Result<Assignment, Error> Create(Guid orderId, Location location, Volume volume)
    {
        if (location == null)
            return GeneralErrors.ValueIsRequired(nameof(location));
        if (volume == null)
            return GeneralErrors.ValueIsRequired(nameof(volume));

        return new Assignment(orderId, location, volume);
    }

    public Result<bool, Error> Complete(Location courierLocation)
    {
        if (courierLocation == null)
            return GeneralErrors.ValueIsRequired(nameof(courierLocation));

        if (Status == Status.Completed)
            return new Error("status.must.not.be.completed", "Assignment has already been completed");
        // Для двух статусов Assigned и Completed проверка избыточно, она существует
        // на случай добавления в будущем дополнительных статусов.
        if (Status != Status.Assigned)
            return new Error("status.must.be.assigned", "Could not complete assignment if it's not assigned");

        var distance = Location.DistanceTo(courierLocation);
        if (distance > 1)
            return new Error("courier.must.be.no.further.then.one.from.assignment.location", "The courier location must be no further then one from the assignment location.");

        Status = Status.Completed;
        return true;
    }
}