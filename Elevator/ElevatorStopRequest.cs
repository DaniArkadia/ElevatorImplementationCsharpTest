using System;

/* I chose to encapsulate requests in this class as it made sense to me to pass around the requests as an entire object,
rather than passing a load of arguments around.*/
public class ElevatorStopRequest
{
   public enum RequestDirection { up, down, none };

   // Whether the client wishes to travel to a higher or lower level.
   public RequestDirection direction { get; private set; }
   public ElevatorStop requestedStop { get; private set; }
   public ElevatorCallButton source { get; private set; }

   // Buttons and whatever else can hook into this, it gets invoked when the request has been fulfilled.
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
