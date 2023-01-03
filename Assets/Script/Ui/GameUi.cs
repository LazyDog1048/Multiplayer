using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameThing
{
    public class GameUi : MonoBehaviour
    {
        public GameHUDMemuUi hubUi;
        public GameCountDownMenuUi countDownUi;
        public GameOverMenuUi whoWinUi;

        IEnumerator Start()
        {
            while (!GameManager.IsEnable)
            {
                yield return null;
            }
            
        }

        public void OnStartGame()
        {
            countDownUi.StartCountDown();
        }
        
        public void OnShowWinner(string winner)
        {
            whoWinUi.SetWinnerText(winner); 
        }
    }
    
}
