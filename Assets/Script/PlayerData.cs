using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Player
{
    [Serializable]
    public class PlayerData
    {
        public uint netId;
        public string username;

        public PlayerData()
        {
            
        }
        public PlayerData(uint _netId)
        {
            netId = _netId;
        }
    }
    
}
