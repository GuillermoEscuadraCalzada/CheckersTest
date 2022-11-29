using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Checkers
{
    public enum TileChipT { NONE, BLACK, WHITE, BLACK_KING, WHITE_KING}
    public delegate void OnChipMovement();

    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        //Type of the tile
        [SerializeField] TileChipT tileChipType = TileChipT.NONE;
        [SerializeField] PlayerNumber playerNumber = PlayerNumber.NONE;
        [SerializeField] Vector2Int positionInBoard;
        [SerializeField] bool isEndline;
        [SerializeField] Collider _collider;

        [SerializeField] CameraChipSelection cameraChip;
        public Color OriginalColor { get;  private set; }

        //Delegate para cuando una ficha entra en su collider
        OnChipMovement chipMovement;

        public Chip CurrentChip { get; private set; }

        public TileChipT TileCheckerType => tileChipType;
        public Vector2Int PositionInBoard { get => positionInBoard; set => positionInBoard = value; }
        public bool IsEndline => isEndline;
        private void Awake()
        {
            chipMovement = ChipMovesToTile;
            OriginalColor= GetComponent<Renderer>().material.color;
        }

        public void OnTriggerEnter(Collider other)
        {
            CurrentChip = other.gameObject.GetComponent<Chip>();
            chipMovement?.Invoke();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log("Click");
            cameraChip.TileSelection(this);
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