using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a useful class I made a while ago that I use in many of my projects.
public class MonoTimer : MonoBehaviour
{
   Action onTimerCancelled;
   public void DelayForSeconds(float seconds, Action onFinished)
   {
      StartCoroutine(DelayForSecondsCO(seconds, onFinished));
   }

   IEnumerator DelayForSecondsCO(float seconds, Action onFinished)
   {
      var elapsedTime = 0f;
      while (elapsedTime < seconds)
      {
         elapsedTime += Time.deltaTime;
         yield return null;
      }
      onFinished?.Invoke();
      Destroy(gameObject, 1f);
      yield return null;
   }

   public void DelayUntil(Func<bool> condition, Action onFinished)
   {
      StartCoroutine(DelayUntilCO(condition, onFinished));
   }

   IEnumerator DelayUntilCO(Func<bool> condition, Action onFinished)
   {
      while (condition?.Invoke() == false)
      {
         yield return null;
      }
      onFinished?.Invoke();
      Destroy(gameObject, 1f);
      yield return null;
   }

   public void ToggleForSeconds<T>(Action<T> onToggleValue, T defaultValue, T toggleValue, float seconds)
   {
      StartCoroutine(ToggleForSecondsCO<T>(onToggleValue, defaultValue, toggleValue, seconds));
   }

   IEnumerator ToggleForSecondsCO<T>(Action<T> onToggleValue, T defaultValue, T toggleValue, float seconds)
   {
      onTimerCancelled += () => onToggleValue(defaultValue);
      onToggleValue?.Invoke(toggleValue);

      var elapsedTime = 0f;
      while (elapsedTime < seconds)
      {
         elapsedTime += Time.deltaTime;
         yield return null;
      }  
      onToggleValue?.Invoke(defaultValue);
      onTimerCancelled = null;
      yield return null;
   }

   public void TickForSeconds(float seconds, Action tickBehaviour, Action onFinished = null)
   {
      StartCoroutine(TickForSecondsCO(seconds, tickBehaviour, onFinished));
   }
   IEnumerator TickForSecondsCO(float seconds, Action tickBehaviour, Action onFinished = null)
   {
      var elapsedTime = 0f;
      while (elapsedTime < seconds)
      {
         tickBehaviour?.Invoke();
         elapsedTime += Time.deltaTime;
         yield return null;
      }
      onFinished?.Invoke();
      Destroy(gameObject, 1f);
      yield return null;
   }

   public void TickUntil(Action tickBehaviour, Func<bool> condition, Action onFinished = null)
   {
      StartCoroutine(TickUntilCO(tickBehaviour, condition, onFinished));
   }

   IEnumerator TickUntilCO(Action tickBehaviour, Func<bool> condition, Action onFinished = null)
   {
      while (condition?.Invoke() == false)
      {
         tickBehaviour?.Invoke();
         yield return null;
      }
      onFinished?.Invoke();
      Destroy(gameObject, 1f);
      yield return null;
   }

   public void TryTriggerAfterBuffer(float seconds, Func<bool> condition, Action onFinishedIfTrue, Action onFinishedIfFalse, bool isConditionConstant = false)
   {
      StartCoroutine(TryTriggerAfterBufferCO(seconds, condition, onFinishedIfTrue, isConditionConstant, () => Cancel(), onFinishedIfFalse));
   }

   IEnumerator TryTriggerAfterBufferCO(float seconds, Func<bool> condition, Action onFinished, bool isConditionConstant, Action onConditionBroken, Action onFinishedIfFalse)
   {
      var elapsedTime = 0f;
      while (elapsedTime < seconds)
      {
         if (isConditionConstant)
         {
            if (condition?.Invoke() == false)
            {
               onConditionBroken?.Invoke();
            }
         }
         elapsedTime += Time.deltaTime;
         yield return null;
      }
      if (condition?.Invoke() == true) 
      { 
         onFinished?.Invoke(); 
      }
      else
      {
         if (onFinishedIfFalse != null)
         {
            onFinishedIfFalse?.Invoke();
         }
      }
      Destroy(gameObject, 1f);
      yield return null;
   }


   public void Cancel()
   {
      StopAllCoroutines();
      onTimerCancelled?.Invoke();
      onTimerCancelled = null;
      Destroy(gameObject, 1f);
   }
}
