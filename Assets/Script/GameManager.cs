using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using Player;
using Steamworks;
using UnityEngine;

namespace GameThing
{
    public class GameManager : NetworkBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance => instance;
        public static bool IsEnable => instance != null && instance.localPlayer != null;
        public SyncDictionary<uint, PlayerData> playerDatas = new SyncDictionary<uint, PlayerData>();
        public enum GameState
        {
            None,
            GameStart,
            GamePause,
            GameResume,
            GameOver
        }
        
        public GameState gameState = GameState.None;
        
        [HideInInspector]
        public PlayerController localPlayer;

        public GameUi gameUi;
        private void Awake()
        {
            instance = this;
            // playerDatas = new SyncDictionary<uint, PlayerData>();
        }
        
        public static void SetLocalPlayer(PlayerController player)
        {
            if(Instance.localPlayer != null)
                return;
            Instance.localPlayer = player;
            
        }
        
        public void AddPlayer(PlayerController player)
        {
            if(playerDatas.ContainsKey(player.netId))
                return;
            PlayerData playerData = new PlayerData(player.netId);
            var steamOwner = SteamApps.GetAppOwner();
            playerData.username = steamOwner.ToString();
            playerDatas.Add(player.netId, playerData);
        }
        
        public void RemovePlayerName(PlayerController player)
        {
            if(!playerDatas.ContainsKey(player.netId))
                return;
            playerDatas.Remove(player.netId);
            CheckGameOver();
        }

        public PlayerData GetPlayerData(uint netId)
        {
            Debug.Log("getplayer");
            if(!playerDatas.ContainsKey(netId))
                return null;
            return playerDatas[netId];
        }
        
        private void CheckGameOver()
        {
            Debug.Log(playerDatas.Count);
            if (playerDatas.Count == 1)
            {
                CmdGameOver();
            }
        }
        
        [Server]
        public void StartGame()
        {
            gameState = GameState.GameStart;
            RpcStartGame();
        }
        
        [ClientRpc]
        private void RpcStartGame()
        {
            gameUi.OnStartGame();
        }

        [Server]
        private void CmdGameOver()
        {
            gameState = GameState.GameOver;
            ShowWinner();
        }
        
        [ClientRpc]
        private void ShowWinner()
        {
            gameUi.OnShowWinner(playerDatas.Values.ToArray()[0].username);
        }
    }
    
}
