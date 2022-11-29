using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Checkers
{
    public class CheckersBoardUI : MonoBehaviour
    {
        public static CheckersBoardUI Instance { get; private set; }


        [SerializeField] Text playerTurn;
        [SerializeField] Text[] playersChipCount;

        private void Awake()
        {
            if (!Instance) Instance = this;
        }

        public void UpdatePlayerTurn(string playerName)
        {
            playerTurn.text = playerName + " turn";
        }

        public void UpdateText(int index, string text)
        {
            if (playersChipCount[index] == null) return;
            playersChipCount[index].text = text;
        }

    }
}