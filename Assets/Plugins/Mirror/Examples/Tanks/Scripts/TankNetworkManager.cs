using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Mirror.Examples.Tanks
{
    public class TankNetworkManager : NetworkManager
    {
        private void OnConnectedToServer()
        {
            Debug.Log($"OnConnectedToServer  {networkAddress}");
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();
            Debug.Log($"OnClientConnect  {networkAddress}");
        }
    }
    
}
