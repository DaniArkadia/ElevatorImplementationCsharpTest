using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractController : MonoBehaviour
{
   Camera currentCamera;

   void Awake()
   {
      currentCamera = Camera.main;
   }

   void Update()
   {
      if (Input.GetMouseButton(0))
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
}
