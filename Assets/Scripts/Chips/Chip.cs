using NRKernal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Checkers
{
    public enum Chip_Color
    {
        WHITE,
        BLACK
    }
    public class Chip : MonoBehaviour
    {
        public static int ChipLayer = 6;
        public Chip_Color checkerColor; //Chip color
        [SerializeField] bool isChecker; //Is a checker or normal chip
        [SerializeField] Vector2Int positionInBoard; //Position of chip on the board 2D array 
        [SerializeField] Player chipPlayer; //Player Reference
        private Vector2Int[] indexesToCheck = null;

        public Color OriginalColor { get; private set; }

        public bool HasBeenSelected { get; set; }

        private List<Tile> availableTiles = new List<Tile>(4);

        public Vector2Int PositionInBoard { get => positionInBoard; set => positionInBoard = value; }

        public bool IsChecker { get =>isChecker; set => isChecker = value; }

        public int ChipPlayerValue => (int)chipPlayer.PlayerNumber;


        private void Awake()
        {
            OriginalColor = GetComponent<Renderer>().material.color;
            chipPlayer.playerChips.Add(this);
        }

        /// <summary>
        /// La ficha evoluciona a ser una dama
        /// </summary>
        /// <param name="isEndline">el booleano de la casilla a la que entró</param>
        /// <param name="playerNumber">el número de jugador al que pertenece la casilla</param>
        public void EvolveFromChipToChecker(Tile tileToCheck, PlayerNumber playerNumber)
        {
            //La casilla es el final de la línea y no le pertenece al jugador de esta ficha
            if (tileToCheck.IsEndline && playerNumber != chipPlayer.PlayerNumber)
                IsChecker = true; //Se hace una dama
            
        }

        /// <summary>
        /// Se realiza una búsqueda con el arreglo del tablero para saber qué posiciones puede utilizar cada ficha.
        /// </summary>
        public void AvailableTilesToMove()
        {
            availableTiles.Clear();

            /*Hacer variable privada para que sea accedida en otra función y checar manualmente cada caso*/
            //Se pregunta si es el jugador uno
            indexesToCheck = IndexesFromPosition(positionInBoard);

            for (int i = 0; i < 4; i++)
            {
                if (i == 2 && !IsChecker) break;

                //EnemyChipCanBeDestroyed(indexesToCheck[0], indexesToCheck);
                //Se pregunta si la posición está dentro de los rangos del tablero
                if (!IndexIsOutOfRangeOrOccupied(indexesToCheck[i]))
                    availableTiles.Add(CheckersBoard.TilesArray[indexesToCheck[i].x, indexesToCheck[i].y]);
                //Se modifica el material del tile para indicarle al jugador cómo moverse
            }
        }

        private Vector2Int[] IndexesFromPosition(Vector2Int positionStart)
        {
            if (chipPlayer.PlayerNumber == PlayerNumber.ONE)
            {
                ///Se establecen los movimientos para este jugador
                return new Vector2Int[4]
                {
                    new Vector2Int(positionStart.x - 1, positionStart.y - 1),
                    new Vector2Int(positionStart.x - 1, positionStart.y + 1),
                    new Vector2Int(positionStart.x + 1, positionStart.y - 1),
                    new Vector2Int(positionStart.x + 1, positionStart.y + 1)
                };
            }
            //Se pregunta si es el jugador dos
            else if (chipPlayer.PlayerNumber == PlayerNumber.TWO)
            {
                //Se establecen los movimientos para este jugador
                return new Vector2Int[4]
                {
                    new Vector2Int(positionStart.x + 1, positionStart.y - 1),
                    new Vector2Int(positionStart.x + 1, positionStart.y + 1),
                    new Vector2Int(positionStart.x - 1, positionStart.y - 1),
                    new Vector2Int(positionStart.x - 1, positionStart.y + 1)
                };
            }
            return null;
        }


        /// <summary>
        /// Se revisa si los índices indicados en el tablero están ocupados por fichas del mismo color <br></br>
        /// o si están fuera del rango del arreglo
        /// </summary>
        /// <param name="x">la fila donde se encuentra la ficha</param>
        /// <param name="y">la columna donde se encuentra la ficha</param>
        /// <returns>regresa verdadero si se encuentra fuera de rango o si hay una ficha del mismo color</returns>
        private bool IndexIsOutOfRangeOrOccupied(Vector2Int position)
        {
            //se revisa si la posición de la ficha está fuera de rango
            if ((position.x < 0 || position.x > CheckersBoard.rows - 1) || (position.y < 0 || position.y > CheckersBoard.cols - 1)) return true;
            //se revisa si la casilla del tablero tiene ficha y en caso de que sí, se comparan colores.
            else if (CheckersBoard.TilesArray[position.x, position.y].CurrentChip &&
                (CheckersBoard.TilesArray[position.x, position.y].CurrentChip.checkerColor 
                == checkerColor))            
                return true;
            else if(CheckersBoard.TilesArray[position.x, position.y].CurrentChip &&
                (CheckersBoard.TilesArray[position.x, position.y].CurrentChip.checkerColor
                != checkerColor))
            {
                return EnemyChipCantBeDestroyed(position);
            }
            return false;
        }

        /// <summary>
        /// Detects if the available tiles have an enemy chip, if they have one, it will check if there is an empty space behind
        /// </summary>
        /// <param name="PosibleMovements">The array of positions the chip to move will check</param>
        /// <param name="selectedTilePosition"> The selected tile 2D position on the board </param>
        /// <returns></returns>
        public bool EnemyChipCantBeDestroyed(Vector2Int? selectedTilePosition = null)
        {
            if (selectedTilePosition == null) selectedTilePosition = positionInBoard;
            //Creates a double array foreach 
            Vector2Int[] PositionsOfSelectedTile = IndexesFromPosition((Vector2Int)selectedTilePosition);
            //Iterates through all 4 positions
            for (int i = 0; i < 4; i++)
            {
                //The selected tile is equal to the parameter array
                if (selectedTilePosition == indexesToCheck[i])
                {
                    if ((PositionsOfSelectedTile[i].x < 0 || PositionsOfSelectedTile[i].x > CheckersBoard.cols - 1)
                        || (PositionsOfSelectedTile[i].y< 0 || PositionsOfSelectedTile[i].y > CheckersBoard.cols - 1)) continue;
                    //Check board indexes with the position tiles to check if is empty
                    if (CheckersBoard.BOARD_INDEXES[PositionsOfSelectedTile[i].x, PositionsOfSelectedTile[i].y] != 0)
                    {
                        return true; //Returns true, can eat enemy chip
                    }
                }
            }
            return false; //Returns false, can't eat enemy chip
        }

        /// <summary>
        /// Toggles the color of the available tiles for this chip
        /// </summary>
        /// <param name="toggle">True if they will be turned on or false if they will return to original color</param>
        public void ToggleAvailableTiles(bool toggle)
        {
            //Iterates through every available tile
            foreach (var tile in availableTiles)
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
                foundTile = availableTiles.Find(x => x == tileToMove);
            if (!foundTile) return; //Return if tile is null
            //Moves the position of the chip to the tile, conserving Y position
            transform.position = new(tileToMove.transform.position.x, transform.position.y, tileToMove.transform.position.z);
            availableTiles.Clear(); //Clears the tiles available for the chip
        }

        public Vector2Int SkipEatenChipTile(Vector2Int eatenChipPositionInBoard)
        {
            //if ((eatenChipPositionInBoard.x < 0 || eatenChipPositionInBoard.x > CheckersBoard.cols - 1)
            //            || (eatenChipPositionInBoard.y < 0 || eatenChipPositionInBoard.y > CheckersBoard.cols - 1))
            //    return eatenChipPositionInBoard;

            if (chipPlayer.PlayerNumber == PlayerNumber.ONE)
            {
                if(eatenChipPositionInBoard == indexesToCheck[0])
                {
                    return new Vector2Int(eatenChipPositionInBoard.x - 1, eatenChipPositionInBoard.y - 1);
                }
                else if (eatenChipPositionInBoard == indexesToCheck[1])
                {
                    return new Vector2Int(eatenChipPositionInBoard.x - 1, eatenChipPositionInBoard.y + 1);
                }
                else if (eatenChipPositionInBoard == indexesToCheck[2])
                {
                    return new Vector2Int(eatenChipPositionInBoard.x + 1, eatenChipPositionInBoard.y - 1);
                }
                else if (eatenChipPositionInBoard == indexesToCheck[3])
                {
                    return new Vector2Int(eatenChipPositionInBoard.x + 1, eatenChipPositionInBoard.y + 1);
                }

            }
            else if(chipPlayer.PlayerNumber == PlayerNumber.TWO)
            {
                if (eatenChipPositionInBoard == indexesToCheck[0])
                {
                    return new Vector2Int(eatenChipPositionInBoard.x + 1, eatenChipPositionInBoard.y - 1);
                }
                else if (eatenChipPositionInBoard == indexesToCheck[1])
                {
                    return new Vector2Int(eatenChipPositionInBoard.x + 1, eatenChipPositionInBoard.y + 1);
                }
                else if (eatenChipPositionInBoard == indexesToCheck[3])
                {
                    return new Vector2Int(eatenChipPositionInBoard.x - 1, eatenChipPositionInBoard.y - 1);
                }
                else if (eatenChipPositionInBoard == indexesToCheck[4])
                {
                    return new Vector2Int(eatenChipPositionInBoard.x - 1, eatenChipPositionInBoard.y + 1);
                }
            }
            return new Vector2Int();
        }


    }
}
