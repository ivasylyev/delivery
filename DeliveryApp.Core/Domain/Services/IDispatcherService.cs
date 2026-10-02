using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.OrderAggregate;
using Errs;

namespace DeliveryApp.Core.Domain.Services;

public interface IDispatcherService
{
     Result<Courier, Error> FindCourierAndAssignOrder(Order orderToAssign, IEnumerable<Courier> couriers);
}