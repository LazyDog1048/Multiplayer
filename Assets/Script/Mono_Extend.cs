using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Extend
{
    public static class Mono_Extend
    {
        public static void WaitExecute(this MonoBehaviour mono,Func<bool> completeCondition,UnityAction complete)
        {
            mono.WaitExecute(completeCondition, complete,null);
        }
        
        public static void WaitExecute(this MonoBehaviour mono,Func<bool> completeCondition,UnityAction complete,UnityAction Update)
        {
            if(mono.isActiveAndEnabled)
                mono.StartCoroutine(Delay());

            IEnumerator Delay()
            {
                while(!completeCondition())
                {
                    Update?.Invoke();
                    yield return 0;
                }
                complete?.Invoke();
            }
        }
        
        public static void DelayExecute(this MonoBehaviour mono,float time,UnityAction action)
        {
            if(mono.isActiveAndEnabled)
                mono.StartCoroutine(Delay());
            else
            {
                // Debug.LogError($"{mono.name}  is Not Idle");
            }
        
            IEnumerator Delay()
            {
                yield return new WaitForSeconds(time);
                if(mono.isActiveAndEnabled)
                    action.Invoke();
            }
        }

        /// <summary>
        /// 等待条件完成
        /// </summary>
        /// <param name="mono"></param>
        /// <param name="interval"></param>
        /// <param name="times"></param>
        /// <param name="action"></param>
        public static Coroutine DelayLoopExecute(this MonoBehaviour mono,float interval,int times,UnityAction<int> action)
        {
            if (interval < 0.01f)
                interval = 0.01f;
            return mono.StartCoroutine(DelayLoop());
            IEnumerator DelayLoop()
            {
                for (int i = 0; i < times; i++)
                {
                    yield return new WaitForSeconds(interval);
                    if(mono.isActiveAndEnabled)
                        action.Invoke(i);
                    if (times == 99)
                        i--;
                }
            }
        }
    }

}
