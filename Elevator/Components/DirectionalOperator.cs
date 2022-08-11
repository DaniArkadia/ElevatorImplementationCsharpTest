using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* This ElevatorOperator will alternate between moving up and moving down, unfortunately I ran out of time before this Operator could be completed,
however I thought I'd leave it here as you can see where I was going with it. */
public class DirectionalOperator : ElevatorOperator
{
   enum MotionState { up, down };

   Queue<ElevatorStopRequest> nextUpRequests;
   Queue<ElevatorStopRequest> nextDownRequests;

   MotionState motionState;


   internal DirectionalOperator(Transform elevatorTransform) : base(elevatorTransform)
   {
      this.elevatorTransform = elevatorTransform;

      requestedStops = new Queue<ElevatorStopRequest>();
      nextUpRequests = new Queue<ElevatorStopRequest>();
      nextDownRequests = new Queue<ElevatorStopRequest>();
   }

   /* In this implementation we alternate between moving upwards and downwards */
   internal override bool TryHandleElevatorRequest(ElevatorStopRequest request)
   {   
      // If there are no active requests
      if (requestedStops.Count == 0)
      {
         if (nextDownRequests.Count == 0 && nextUpRequests.Count == 0)
         {
            motionState = request.direction == ElevatorStopRequest.RequestDirection.up ? MotionState.up : MotionState.down;
            request.source.LinkToRequest(request);
            requestedStops.Enqueue(request);
            return true;
         }
         if (motionState == MotionState.down)
         {
            if (nextDownRequests.Count > 0)
            {
               request.source.LinkToRequest(request);
               nextDownRequests.Enqueue(request);
               requestedStops = nextDownRequests;
               return true;
            }
         }
         else if (motionState == MotionState.up)
         {
            request.source.LinkToRequest(request);
            nextUpRequests.Enqueue(request);
            requestedStops = nextUpRequests;
            return true;
         }
      }
      else if (motionState == MotionState.up)
      {
         if (request.direction == ElevatorStopRequest.RequestDirection.up)
         {
            // If we're moving up and an up request is sent from a stop below us, we add the request to a new queue
            if (request.requestedStop.stopTransform.position.y < elevatorTransform.position.y)
            {
               request.source.LinkToRequest(request);
               nextUpRequests.Enqueue(request);
            }
            else
            {
               request.source.LinkToRequest(request);
               requestedStops.Enqueue(request);
            }
         }
         if (request.direction == ElevatorStopRequest.RequestDirection.down)
         {
            request.source.LinkToRequest(request);
            nextDownRequests.Enqueue(request);
         }
      }
      else if (motionState == MotionState.down)
      {
         if (request.direction == ElevatorStopRequest.RequestDirection.down)
         {
            // If we're moving down and a down request is sent from a stop above us, we add the request to a new queue
            if (request.requestedStop.stopTransform.position.y > elevatorTransform.position.y)
            {
               request.source.LinkToRequest(request);
               nextDownRequests.Enqueue(request);
            }
            else
            {
               request.source.LinkToRequest(request);
               requestedStops.Enqueue(request);
            }
         }
         if (request.direction == ElevatorStopRequest.RequestDirection.up)
         {
            request.source.LinkToRequest(request);
            nextUpRequests.Enqueue(request);
         }
      }
      return true;
   }
}
