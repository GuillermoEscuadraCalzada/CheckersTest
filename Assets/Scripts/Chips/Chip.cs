using NRKernal;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Checkers
{
    public enum Chip_Color
    {
        WHITE,
        BLACK
    }
    public class Chip : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public static int ChipLayer = 6;
        public Chip_Color checkerColor; //Chip color
        [SerializeField] bool isChecker; //Is a checker or normal chip
        [SerializeField] Player chipPlayer; //Player Reference
        [SerializeField] CameraChipSelection cameraChip;

        public ChipPosition chipPosition = new();


        public Color OriginalColor { get; private set; }

        public bool CanMove { get; set; } = false;

        public List<Tile> AvailableTiles { get; } = new(4);
        public List<Chip> PosibleChipsToEat { get; } = new(4);
        public bool IsChecker { get =>isChecker; set => isChecker = value; }

        public int ChipPlayerValue => chipPlayer.PlayerNumber;
        public Player ChipPlayer => chipPlayer;

        private void Awake()
        {
            OriginalColor = GetComponent<Renderer>().material.color;
            chipPlayer.playerChips.Add(this);
            CheckersBoardUI.Instance.UpdateText(ChipPlayerValue - 1, ChipPlayer.playerChips.Count.ToString());
        }

        private void OnDestroy()
        {
            CheckersBoard.BOARD_INDEXES[chipPosition.PositionInBoard.x, chipPosition.PositionInBoard.y] = 0;
            chipPlayer.playerChips.Remove(this);
            CheckersBoardUI.Instance.UpdateText(ChipPlayerValue - 1, ChipPlayer.playerChips.Count.ToString());
        }

        /// <summary>
        /// Chip evolves to a checker
        /// </summary>
        /// <param name="tileToCheck">The tile that will be evaluated</param>
        /// <param name="playerNumber">The number of the player the tile belongs to</param>
        public void EvolveFromChipToChecker(Tile tileToCheck, PlayerNumber playerNumber)
        {
            if (tileToCheck.IsEndline && (int)playerNumber != chipPlayer.PlayerNumber)
                IsChecker = true;
            if (IsChecker == true)
            {
                print("chip evolved to checker");
            }
        }
           

        /// <summary>
        ///Searches for the available tiles for this chip
        /// </summary>
        public void AvailableTilesToMove()
        {
            AvailableTiles.Clear(); //clears any possible available tile for this chip
            PosibleChipsToEat.Clear();
            //Creates an array with all 4 corners of this chip
            Vector2Int[]indexesToCheck = new Vector2Int[4] {
                chipPosition.Upper_Left(),
                chipPosition.Upper_Right(),
                chipPosition.Lower_Left(),
                chipPosition.Lower_Right()
            };

            ///Iterates through all 4 corners
            for (int i = 0; i < 4; i++)
            {
                //Checks if the chip player is the second one and if it is a checker, to avoid going to the upper left and right corners
                if (i < 2 && chipPlayer.PlayerNumber == (int)PlayerNumber.TWO && !IsChecker) continue;
                //Checks if the chip player is the first one and if it is a checker, to avoid going to the lower left and right corners
                else if (i == 2 && chipPlayer.PlayerNumber == (int)PlayerNumber.ONE && !IsChecker) break;

                //Checks if the indicated tile is available
                if (!CheckTileAvailabilty(indexesToCheck[i]))
                {
                    //Adds tile to the chip list
                    AvailableTiles.Add(CheckersBoard.TilesArray[indexesToCheck[i].x, indexesToCheck[i].y]);
                    Chip tileChip = CheckersBoard.TilesArray[indexesToCheck[i].x, indexesToCheck[i].y].CurrentChip;
                    if (tileChip) PosibleChipsToEat.Add(tileChip);

                }
            }
        }

        /// <summary>
        /// Checks if the tile position is out of bounds, if it can be eaten or if is occupied by another chip of the same player
        /// </summary>
        /// <param name="tilePosition">The position of the tile that will be checked</param>
        /// <returns>Returns true if the index is having an issue, returns false if the index is accesible</returns>
        private bool CheckTileAvailabilty(Vector2Int tilePosition)
        {
            //Checks index out of bounds
            if (PositionOutOfBounds(tilePosition)) return true;
            //checks if the checking chip is the same color as the one the tile has
            else if (CheckersBoard.TilesArray[tilePosition.x, tilePosition.y].CurrentChip &&
                (CheckersBoard.TilesArray[tilePosition.x, tilePosition.y].CurrentChip.checkerColor
                == checkerColor))
                return true;
            //Checks if the chip of the tile can be eaten
            else if (CheckersBoard.TilesArray[tilePosition.x, tilePosition.y].CurrentChip &&
                (CheckersBoard.TilesArray[tilePosition.x, tilePosition.y].CurrentChip.checkerColor
                != checkerColor))
            {
                return EnemyChipCantBeDestroyed(tilePosition);
            }
            return false;
        }

        /// <summary>
        /// Checks if parameter position is out of bounds 
        /// </summary>
        /// <param name="position">The position that will be checked</param>
        /// <returns>Returns true if the position is out of bounds</returns>
        private bool PositionOutOfBounds(Vector2Int position) => (position.x < 0 || position.x > CheckersBoard.rowsAndCols - 1) || (position.y < 0 || position.y > CheckersBoard.rowsAndCols - 1);
        

        /// <summary>
        /// Detects if the available tiles have an enemy chip, if they have one, it will check if there is an empty space behind
        /// </summary>
        /// <param name="PosibleMovements">The array of positions the chip to move will check</param>
        /// <param name="selectedTilePosition"> The selected tile 2D position on the board </param>
        /// <returns></returns>
        public bool EnemyChipCantBeDestroyed(Vector2Int selectedTilePosition )
        {
            //Creates a temporal Vector2Int
            Vector2Int res;

            //Switches the selected position through the 4 corners of the chip Position
            switch (selectedTilePosition)
            {
                //The selected tile is on the upper left corner
                case Vector2Int v when v.Equals(chipPosition.Upper_Left()):
                    res = ChipPosition.GetChipUpperLeftTile(v); //Gets upper left tile of selectedTilePosition
                    //Checks if position is out of bounds or different to 0
                    if (PositionOutOfBounds(res)||CheckersBoard.BOARD_INDEXES[res.x, res.y] != 0)
                        return true;
                    break;
                //The selected tile is on the upper right corner
                case Vector2Int v when v.Equals(chipPosition.Upper_Right()):
                    res = ChipPosition.GetChipUpperRightTile(v);//Gets upper right tile of selectedTilePosition
                    //Checks if position is out of bounds or different to 0
                    if (PositionOutOfBounds(res) || CheckersBoard.BOARD_INDEXES[res.x, res.y] != 0)
                        return true;
                    break;
                //The selected tile is on the lower left corner
                case Vector2Int v when v.Equals(chipPosition.Lower_Left()):
                    res = ChipPosition.GetChipLowerLeftTile(v);//Gets lower left tile of selectedTilePosition
                    //Checks if position is out of bounds or different to 0
                    if (PositionOutOfBounds(res) || CheckersBoard.BOARD_INDEXES[res.x, res.y] != 0)
                        return true;
                    break;
                //The selected tile is on the lower right corner
                case Vector2Int v when v.Equals(chipPosition.Lower_Right()):
                    res = ChipPosition.GetChipLowerRightTile(v);//Gets lower right tile of selectedTilePosition
                    //Checks if position is out of bounds or different to 0
                    if (PositionOutOfBounds(res) || CheckersBoard.BOARD_INDEXES[res.x, res.y] != 0)
                        return true;
                    break;
            }
            return false; //Returns false, can eat enemy chip
        }

        /// <summary>
        /// Toggles the color of the available tiles for this chip
        /// </summary>
        /// <param name="toggle">True if they will be turned on or false if they will return to original color</param>
        public void ToggleAvailableTiles(bool toggle)
        {
            //Iterates through every available tile
            foreach (var tile in AvailableTiles)
            {
                //Changes renderer color for this tile material
                tile.Renderer.material.color
                = toggle ? new Color(1, 0, 0) : tile.OriginalColor;
            }
        }

        /// <summary>
        /// Moves chip to the indicated tile
        /// </summary>
        /// <param name="tileToMove">The tile that has been selected where the chip will move</param>
        public void MoveToTile(Tile tileToMove, bool searchOnList = true)
        {
            if (!tileToMove) return; //Tile to move is nu ll
            //Looks for the tile on the list of tiles from this chip
            Tile foundTile = tileToMove;
            if (searchOnList)
                foundTile = AvailableTiles.Find(x => x == tileToMove);
            if (!foundTile) return; //Return if tile is null

            Chip prevChip = tileToMove.CurrentChip;
            if (prevChip)
            {
                Vector2Int newTile = SkipEatenChipTile(prevChip.chipPosition.PositionInBoard);
                Destroy(prevChip.gameObject);
                transform.localPosition = 
                    new
                    (CheckersBoard.TilesArray[newTile.x, newTile.y].transform.localPosition.x, 
                    transform.localPosition.y, 
                    CheckersBoard.TilesArray[newTile.x, newTile.y].transform.localPosition.z);


                chipPosition.PositionInBoard = new Vector2Int(newTile.x, newTile.y);
                //MoveToTile(CheckersBoard.TilesArray[newTile.x, newTile.y], false);
                AvailableTilesToMove();
                if (PosibleChipsToEat.Count == 0)
                {
                    CheckersBoard.Instance.ChangePlayerTurn();
                }
                else
                {
                    print("Moved Chip has a posibility to kill another chip");
                    ChipPlayer.ToggleMobilityOfChips(this, false);
                }
                return;
            }
            else
            {
                CheckersBoard.Instance.ChangePlayerTurn();
            }
            //Moves the position of the chip to the tile, conserving Y position
            transform.localPosition = new(tileToMove.transform.localPosition.x, y: transform.localPosition.y, tileToMove.transform.localPosition.z);
            //chipPosition.PositionInBoard = tileToMove.PositionInBoard;       
        }

        /// <summary>
        /// Skips the tile of the eaten chip and moves to the next one in the moving chip direction.
        /// </summary>
        /// <param name="eatenChipPositionInBoard">The position of the chip that is being eaten</param>
        /// <returns>Returns the position of the next tile the moving chip will end on after eating a chip</returns>
        public Vector2Int SkipEatenChipTile(Vector2Int eatenChipPositionInBoard)
        {
            //Switches the position of the eaten chip with the 4 corners of the moving chip
            switch (eatenChipPositionInBoard)
            {
                //The eating chip is on the upper left corner
                case Vector2Int v when v.Equals(chipPosition.Upper_Left()):
                    //Checks if the position that should be next is not out of bounds
                    if (!PositionOutOfBounds(v = ChipPosition.GetChipUpperLeftTile(v)))
                        return  v;
                    break;
                //The eating chip is on the upper right corner
                case Vector2Int v when v.Equals(chipPosition.Upper_Right()):
                    //Checks if the position that should be next is not out of bounds
                    if (!PositionOutOfBounds(v = ChipPosition.GetChipUpperRightTile(v)))
                        return v;
                    break;
                //The eating chip is on the lower left corner
                case Vector2Int v when v.Equals(chipPosition.Lower_Left()):
                    //Checks if the position that should be next is not out of bounds
                    if (!PositionOutOfBounds(v = ChipPosition.GetChipLowerLeftTile(v)))
                        return v;
                    break;
                //The eating chip is on the lower right corner
                case Vector2Int v when v.Equals(chipPosition.Lower_Right()):
                    //Checks if the position that should be next is not out of bounds
                    if (!PositionOutOfBounds(v = ChipPosition.GetChipLowerRightTile(v)))
                        return v;
                    break;
            }
            return eatenChipPositionInBoard;
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            cameraChip.ClickChip(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            cameraChip.OnHoverEnter(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            cameraChip.OnHoverEnter(this, false);
        }
    }
}
