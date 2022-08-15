using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

/* This mono has a simple way for the elevator doors to open and close and invoke callbacks at the end of each task
I made this for convenience and isn't necessarily how I would handle this normally, I did it this way as I know 
the focus of this shouldn't be on the unity specific stuff. */
public class ElevatorDoors : MonoBehaviour
{

   [SerializeField] PlayableAsset openTimeline;
   [SerializeField] PlayableAsset closeTimeline;

   PlayableDirector director;
   bool isBusy;

   public void Open(Action onFinishedCallback = null)
   {
      if (isBusy) return;

      StartCoroutine(PlayAnimation(openTimeline, onFinishedCallback));
   }

   public void Close(Action onFinishedCallback = null)
   {
      StartCoroutine(PlayAnimation(closeTimeline, onFinishedCallback));
   }

   void Awake()
   {
      director = GetComponent<PlayableDirector>();
   }

   IEnumerator PlayAnimation(PlayableAsset timeline, Action onFinishedCallback)
   {
      isBusy = true;

      director.playableAsset = timeline;
      director.Play();

      while (director.time <= director.duration - 0.1f)
      {
         yield return null;
      }
      isBusy = false;
      onFinishedCallback?.Invoke();
      yield return null;
   }

   internal bool AreFullyClosed()
   {
      return director.playableAsset == closeTimeline && !isBusy;
   }

   internal bool AreFullyOpen()
   {
      return director.playableAsset == openTimeline && !isBusy;
   }
}
