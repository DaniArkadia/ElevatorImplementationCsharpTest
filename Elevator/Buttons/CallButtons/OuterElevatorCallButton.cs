using UnityEngine;

public class OuterElevatorCallButton : ElevatorCallButton
{
   [SerializeField] protected ElevatorStopRequest.RequestDirection requestDirection;

   public override ElevatorStopRequest GenerateRequest()
   {
      var request = new ElevatorStopRequest(requestDirection, stop, this);
      return request;
   }

}
