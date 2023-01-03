using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerUi : MonoBehaviour
    {
        // private GameObject hpBar;
        [SerializeField]
        private Image hpBarImage;
        [SerializeField]
        private TextMeshPro nameText;

        private int maxHp;
        public void Init(int maxHp)
        {
            hpBarImage.fillAmount = 1;
            this.maxHp = maxHp;
            // hpBar = GameObject.Find("HpBar");
        }
        
        public void UpdateHpBar(int currentHp)
        {
            hpBarImage.fillAmount = (float)currentHp / maxHp;
        }
        
        public void UpdateName(string playerName)
        {
            nameText.text = playerName;
        }
    }
    
}
