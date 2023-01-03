using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace GameThing
{
    public class GameOverMenuUi : MonoBehaviour
    {
        public TextMeshProUGUI winnerText;
        
        public void SetWinnerText(string winner)
        {
            winnerText.text = winner;
            gameObject.SetActive(true);
        }
    }
    
}
