using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public enum Chip_Color
    {
        WHITE,
        BLACK
    }
    public class Chip : MonoBehaviour
    {
        public Chip_Color checkerColor; //El color de la ficha
        [SerializeField] bool isChecker; //Si es una dama o no
        [SerializeField] Player chipPlayer;

        public bool IsChecker { get =>isChecker; set => isChecker = value; }

        private void Awake()
        {
            chipPlayer.playerChips.Add(this);
        }

        public void EvolveFromChipToChecker(bool isEndline, PlayerNumber playerNumber)
        {
            if (isEndline && playerNumber != chipPlayer.PlayerNumber)
                IsChecker = true;
            
        }


    }
}
