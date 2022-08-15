using UnityEngine;

/* The base class for all buttons connected to the Elevator class, this could be potentially abstracted out further to be the base
for any interactive world buttons, but for this example it makes sense like this. */
public abstract class ElevatorButton : MonoBehaviour, IInteractable
{
   [SerializeField] protected Elevator connectedElevator;
   [SerializeField] protected GameObject buttonLight;
   protected bool isPressed;

   protected MonoTimer toggleTimer;
   public abstract void OnInteract();

   // Toggles the buttons state.
   protected virtual void ToggleButton(bool value)
   {
      isPressed = value;
      buttonLight.SetActive(value);
   }
   
   // This is called when the request that is linked to this button has been fullfilled.
   protected virtual void OnRequestFulfilled()
   {
      ToggleButton(false);
   }

}
