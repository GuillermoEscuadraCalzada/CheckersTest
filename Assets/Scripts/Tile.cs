using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public enum TileChipT { NONE, BLACK, WHITE, BLACK_KING, WHITE_KING}
    public delegate void OnChipMovement();

    public class Tile : MonoBehaviour
    {
        //Type of the tile
        [SerializeField] TileChipT tileChipType = TileChipT.NONE;
        [SerializeField] private Renderer _renderer;
        [SerializeField] PlayerNumber playerNumber = PlayerNumber.NONE;
        [SerializeField] Vector2Int positionInBoard;
        [SerializeField] bool isEndline;
        public Color OriginalColor { get;  private set; }

        //Delegate para cuando una ficha entra en su collider
        OnChipMovement chipMovement;

        public Chip CurrentChip { get; private set; }

        public TileChipT TileCheckerType => tileChipType;
        public Vector2Int PositionInBoard { get => positionInBoard; set => positionInBoard = value; }
        public Renderer Renderer { get => _renderer; }
        public bool IsEndline => isEndline;
        private void Awake()
        {
            chipMovement = ChipMovesToTile;
            OriginalColor= _renderer.material.color;
        }

        private void OnTriggerEnter(Collider other)
        {
            CurrentChip = other.gameObject.GetComponent<Chip>();
            chipMovement?.Invoke();
        }

        /// <summary>
        /// Changes tile type depending on the color of the chip and if it is a checker
        /// </summary>
        /// <param name="chip_Color">Color of the chip</param>
        /// <param name="checkerIsKing">Is a Chip or a Checker</param>
        private void ChangeTileType(Chip_Color chip_Color,bool checkerIsKing)
        {
            switch (chip_Color)
            {
                case Chip_Color.WHITE: //Color white
                    tileChipType = checkerIsKing ? TileChipT.WHITE_KING : TileChipT.WHITE;
                    break;
                case Chip_Color.BLACK: //Color black
                    tileChipType = checkerIsKing ? TileChipT.BLACK_KING : TileChipT.BLACK;
                    break;
            }
        }

        private void OnTriggerExit(Collider other) {
            CurrentChip = null;
            chipMovement?.Invoke();
        }

        /// <summary>
        /// A new chip enters to this tile
        /// </summary>
        private void ChipMovesToTile()
        {
            //CurrentChip is not null
            if (CurrentChip)
            {
                ///Change tile type with the checker type
                ChangeTileType(CurrentChip.checkerColor, CurrentChip.IsChecker);
                //Chip evolves to a Checker
                CurrentChip.EvolveFromChipToChecker(this, playerNumber);
                CurrentChip.chipPosition.PositionInBoard = positionInBoard;
            }else 
                tileChipType = TileChipT.NONE; //Changes tile type to NONE in case thereis no chip
            CheckersBoard.BOARD_INDEXES[PositionInBoard.x, PositionInBoard.y] = (int)tileChipType; //Changes value of tile on the main board indexes
        }

    }

}