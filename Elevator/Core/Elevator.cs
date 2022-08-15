using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* The Elevator class is comprised of two important components, the movement component simply handles the physics of moving between floors,
the ElevatorOperator handles all stop requests recieved by the elevator, it can be easily subclassed for all kinds of fancy elevator traversal methods
the Elevator class itself deals with the (mostly) higher level scripting, like opening doors, waiting for doors to close, etc... I've chosen to seal
this class as I've tried to make this extensible using the mentioned components so this is my way of communicating that to whoever might end up using it. */
[SelectionBase]
public sealed class Elevator : MonoBehaviour
{
   /* All elevator stops associated with this elevator are stored here, originally they represented by MonoBehaviours
   attached to GameObjects representing the position of each stop, but I feel like serializing them directly 
   in this object will make designers lives easier, instead of them needing to jump around the hierarchy. */
   [SerializeField] List<ElevatorStop> availableStops;

   // C# Components
   [SerializeField] ElevatorMovement elevatorMovement;
   
   [Header("Sound")]
   [SerializeField] AudioClip music;
   [SerializeField] AudioClip floorReachedSound;

   ElevatorOperator elevatorOperator; 

   // The doors attached to the elevator.
   ElevatorDoors doors;
   AudioSource speaker;

   ElevatorStop previousStop;
   ElevatorStop currentStop;

   MonoTimer doorCloseTimer;
   bool isBusy;


   // Returns the ElevatorStop associated with the given floorNumber from our availableStops.
   public ElevatorStop GetConnectedStop(int floorNumber)
   {
      try
      {
         return availableStops.Where((s) => s.GetFloorNumber() == floorNumber).First();
      }
      catch (System.Exception)
      {
         throw new Exception("stop with floorNumber: " + floorNumber + " doesn't exist.");
      }
   }

   /* Called by elevator call buttons to request a stop, in this implementation it will only handle the request.
   if a comparable request doesn't already exist in the queue. */
   public bool TryRequestStop(ElevatorStopRequest request)
   {
      // If we call the elevator from the floor it is already at.
      if (currentStop == request.requestedStop)
      {
         if (!isBusy)
         {
            OnOpenSameFloor();
         }
         return false;
      }
      return elevatorOperator.TryHandleElevatorRequest(request);
   }

   /* Called by certain ElevatorDoorButtons, the elevator is moving we ignore this request, and we check that the doors
   Are actually closed before we try to open them. */
   public bool TryRequestOpenDoors()
   {
      if (elevatorOperator.HasActiveRequests()) return false;
      if (doors.AreFullyClosed())
      {
         OnOpenSameFloor();
         return true;
      }
      return false;
   }

   /* Similar to the method above but handles closing instead, these could be squashed into one method to reduce repetition,
   but in this case I feel like keeping them seperated makes way more sense for readability.*/
   public bool TryRequestCloseDoors()
   {
      if (doors.AreFullyOpen() && currentStop != null)
      {
         ForceCloseDoors();
         return true;
      }
      return false;
   }
   
   void ForceCloseDoors()
   {
      isBusy = true;
      if (doorCloseTimer != null)
      {
         doorCloseTimer.Cancel();
      }
      currentStop.OnDoorsTimedOut();
      doors.Close(() => isBusy = false);
   }

   void Update()
   {
      // Is busy is a flag that is true when the elevator doors are open, this stops the elevator from moving prematurely.
      if (isBusy) return;
      
      /* If we have any requests queued, move the elevator towards the requested stop,
      once we get there we call the OnReachStop method */

      if (elevatorOperator.HasActiveRequests())
      {
         if (currentStop != null)
         {
            previousStop = currentStop;
            currentStop = null;
         }
         var targetRequest = elevatorOperator.GetStopToMoveTo();
         if (elevatorMovement.MoveToStop(transform, targetRequest.requestedStop))
         {
            OnReachedStop();
         }
      }
   }

   /* Sets isBusy to true, as we are about to open the doors of the elevator. Removes the previous request as we 
   have now satisfied it, we open both the doors of the last stop we reached, and the elevator doors (there are
   two sets of doors because "Safety First!" :P). We also pass in a delayed callback to the OnWaitingTimedOut method
   which will be triggered after a 5 seconds delay.*/
   void OnReachedStop()
   {
      isBusy = true;
      var finishedRequest = elevatorOperator.OnCleanUpRequest();
      finishedRequest.OnFulfillRequest();
      currentStop = finishedRequest.requestedStop;
      currentStop.OnStopReached();
      doors.Open(() => doorCloseTimer = Timing.DelayForSeconds(5f, OnWaitingTimedOut));
      speaker.PlayOneShot(floorReachedSound);
   }

   // Opens the doors, sets the isBusy flag to true, then closes the doors and sets the isBusy flag back to false.
   void OnOpenSameFloor()
   {
      isBusy = true;
      currentStop.OnStopReached();

      doors.Open(() => doorCloseTimer = Timing.DelayForSeconds(5f, () => 
      {
         currentStop.OnDoorsTimedOut();
         doors.Close(() => isBusy = false);
      }));
   }

   /* Called when the doors are ready to close again after they have been open for a predetermined amount of time.
   Once the doors are fully closed we set isBusy to false so that the elevator will be able to move again. */
   void OnWaitingTimedOut()
   {
      if (currentStop != null)
      {
         currentStop.OnDoorsTimedOut();
      }
      doors.Close(() => isBusy = false);
   }

   void Awake()
   {
      elevatorOperator = new ElevatorOperator(transform);
      doors = GetComponentInChildren<ElevatorDoors>();
      speaker = GetComponentInChildren<AudioSource>();
      speaker.clip = music;
      speaker.Play();
      availableStops.ForEach((s) => s.Init());
   }

}
