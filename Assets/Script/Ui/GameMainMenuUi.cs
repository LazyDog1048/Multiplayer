using System.Collections;
using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

namespace GameThing
{
    public class GameMainMenuUi : MonoBehaviour
    {
        public Button startButton;
        public Button joinButton;
        [SerializeField]
        private bool useSteam = false;
        
        protected Callback<LobbyCreated_t> lobbyCreated;
        protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;
        protected Callback<LobbyEnter_t> lobbyEntered;

        private void Start()
        {
            startButton.onClick.AddListener(StartLobby);
            joinButton.onClick.AddListener(JoinGame);
            
            if(!useSteam)
                return;
            lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
            gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
            lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
        }
        
        private void StartLobby()
        {
            if (useSteam)
            {
                SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly,
                    NetworkManager.singleton.maxConnections);
                return;
            }
            NetworkManager.singleton.StartHost();
            Debug.Log("Start Game");
        }
        
        private void JoinGame()
        {
            Debug.Log("Join Game");
            NetworkManager.singleton.StartClient();
        }
        
        private void OnLobbyCreated(LobbyCreated_t callback)
        {
            if (callback.m_eResult != EResult.k_EResultOK)
            {
                Debug.Log("Failed to create lobby");
                return;
            }
            Debug.Log("Lobby created");
            NetworkManager.singleton.StartHost();
            SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby),"HostAddress",SteamUser.GetSteamID().ToString());
            // SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress", SteamUser.GetSteamID().ToString());
        }
        
        private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
        {
            Debug.Log("Joining lobby");
            SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
        }
        
        private void OnLobbyEntered(LobbyEnter_t callback)
        {
            if(NetworkServer.active)
                return;

            Debug.Log("Entered lobby");
            string hostAddress = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAddress");
            NetworkManager.singleton.networkAddress = hostAddress;
            NetworkManager.singleton.StartClient();
        }
    }
    
}
