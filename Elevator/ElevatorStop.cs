using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElevatorStop : ISerializationCallbackReceiver
{
   [HideInInspector] public string name;

   public Transform stopTransform;
   public event Action onStopReached;

   [SerializeField] int floorNumber;
   ElevatorDoors doorsOuter;

   public void Init()
   {
      doorsOuter = stopTransform.GetComponentInChildren<ElevatorDoors>();
   }
 
   public int GetFloorNumber()
   {
      return floorNumber;
   }

   public void OnStopReached(Action onDoorsOpen = null)
   {
      onStopReached?.Invoke();
      doorsOuter.Open(onDoorsOpen);
   }

   public void OnDoorsTimedOut()
   {
      doorsOuter.Close();
   }

   // This is to set the index of the floor so it appears as the title of the listed element in the elevator's inspector
   public void OnBeforeSerialize()
   {
      name = "Floor: " + floorNumber;
   }
   public void OnAfterDeserialize() { }
}
