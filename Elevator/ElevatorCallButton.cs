using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorCallButton : MonoBehaviour, IInteractable
{
   public bool isPressed { get; private set; }

   [SerializeField] protected int floorNumber;
   [SerializeField] protected GameObject buttonLight;
   [SerializeField] protected Elevator connectedElevator;
   [SerializeField] protected ElevatorStopRequest.RequestDirection requestDirection;

   protected ElevatorStop stop;
   MonoTimer toggleTimer;


   // If the button wasn't already pressed, sends a stop request to the connectElevator.
   public void OnInteract()
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

   /* The OnInteract method will most likely be the same for all ElevatorCallButtons therefore I chose to just use polymorphism 
   to the creation of the request in order to get the desired difference in behaviour amongst subclasses*/
   public virtual ElevatorStopRequest GenerateRequest()
   {
      return new ElevatorStopRequest(requestDirection, stop, this);
   }

   // This lets us link a callback to when our request is finished being handled.
   public void LinkToRequest(ElevatorStopRequest request)
   {
      request.onRequestFulfilled += OnRequestFulfilled;
   }

   protected void OnRequestFulfilled()
   {
      ToggleButton(false);
   }

   // Finds the corrosponding ElevatorStop from the connectedElevator using the floorNumber.
   void Awake()
   {
      OnAwake();
      stop = connectedElevator.GetConnectedStop(floorNumber);
   }

   // I provide an easy way to execute something within the Awake method while keeping the base code for getting the connected stop intact.
   protected virtual void OnAwake() { }

   // Toggles the buttons state.
   protected virtual void ToggleButton(bool toggle)
   {
      isPressed = toggle;
      buttonLight.SetActive(toggle);
   }
}
