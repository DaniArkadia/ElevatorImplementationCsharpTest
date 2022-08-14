using TMPro;
using UnityEngine;


/* Sometimes it's a hard call on whether to use the principles of oop or whether to do something simpler like evaluating 
a bool in order to switch between desired behaviour. Sometimes I prefer a simple flag in order to reduce overall complexity,
however in this case I felt that the InnerElevatorButton was conceptually different enough to warrant it's own subclass.*/
public class InnerElevatorCallButton : ElevatorCallButton, IInteractable
{
   /* If enabled, the user will be able to manually set the text of the TextMesh, 
   otherwise it will be automatically set to the floorNumber */
   [SerializeField] bool hasCustomLabel;

   [SerializeField] Color onLabelColor = new Color(1,0.87f,0,1);
   Color defaultLabelColor;
   
   TextMeshPro label;

   /* Inner buttons aren't assigned a specific direction like the buttons on the outside,
   however we can infere direction relative to the elevators position later on if we need to */
   public override ElevatorStopRequest GenerateRequest()
   {
      var request = new ElevatorStopRequest(ElevatorStopRequest.RequestDirection.none, stop, this);
      return request;
   }

   protected override void ToggleButton(bool value)
   {
      base.ToggleButton(value);
      label.color = value == true ? onLabelColor : defaultLabelColor;
   }

   /* Since an Inner Button should be a child of a gameObject with the Elevator component, we can save the designers some time by
   finding the Elevator component ourselves, they could still manually assign a different Elevator if they so choose though. */
   protected override void OnAwake()
   {
      defaultLabelColor = label.color;
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
   void OnValidate()
   {
      if (label == null)
      {
         label = GetComponentInChildren<TextMeshPro>();
      }
      if (!hasCustomLabel)
      {
         if (label.text != floorNumber.ToString())
         {
            label.text = floorNumber.ToString();
         }
      }
      /* Unfortunately, fields with the [SerializeField] attribute are still displayed in the inspectors of subclassed MonoBehaviours
      even if they are marked as private, so I chose to mark them as protected and just reset the value if the user tries to change
      it in the inspector. */
      if (requestDirection == ElevatorStopRequest.RequestDirection.none) return;

      Debug.LogWarning("Inner elevator buttons don't need to be provided with a direction.");
      requestDirection = ElevatorStopRequest.RequestDirection.none;
   }

}
