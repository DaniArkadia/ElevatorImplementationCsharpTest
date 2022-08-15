using UnityEngine;

/* This is something quick I threw together to enable interaction with the elevator, I tried not to focus
too much on the aspects of the code which weren't necessarily part of the test. As such in these cases I've left the commenting out.'*/
public class InteractController : MonoBehaviour
{
   Camera currentCamera;

   delegate bool InteractRayMethod(out RaycastHit hit);
   InteractRayMethod interactRayMethod;

   float delayBuffer;

   public void SetPOVInteractionRay() => interactRayMethod = InteractFromScreenCenter;
   public void SetDefaultInteractionRay() => interactRayMethod = InteractFromMousePosition;

   void Awake()
   {
      currentCamera = Camera.main;

      SetDefaultInteractionRay();
   }

   void FixedUpdate()
   {
      if (Input.GetMouseButtonDown(0))
      {
         RaycastHit hit;
         if (Physics.Raycast(currentCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, ~0, QueryTriggerInteraction.Collide))
         {
            IInteractable interactable;
            if (hit.transform.TryGetComponent<IInteractable>(out interactable))
            {
               interactable.OnInteract();
            }
         }
      }
   }

   bool InteractFromMousePosition(out RaycastHit hit)
   {
      return (Physics.Raycast(currentCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, ~0, QueryTriggerInteraction.Collide));
   }

   bool InteractFromScreenCenter(out RaycastHit hit)
   {
      return (Physics.Raycast(currentCamera.ScreenPointToRay(Input.mousePosition), out hit, 100, ~0, QueryTriggerInteraction.Collide));
   }
}
