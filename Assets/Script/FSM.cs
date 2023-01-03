using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Player
{
    public class FSM : NetworkBehaviour
    {
        protected virtual void Initialized(){}
        protected virtual void FSMUpdate(){}
        protected virtual void FSMFixedUpdate(){}

        
        
        void Start()
        {
            Initialized();    
        }

        void Update()
        {
            if(!isLocalPlayer)
                return;
            FSMUpdate();
        }
        
        void FixedUpdate()
        {
            if(!isLocalPlayer)
                return;
            FSMFixedUpdate();
        }
    }
    
}
