using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCallButton : MonoBehaviour, IInteractable
{
   public bool isPressed { get; private set; }

   [SerializeField] int floorNumber;
   [SerializeField] GameObject buttonLight;
   [SerializeField] Elevator connectedElevator;
   [SerializeField] ElevatorStopRequest.RequestDirection requestDirection;

   MonoTimer toggleTimer;

   ElevatorStop stop;

   // If the button wasn't already pressed, sends a stop request to the connectElevator.
   public void OnInteract()
   {
      if (isPressed) return;

      if (connectedElevator.TryRequestStop(new ElevatorStopRequest(requestDirection, stop, this)))
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

   // This lets us link a callback to when our request is finished being handled.
   internal void LinkToRequest(ElevatorStopRequest request)
   {
      request.onRequestFulfilled += OnRequestFulfilled;
   }

   internal void OnRequestFulfilled()
   {
      ToggleButton(false);
   }

   // Finds the corrosponding ElevatorStop from the connectedElevator using the floorNumber.
   void Awake()
   {
      stop = connectedElevator.GetConnectedStop(floorNumber);
   }

   // Toggles the buttons state.
   void ToggleButton(bool toggle)
   {
      isPressed = toggle;
      buttonLight.SetActive(toggle);
   }
}
