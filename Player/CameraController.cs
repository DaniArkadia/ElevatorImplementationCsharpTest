using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Just threw this together quickly to let you pan around at will, made quickly for convenience as I know this
isn't the focus of the task at hand */
public class CameraController : MonoBehaviour
{
   [SerializeField] float flySpeed;
   void Update()
   {
      transform.position += transform.up * Input.GetAxis("Vertical") * flySpeed * Time.deltaTime; 
      transform.position += transform.right * Input.GetAxis("Horizontal") * flySpeed * Time.deltaTime; 
   }
}
