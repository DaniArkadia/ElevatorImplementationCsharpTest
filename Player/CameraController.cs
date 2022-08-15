using UnityEngine;

/* This is something quick I threw together to enable interaction with the elevator, I tried not to focus
too much on the aspects of the code which weren't necessarily part of the test. As such in these cases I've left the commenting out.'*/
public class CameraController : MonoBehaviour
{  
   [SerializeField] float flySpeed;
   [SerializeField] float lookSpeed = 1f;

   [SerializeField] Transform outerCameraPos;
   [SerializeField] Transform elevatorCameraPos;

   [SerializeField] GameObject crosshair;

   InteractController InteractController;

   void Awake()
   {
      InteractController = GetComponent<InteractController>();
   }

   void Update()
   {
      if (Input.GetKeyDown(KeyCode.LeftShift))
      {
         MoveCameraToPosition(elevatorCameraPos);
         crosshair.SetActive(true);
         InteractController.SetPOVInteractionRay();
         Cursor.lockState = CursorLockMode.Locked;
         Cursor.visible = false;
      }
      if (Input.GetKeyUp(KeyCode.LeftShift))
      {
         MoveCameraToPosition(outerCameraPos);
         crosshair.SetActive(false);
         InteractController.SetDefaultInteractionRay();
         Cursor.lockState = CursorLockMode.None;
         Cursor.visible = true;
      }
      if (transform.parent == outerCameraPos)
      {
         transform.parent.position += transform.up * Input.GetAxis("Vertical") * flySpeed * Time.deltaTime; 
         transform.parent.position += transform.right * Input.GetAxis("Horizontal") * flySpeed * Time.deltaTime; 
      }
      else
      {
         transform.parent.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime, 0);
         transform.Rotate(-Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime, 0, 0);
      }
   }

   void MoveCameraToPosition(Transform positionTransform)
   {
      transform.position = positionTransform.position;
      transform.rotation = positionTransform.rotation;
      transform.parent = positionTransform;
   }
}
