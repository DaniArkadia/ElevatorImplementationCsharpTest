using TMPro;
using UnityEngine;

public class InnerElevatorCallButton : ElevatorCallButton
{
   /* If enabled, the user will be able to manually set the text of the TextMesh, 
   otherwise it will be automatically set to the floorNumber */
   [SerializeField] bool hasCustomLabel;
   [SerializeField] Color onLabelColor = new Color(1,0.87f,0,1);
   
   Color defaultLabelColor;
   
   TextMeshPro label;

   /* Inner buttons aren't assigned a specific direction like the buttons on the outside,
   however we can infer direction relative to the elevators position later on if we need to */
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
  void Awake()
   {  
      label = GetComponentInChildren<TextMeshPro>();

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
      stop = connectedElevator.GetConnectedStop(floorNumber);
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
   }

}
