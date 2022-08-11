using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* A component of the Elevator that acts as the brain, it handles ElevatorStopRequests */
public class ElevatorOperator
{
   /* We pass on the elevatorTransform as it can be useful in subclasses, I would usually avoid including 
   members that aren't used by all subclasses, but I chose to allow it in this case as many subclasses 
   may use it */
   protected Transform elevatorTransform;

   // Requests are stored in a queue, we always handle the request at the front of the queue.
   protected Queue<ElevatorStopRequest> requestedStops;

   internal ElevatorOperator(Transform elevatorTransform)
   {
      requestedStops = new Queue<ElevatorStopRequest>();
      this.elevatorTransform = elevatorTransform;
   }

   /* This can be overriden to change the order in which the elevator stop requests will be handled
   but in this basic implementation it just works on a first come first serve basis and queues requests accordingly */
   internal virtual bool TryHandleElevatorRequest(ElevatorStopRequest request)
   {
      if (requestedStops.Contains(request)) return false;

      /* In the base implementation of the ElevatorOperatingMode we queue requests without considering direction, so if we recieve a request for the same floor
      but a different direction we don't add it to the queue and essentially ignore that request */
      var sameStop = requestedStops.Where((s) => s.requestedStop == request.requestedStop);
      if (sameStop.Count() == 0)
      {
         request.source.LinkToRequest(request);
         requestedStops.Enqueue(request);
         return true;
      }
      else
      {
         request.source.LinkToRequest(sameStop.First());
      }
      return true;
   }

   // Checks that there are active requests in the queue
   internal bool HasActiveRequests()
   {
      return requestedStops.Count != 0;
   }

   // Returns the stop request at the front of the queue
   internal ElevatorStopRequest GetStopToMoveTo()
   {
      return requestedStops.Peek();
   }

   // Dequeues the last request so that the next in the queue can be fullfilled.
   internal ElevatorStopRequest OnCleanUpRequest()
   {
      return requestedStops.Dequeue();
   }
}
