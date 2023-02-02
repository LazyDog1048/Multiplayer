using System;
using System.Collections;
using System.Collections.Generic;
using GameThing;
using Steamworks;
using UnityEngine;

namespace SteamThing
{
    public class SteamFriendsManager: MonoBehaviour
    {
        public static SteamFriendsManager Instance { get; private set; }
        
        public AppId_t AppId = (AppId_t)1109040;   
        public EFriendFlags eFriendFlags = EFriendFlags.k_EFriendFlagAll;
        
        public SteamFriendMenuUi SteamFriendMenuUi;
        
        private Dictionary<CSteamID, SteamBaseInfo> _allMySteamUsersInfosDic;

        public Dictionary<CSteamID, SteamBaseInfo> AllMySteamUsersInfosDic => _allMySteamUsersInfosDic;
        AvatarDownloadder avatarDownloadder;
        // public SteamFriendsManager()
        // {
        //     Instance = this;
        //     allMySteamUsersInfosList = new List<SteamBaseInfo>();
        //     _allMySteamUsersInfosDic = new Dictionary<CSteamID, SteamBaseInfo>();
        //     GetOwnerFriends();
        // }

        private void Start()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(this);
            
            avatarDownloadder = new AvatarDownloadder();
            _allMySteamUsersInfosDic = new Dictionary<CSteamID, SteamBaseInfo>();
            GetOwnerFriends();
            SteamFriendMenuUi.LoadAllFriends();
        }

        private void GetOwnerFriends()
        {
            //本地自己
            CSteamID csid = SteamApps.GetAppOwner();
            SteamBaseInfo ower = new SteamBaseInfo(csid);
            
            _allMySteamUsersInfosDic.Add(ower.steamId, ower);
            for (int i = 0; i < SteamFriends.GetFriendCount(eFriendFlags); i++)
            {
                if (SteamApps.BIsSubscribedApp(AppId))//必须拥有本产品的好友
                {
                }
                SteamBaseInfo steamBaseInfo = new SteamBaseInfo(SteamFriends.GetFriendByIndex(i, eFriendFlags));
                _allMySteamUsersInfosDic.Add(steamBaseInfo.steamId, steamBaseInfo);
            }
            Callback<PersonaStateChange_t>.Create(SteamPersonaStateChange);
            
        }


        private void SteamPersonaStateChange(PersonaStateChange_t callback)
        {
            Debug.Log($"SteamPersonaStateChange {callback.m_ulSteamID} {callback.m_nChangeFlags}");
            UpdateSteamFriendInfo();
        }
        
        private void UpdateSteamFriendInfo()
        {
            foreach (var key_val in _allMySteamUsersInfosDic)
            {
                key_val.Value.personaState = SteamFriends.GetFriendPersonaState(key_val.Key);
            }
        }
        
        public SteamBaseInfo GetSteamBaseInfo(CSteamID steamId)
        {
            if (_allMySteamUsersInfosDic.ContainsKey(steamId))
            {
                return _allMySteamUsersInfosDic[steamId];
            }
            return null;
        }
    }
    
}
