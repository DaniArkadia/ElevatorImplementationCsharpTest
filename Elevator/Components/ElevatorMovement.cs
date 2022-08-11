using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This component handles all the actual movement of the elevator, it's pretty dumb on it's own and doesn't know much about what's going out outside.
[System.Serializable]
public class ElevatorMovement
{
   [SerializeField] float speed = 5f;
   [SerializeField] float lerpSpeedFactor = 3f;
 

   // Moves elevator towards stop and returns true once it has reached there.
   public bool MoveToStop(Transform transform, ElevatorStop stop)
   {
      if (Vector3.Distance(transform.position, stop.stopTransform.position) > 5f)
      {
         transform.position = Vector3.MoveTowards(transform.position, stop.stopTransform.position, speed * Time.deltaTime);
         return false;
      }
      else if (Vector3.Distance(transform.position, stop.stopTransform.position) > 0.01f)
      {
         transform.position = Vector3.Lerp(transform.position, stop.stopTransform.position, lerpSpeedFactor * Time.deltaTime);
         return false;
      }
      return true;
   }
}
