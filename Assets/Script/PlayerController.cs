using System.Collections;
using System.Collections.Generic;
using Extend;
using GameThing;
using Mirror;
using Steamworks;
using UnityEngine;

namespace Player
{
    public class PlayerController : FSM,IAnimatorController
    {
        [SerializeField]
        private PlayerUi playerUi;

        [Header("Properties")]
        // [SyncVar(hook =  nameof(OnNameChange))]
        public string playerName;
        public int maxHp;
        public float speed = 5f;
        [Header("AttackThings")] 
        public int damage = 10;
        public float attackRadius = 1f;
        public LayerMask enemyLayer;

        [SyncVar(hook =  nameof(OnHealthChanged))]
        private int currentHp;
        
        
        private float horizontal = 0f;
        private int lookDirection = 1;
        private PigState finalState;
        private Pig_Animator pigAnimator;
        private Rigidbody2D rb2D;
        private Transform body;
        private Transform atkPoint;
        
        
        protected override void Initialized()
        {
            pigAnimator = GetComponent<Pig_Animator>();
            rb2D = GetComponent<Rigidbody2D>();
            body = transform.Find("Body");
            atkPoint = transform.Find("AtkPoint");
            currentHp = maxHp;    
            playerUi.Init(maxHp);
            this.DelayExecute(2,()=>
            {
                pigAnimator.CurState = PigState.Idle;
            });
            PlayerData data = GameManager.Instance.GetPlayerData(netId);
            if(isLocalPlayer)
                data.username = SteamFriends.GetPersonaName();;
            // playerData.username = personName;
            playerName = data.username;
            playerUi.UpdateName(playerName);
        }

        public override void OnStartLocalPlayer()
        {
            GameManager.SetLocalPlayer(this);
        }

        #region hook
        private void OnNameChange(string oldName, string newName)
        {
            playerUi.UpdateName(newName);
            // playerUi.SetName(newName);
        }
        
        private void OnHealthChanged(int oldHp, int newHp)
        {
            playerUi.UpdateHpBar(newHp);
        }
        #endregion
        #region cmd
        [Command]
        public void CmdOnTakeDamage(int damage)
        {
            currentHp -= damage;
            Debug.Log("take");
            Debug.Log($"{name}   {currentHp}");
            if (currentHp <= 0)
                UpdateDeadState();
        }

        [Command(requiresAuthority = false)]
        private void CmdHitEnemy(PlayerController target,int damage)
        {
            Debug.Log("hit");
            target.CmdOnTakeDamage(damage);
        }

        [Command]
        private void CmdDeadPlayer()
        {
            GameManager.Instance.RemovePlayerName(this);
        }
        #endregion
        protected override void FSMUpdate()
        {
            if(GameManager.Instance.gameState != GameManager.GameState.GameStart)
                return;
            if(pigAnimator.CurState == PigState.Dead)
                return;
            StateUpdate();
            AnimStateUpdate();
        }

        private void DirectionUpdate()
        {
            horizontal = Input.GetAxis("Horizontal");
            if(horizontal>0)
                lookDirection = -1;
            else if(horizontal<0)
                lookDirection = 1;
            body.localScale = new Vector3(lookDirection,1,1);
        }
        private void StateUpdate()
        {
            DirectionUpdate();
            UpdateIdleState();
            if (Input.GetKeyDown(KeyCode.J))
            {
                finalState = PigState.Attack;
            }
        }
        private void AnimStateUpdate()
        {
            pigAnimator.CurState = finalState;
            switch (finalState)
            {
                case PigState.Idle:
                    break;
                case PigState.Run:
                    UpdateMoveState();
                    break;
                case PigState.Attack:
                    UpdateAtkState();
                    break;
            }
        }
        
        private void UpdateIdleState()
        {
            // finalState = Mathf.Abs(horizontal) > Mathf.Epsilon ? PigState.Idle : PigState.Run;
            finalState = horizontal != 0 ? PigState.Run : PigState.Idle;
        }

        private void UpdateAtkState()
        {
            Collider2D[] enemies = Physics2D.OverlapCircleAll(atkPoint.position, attackRadius, enemyLayer);

            // if(enemies== null)
            //     return;
            foreach (var enemy in enemies)
            {
                PlayerController playerController = enemy.GetComponentInParent<PlayerController>();
                if (playerController.netId != this.netId)
                {
                    CmdHitEnemy(playerController,damage);
                }
            }
        }
        private void UpdateMoveState()
        {
            rb2D.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb2D.velocity.y);
        }
        
        private void UpdateDeadState()
        {
            // finalState = Mathf.Abs(horizontal) > Mathf.Epsilon ? PigState.Idle : PigState.Run;
            finalState = PigState.Dead;
            CmdDeadPlayer();
            AnimStateUpdate();
        }

        public void AnimatorStateEnter()
        {
            
        }

        public void AnimatorStateComplete()
        {
            
        }
    }
    
}
