using System.Collections;
using System.Collections.Generic;
using GameThing;
using Steamworks;
using UnityEngine;
using UnityEngine.Events;

namespace SteamThing
{
    [System.Serializable]
    public class SteamBaseInfo
    {
        public readonly CSteamID steamId;
        public Texture2D steamImage { get; private set; }
        public string personName;
        public EPersonaState personaState;

        public int avatarState = 0;

        public UnityEvent<Texture2D> onTextureDownloaded;
        public SteamBaseInfo(CSteamID steamId)
        {
            this.steamId = steamId;
            steamImage = null;
            personName = SteamFriends.GetFriendPersonaName(steamId);
            personaState = SteamFriends.GetFriendPersonaState(steamId);
            avatarState = -1;
            onTextureDownloaded = new UnityEvent<Texture2D>();
            AvatarDownloadder.Instance.StartDownloadAvatar(this);
            // Debug.Log("[" + personName + "]是否拥有此产品?=" + SteamApps.BIsSubscribedApp(SteamFriendsManager.Instance.AppId));
        }

        // public void StartDownload()
        // {
        //     
        // }
        public void SetTexture(Texture2D texture)
        {
            if(texture == null)
                return;
            steamImage = texture;
            avatarState = 1;
            onTextureDownloaded?.Invoke(texture);
        }
        
    }
}
