using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a useful class I made a while ago that I use in many of my projects.
public static class Timing
{
   static MonoTimer GetNewMonoTimer()
   {
      var instance = new GameObject("DelayTimer");
      instance.hideFlags = HideFlags.HideInHierarchy;
      var timer = instance.AddComponent<MonoTimer>();
      return timer;
   }
   public static MonoTimer DelayForSeconds(float seconds, Action OnFinished)
   {
      var timer = GetNewMonoTimer();
      timer.DelayForSeconds(seconds, OnFinished);
      return timer;
   }

   public static MonoTimer TickForSeconds(float seconds, Action tickBehaviour, Action onFinished = null)
   {
      var timer = GetNewMonoTimer();
      timer.TickForSeconds(seconds, tickBehaviour, onFinished);
      return timer;
   }

   
   public static MonoTimer TickUntil(Action tickBehaviour, Func<bool> condition, Action onFinished = null)
   {
      var timer = GetNewMonoTimer();
      timer.TickUntil(tickBehaviour, condition, onFinished);
      return timer;
   }

   public static MonoTimer ToggleForSeconds<T>(Action<T> setPreviousValue, T defaultValue, T toggleValue, float seconds)
   {
      var timer = GetNewMonoTimer();
      timer.ToggleForSeconds(setPreviousValue, defaultValue, toggleValue, seconds);
      return timer;
   }

   public static MonoTimer DelayUntil(Func<bool> condition, Action onFinished)
   {
      var timer = GetNewMonoTimer();
      timer.DelayUntil(condition, onFinished);
      return timer;
   }

   public static MonoTimer TryTriggerAfterDelay(float seconds, Func<bool> condition, Action onFinishedIfTrue, Action onFinishedIfFalse = null, bool isConditionConstant = false)
   {
      var timer = GetNewMonoTimer();
      timer.TryTriggerAfterBuffer(seconds, condition, onFinishedIfTrue, onFinishedIfFalse, isConditionConstant);
      return timer;
   }
}
