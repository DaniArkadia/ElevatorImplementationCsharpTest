using UnityEngine;

/* This is the base class for all ElevatorButtons that actively request a change in stop.*/
public abstract class ElevatorCallButton : ElevatorButton
{
   [SerializeField] protected int floorNumber;
   protected ElevatorStop stop;


   // If the button wasn't already pressed, sends a stop request to the connectElevator.
   public override void OnInteract()
   {
      if (isPressed) return;
      var request = GenerateRequest();
      if (connectedElevator.TryRequestStop(request))
      {
         if (toggleTimer != null)
         {
            toggleTimer.Cancel();
         }
         ToggleButton(true);
      }
      else
      {
         // If a request is refused we turn the light on and toggle the buttons state for better visual feedback.
         toggleTimer = Timing.ToggleForSeconds(ToggleButton, false, true, 0.1f);
      }
   }
   public abstract ElevatorStopRequest GenerateRequest();

   // This lets us link a callback to when our request is finished being handled.
   public void LinkToRequest(ElevatorStopRequest request)
   {
      request.onRequestFulfilled += OnRequestFulfilled;
   }

   // Toggles the buttons state.
   protected override void ToggleButton(bool toggle)
   {
      isPressed = toggle;
      buttonLight.SetActive(toggle);
   }

   void Awake() 
   { 
      stop = connectedElevator.GetConnectedStop(floorNumber);
   }
}
