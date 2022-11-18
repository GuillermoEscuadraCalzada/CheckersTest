using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public enum TileChipT { NONE, BLACK, WHITE, BLACK_KING, WHITE_KING}
    public  delegate void OnChipMovement();

    public class Tile : MonoBehaviour
    {
        public static int TileLayer = 8;
        //El tipo de la ficha
        [SerializeField] TileChipT tileChipType = TileChipT.NONE;
        [SerializeField] private Renderer _renderer;
        [SerializeField] PlayerNumber playerNumber = PlayerNumber.NONE;
        [SerializeField] Vector2Int positionInBoard;
        [SerializeField] bool isEndline;

        //Delegate para cuando una ficha entra en su collider
        OnChipMovement chipMovement;

        public Chip CurrentChip { get; private set; }

        public TileChipT TileCheckerType => tileChipType;
        public Vector2Int PositionInBoard { get => positionInBoard; set => positionInBoard = value; }
        public Renderer Renderer { get => _renderer; }

        private void Awake()
        {
            chipMovement = ChipMovesToTile;
        }

        private void OnTriggerEnter(Collider other)
        {
            CurrentChip = other.gameObject.GetComponent<Chip>();
            
            chipMovement?.Invoke();

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

        private void OnTriggerExit(Collider other) {
            tileChipType = TileChipT.NONE;
            CurrentChip= null;
        }

        /// <summary>
        /// Una nueva ficha entra a esta casilla
        /// </summary>
        private void ChipMovesToTile()
        {
            ///Se cambia el tipo de ficha 
            ChangeTileType(CurrentChip.checkerColor, CurrentChip.IsChecker);
            //La ficha evoluciona si es posible
            CurrentChip.EvolveFromChipToChecker(isEndline, playerNumber);
            CurrentChip.PositionInBoard = PositionInBoard;
            CheckersBoard.BOARD_INDEXES[PositionInBoard.x, PositionInBoard.y] = (int)tileChipType;
        }

    }

}