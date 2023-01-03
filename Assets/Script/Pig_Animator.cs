using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Player
{
    public enum PigState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Attack,
        Hit,
        Dead,
        Ground
    }
    public class Pig_Animator : Animator_2D
    {
        private PigState curState;
        public PigState CurState 
        {
            get =>curState;
            set => ChangeState(value);
        }
        
        private Dictionary<PigState, int> pigStateDic;

        protected override void StateDic()
        {
            pigStateDic = new Dictionary<PigState, int>()
            {
                { PigState.Idle, AnimaStateHash.state_Idle },
                { PigState.Run, AnimaStateHash.state_Run },
                { PigState.Jump, AnimaStateHash.state_Jump },
                { PigState.Fall, AnimaStateHash.state_Fall },
                { PigState.Attack, AnimaStateHash.state_Attack },
                { PigState.Hit, AnimaStateHash.state_Hit },
                { PigState.Dead, AnimaStateHash.state_Dead },
                { PigState.Ground, AnimaStateHash.state_Ground }
            };
            curState = PigState.Idle;
            AddLockState((int)PigState.Attack);
            AddLockState((int)PigState.Hit);
            AddLockState((int)PigState.Dead);
        }
        
        public void ChangeState(PigState state)
        {
            base.SetAnim((int)state);
        }

        protected override void ChangeAnimState(int state)
        {
            curState = (PigState)state;
            animator.Play(pigStateDic[curState],0,0);
        }
    }
    
}
