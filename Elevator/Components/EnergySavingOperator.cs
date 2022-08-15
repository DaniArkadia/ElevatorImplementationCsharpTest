using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* This ElevatorOperator will always choose the closest stops first, making it the most energy efficient relatively as it won't bounce around as much as others*/
public class EnergySavingOperator : ElevatorOperator
{
   internal EnergySavingOperator(Transform elevatorTransform) : base(elevatorTransform)
   {
      this.elevatorTransform = elevatorTransform;
      requestedStops = new Queue<ElevatorStopRequest>();
   }

   /* In this implementation we order our queue by distance */
   internal override bool TryHandleElevatorRequest(ElevatorStopRequest request)
   {
      if (requestedStops.Contains(request)) return false;

      var sameStop = requestedStops.Where((s) => s.requestedStop == request.requestedStop);
      if (sameStop.Count() == 0)
      {
         requestedStops.Enqueue(request);
         var updatedQueue = requestedStops.OrderBy((r) => Vector3.Distance(elevatorTransform.position, r.requestedStop.stopTransform.position));

         Queue<ElevatorStopRequest> tempQueue = new Queue<ElevatorStopRequest>();

         foreach (var req in updatedQueue)
         {
            tempQueue.Enqueue(req);
         }
         requestedStops = tempQueue;
         return true;
      }
      return true;
   }
}
