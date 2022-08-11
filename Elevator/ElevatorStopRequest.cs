using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorStopRequest
{
   public enum RequestDirection { up, down };

   // Whether the client wishes to travel to a higher or lower level.
   public RequestDirection direction { get; private set; }
   public ElevatorStop requestedStop { get; private set; }
   public ElevatorCallButton source { get; private set; }

   public event Action onRequestFulfilled;

   public ElevatorStopRequest(RequestDirection direction, ElevatorStop requestedStop, ElevatorCallButton requestSource)
   {
      this.direction = direction;
      this.requestedStop = requestedStop;
      this.source = requestSource;
   }

   ~ElevatorStopRequest()
   {
      onRequestFulfilled = null;
   }

   public void OnFulfillRequest()
   {
      onRequestFulfilled?.Invoke();
   }

   /* Override the equals operator as any instance of this class with the 
      same member values should be considered a clone and therefore equal. */
   public override bool Equals(object obj)
   {
      var request = (ElevatorStopRequest)obj;
      return request.requestedStop == this.requestedStop && request.direction == direction;
   }

   public override int GetHashCode()
   {
      return HashCode.Combine(direction, requestedStop);
   }
}
