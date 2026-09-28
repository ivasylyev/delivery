using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Errs;

namespace DeliveryApp.Core.Domain.Services;

public class DispatcherService : IDispatcherService
{
    public Result<Courier, Error> FindCourierAndAssignOrder(Order orderToAssign, IEnumerable<Courier> couriers)
    {
        ArgumentNullException.ThrowIfNull(orderToAssign);
        ArgumentNullException.ThrowIfNull(couriers);

        // исключаем всех курьеров, которым нельзя назначить заказ
        var assignable = couriers.Where(c => c.CanAssign(orderToAssign).Value).ToList();
        var candidate = new CourierPointer();
        foreach (var courier in assignable)
        {
            var currentDistance = orderToAssign.Location.DistanceTo(courier.Location);
            var foundBetterCandidate = false;
            // если найден курьер на такоем же расстоянии, считаем лучшим кандидатом того, кто меньше заполнен.
            // Этого не было в задании, но так мы избежим неравномерного распределения заказов
            // когда часть курьеров будет простаивать только потому, что они оказались в конце списка
            if (candidate.MinDistance == currentDistance)
            {
                var newFillFactor = courier.GetFillFactor().Value;
                foundBetterCandidate = candidate.FillFactor > newFillFactor;
            }
            // если найдент курьер на меньшем расстоянии, считаем его лучшим кандидатом.
            else
            {
                foundBetterCandidate = candidate.MinDistance > currentDistance;
            }

            if (foundBetterCandidate) candidate = new CourierPointer(courier, currentDistance);
        }

        if (candidate.Courier == null)
            return new Error("could.not.find.available.courier", "Can not find available courier to assign the order to");

        candidate.Courier.AssignOrder(orderToAssign);
        orderToAssign.Assign();

        return candidate.Courier;
    }

    /// <summary>
    /// Вспомогательный класс для кеширования связанных с курьером величин: степень его заполенности и расстояние до заказа
    /// </summary>
    private class CourierPointer
    {
        public CourierPointer()
        {
            Courier = null;
            MinDistance = int.MaxValue;
            FillFactor = 1;
        }

        public CourierPointer(Courier courier, int minDistance)
        {
            Courier = courier;
            FillFactor = courier.GetFillFactor().Value;
            MinDistance = minDistance;
        }

        public Courier? Courier { get; }
        public int MinDistance { get; }
        public double FillFactor { get; }
    }
}