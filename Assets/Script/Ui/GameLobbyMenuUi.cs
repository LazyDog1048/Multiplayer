using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace GameThing
{
    public class GameLobbyMenuUi : MonoBehaviour
    {
        public Button startButton;
        public Button exitButton;
        
        private void Start()
        {
            if (NetworkServer.active)
            {
                startButton.gameObject.SetActive(true);
                startButton.onClick.AddListener(StartGame);
            }
            else
            {
                startButton.gameObject.SetActive(false);
            }
            exitButton.onClick.AddListener(ExitLobby);
        }
        
        private void StartGame()
        {
            NetworkManager.singleton.ServerChangeScene("OnlineScene");
        }
        
        private void ExitLobby()
        {
            if(NetworkServer.active && NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopHost();
            }
            else if(NetworkClient.isConnected)
            {
                NetworkManager.singleton.StopClient();
            }
        }
    }
    
}
