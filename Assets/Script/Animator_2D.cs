using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Extend;
using UnityEngine;

namespace Player
{
    public class Animator_2D : MonoBehaviour
    {
        private List<int> lockState = new List<int>();
        private Transform body { get; set; }
        private bool stateLock => selfStateLock;
        protected int curState { get; private set; }

        private bool Enable => curState != -1;
        protected Animator animator;
        protected IAnimatorController AnimatorController;
        private bool selfStateLock;
        private bool isPause;

        private void Awake()
        {
            body = transform.Find("Body");
            animator = GetComponent<Animator>();
            AnimatorController = GetComponent<IAnimatorController>();
            curState = 0;


            StateDic();
        }

        protected virtual void StateDic()
        {

        }

        public void AddLockState(int state)
        {
            if (!lockState.Contains(state))
                lockState.Add(state);
        }

        protected virtual bool ForceChangeState(int newState)
        {
            return false;
        }


        public virtual void SetAnim(int state)
        {
            if (curState == state)
                return;

            //状态锁住 且无法强制改变状态
            if (stateLock && !ForceChangeState(state))
                return;
            ChangeAnimState(state);
            curState = state;
            CheckState();
            NeedLockAnim();
            WaitStateComplete();
        }


        protected virtual void ChangeAnimState(int state)
        {


        }

        private void NeedLockAnim()
        {
            foreach (var state in lockState.Where(state => state == curState))
            {
                LockState();
            }
        }

        protected virtual void CheckState()
        {

        }

        protected async void WaitStateComplete()
        {
            AnimatorController.AnimatorStateEnter();
            await Task.Delay(5);
            this.WaitExecute(Condition, Complete);
        }

        protected virtual void LockState()
        {
            selfStateLock = true;
        }

        protected virtual void UnLockState()
        {
            selfStateLock = false;

        }

        private bool Condition()
        {
            return animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f;
        }

        private void Complete()
        {
            UnLockState();
            AnimatorController.AnimatorStateComplete();
        }
    }
    
    public interface IAnimatorController
    {
        void AnimatorStateEnter();
        void AnimatorStateComplete();
    }
}

