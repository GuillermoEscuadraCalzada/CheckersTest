using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public enum TileChipT { NONE, BLACK, WHITE, BLACK_KING, WHITE_KING}
    public class Tile : MonoBehaviour
    {
        //El tipo de la ficha
        [SerializeField] TileChipT tileChipType = TileChipT.NONE;
        [SerializeField] PlayerNumber playerNumber = PlayerNumber.NONE;
        [SerializeField] bool isEndline;

        public TileChipT TileCheckerType => tileChipType;


        private void OnTriggerEnter(Collider other)
        {
            Chip currentChip = other.gameObject.GetComponent<Chip>();
            ChangeTileType(currentChip.checkerColor, currentChip.IsChecker);
            currentChip.EvolveFromChipToChecker(isEndline, playerNumber);
        }

        /// <summary>
        /// Cambia el tipo de ficha que contiene la casilla
        /// </summary>
        /// <param name="chip_Color">El color de la ficha</param>
        /// <param name="checkerIsKing">Si la casilla es dama o no</param>
        private void ChangeTileType(Chip_Color chip_Color,bool checkerIsKing)
        {
            switch (chip_Color)
            {
                case Chip_Color.WHITE:
                    tileChipType = checkerIsKing ? TileChipT.WHITE_KING : TileChipT.WHITE;
                    break;
                case Chip_Color.BLACK:
                    tileChipType = checkerIsKing ? TileChipT.BLACK_KING : TileChipT.BLACK;
                    break;
            }
        }

        private void OnTriggerExit(Collider other) => tileChipType= TileChipT.NONE;
        

    }

}