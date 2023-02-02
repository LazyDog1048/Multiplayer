using System;
using System.Collections;
using System.Collections.Generic;
using Extend;
using SteamThing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameThing
{
    public class SteamFriendPanel
    {
        public RawImage Avatar;
        public TextMeshProUGUI Name;
        
        public SteamFriendPanel(Transform transform,SteamBaseInfo steamBaseInfo)
        {
            Avatar = transform.Find("Image").GetComponent<RawImage>();
            Name = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            Name.text = steamBaseInfo.personName;
            Avatar.texture = steamBaseInfo.steamImage;
            steamBaseInfo.onTextureDownloaded.AddListener(UpdateSprite);
        }

        private void UpdateSprite(Texture2D texture2D)
        {
            Avatar.texture = texture2D;
        }
    }
    
    public class SteamFriendMenuUi : MonoBehaviour
    {
        [SerializeField]
        private GameObject oriPanel;
        [SerializeField]
        private Transform panel;
        
        public void LoadAllFriends()
        {
            foreach (var steamBaseInfo in SteamFriendsManager.Instance.AllMySteamUsersInfosDic)
            {
                var newPanel = Instantiate(oriPanel, panel);
                newPanel.SetActive(true);
                SteamFriendPanel steamFriendPanel = new SteamFriendPanel(newPanel.transform,steamBaseInfo.Value);        
            }
        }
    }
}
