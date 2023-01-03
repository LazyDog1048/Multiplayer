using System;
using System.Collections;
using System.Collections.Generic;
using GameThing;
using Mirror;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Network
{
    public class PigsNetworkManager : NetworkManager
    {
        public List<PlayerController> players;

        private void OnServerInitialized()
        {
            players = new List<PlayerController>();
        }

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            base.OnServerAddPlayer(conn);
            PlayerController player = conn.identity.GetComponent<PlayerController>();
            GameManager.Instance.AddPlayer(player);

            if(!SceneManager.GetActiveScene().name.Equals("OnlineScene"))
                return;
            players.Add(player);
            
            if(players.Count == maxConnections)
            {
                GameManager.Instance.StartGame();
                // players[0].opponent = players[1];
                // players[1].opponent = players[0];
            }
        }
    }
}
