using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public static class AnimaStateHash
    {
        public static readonly int state_Idle = Animator.StringToHash("Idle");
        public static readonly int state_Jump = Animator.StringToHash("Jump");
        public static readonly int state_Run = Animator.StringToHash("Run");
        public static readonly int state_Attack = Animator.StringToHash("Attack");
        public static readonly int state_Dead = Animator.StringToHash("Dead");
        public static readonly int state_Hit = Animator.StringToHash("Hit");
        public static readonly int state_Ground = Animator.StringToHash("Ground");
        public static readonly int state_Fall = Animator.StringToHash("Fall");
    }
    
}
