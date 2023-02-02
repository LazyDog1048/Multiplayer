using System.Collections;
using System.Collections.Generic;
using Steamworks;
using UnityEngine;

namespace SteamThing
{

    public class AvatarDownloadder
    {
        public static AvatarDownloadder Instance { get; private set; }
        Callback<AvatarImageLoaded_t> avatarImageLoaded;
        public AvatarDownloadder()
        { 
            Instance = this;
            avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
        }

        public void StartDownloadAvatar(SteamBaseInfo steamBaseInfo)
        {
            int imageId = SteamFriends.GetLargeFriendAvatar(steamBaseInfo.steamId);
            if (imageId == -1)
            {
                return;
            }
            // Debug.Log($"{steamBaseInfo.personName} 头像 正在下载中....");
            var texture = GetSteamImageAsTexture2D(imageId);
            steamBaseInfo.SetTexture(texture);
        }

        private void OnAvatarImageLoaded(AvatarImageLoaded_t pCallback)
        {
            Debug.Log("下载完毕!");
            SteamBaseInfo steamBaseInfo = SteamFriendsManager.Instance.GetSteamBaseInfo(pCallback.m_steamID);
            var texture = GetSteamImageAsTexture2D(pCallback.m_iImage);
            steamBaseInfo.SetTexture(texture);
        }

        private Texture2D GetSteamImageAsTexture2D(int iImage)
        {
            Texture2D texture2D = null;
            bool isValid = SteamUtils.GetImageSize(iImage, out uint width, out uint height);

            if (isValid)
            {
                byte[] image = new byte[width * height * 4];
                isValid = SteamUtils.GetImageRGBA(iImage, image, (int)(width * height * 4));
                
                if(isValid)
                {
                    texture2D = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false, true);
                    texture2D.LoadRawTextureData(image); 
                    texture2D.Apply();
                }
            }

            return texture2D;
        }
    }

}