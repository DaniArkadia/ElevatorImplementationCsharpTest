using UnityEngine;

/* This button can either open the elevator doors or close them depending on the DoorRequestType, this was one of those moments
when I felt the temptation to split this into two classes, as switching on an enum like this isn't the most Oop way to do things,
and maybe if the class got bigger in the future I would do that, but as it stands I think the reduced complexity this way affords 
is more beneficial. */
public class ElevatorDoorsButton : ElevatorButton
{
   enum DoorRequestType { open, close, }
   [SerializeField] DoorRequestType requestType;


   public override void OnInteract()
   {
      if (requestType == DoorRequestType.open)
      {
         if (connectedElevator.TryRequestOpenDoors())
         {
            TryToggleButtonForSeconds(1f);
            return;
         }
         TryToggleButtonForSeconds(0.05f);
      }
      else if (requestType == DoorRequestType.close)
      {
         if (connectedElevator.TryRequestCloseDoors())
         {
            TryToggleButtonForSeconds(1f);
            return;
         }
         TryToggleButtonForSeconds(0.05f);
      }
   }

   protected override void ToggleButton(bool value)
   {
      buttonLight.SetActive(value);
   }

   /* Since an Inner Button should be a child of a gameObject with the Elevator component, we can save the designers some time by
   finding the Elevator component ourselves, they could still manually assign a different Elevator if they so choose though. */
   void Awake()
   {
      if (connectedElevator == null)
      {
         try
         {
            connectedElevator = gameObject.GetComponentInParent<Elevator>();
         }
         catch (System.Exception)
         {
            throw new System.Exception("InnerElevatorCallButton attached to " + transform.name + " couldn't find parent with Elevator component.");
         }
      }
   }

   // Toggles the button's state.
   void TryToggleButtonForSeconds(float seconds)
   {
      if (toggleTimer == null)
      {
         ToggleButton(true);
         toggleTimer = Timing.DelayForSeconds(seconds, () => ToggleButton(false));
      }
   }
}
