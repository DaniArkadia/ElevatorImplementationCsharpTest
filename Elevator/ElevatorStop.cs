using System;
using UnityEngine;

/* This class encapsulates everything related to the an elevator stop, I chose to not make this a MonoBehaviour;
the main reason for this was that it made it easier to display and setup the list of ElevatorStops in the 
Elevator class's inspector. */

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
