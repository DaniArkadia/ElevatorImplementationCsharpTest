using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ElevatorStop : ISerializationCallbackReceiver
{
   [HideInInspector] public string name;
   public enum DoorDirection { front, rear }

   public Transform stopTransform;
   public event Action onStopReached;

   [SerializeField] int floorNumber;
   [SerializeField] DoorDirection doorDirection;
   [SerializeField] ElevatorDoors doorsOuter;

   public int GetFloorNumber()
   {
      return floorNumber;
   }
   public DoorDirection GetDoorDirection()
   {
      return doorDirection;
   }

   public void OnStopReached(Action onDoorsOpen = null)
   {
      onStopReached?.Invoke();
      doorsOuter.Open(onDoorsOpen);
   }

   internal void OnDoorsTimedOut()
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
