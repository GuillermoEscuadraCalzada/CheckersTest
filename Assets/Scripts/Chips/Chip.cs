using NRKernal;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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

        public ChipPosition chipPosition;

        private Vector2Int[] indexesToCheck = null;

        public Color OriginalColor { get; private set; }

        public bool HasBeenSelected { get; set; }

        private List<Tile> availableTiles = new List<Tile>(4);
        public bool IsChecker { get =>isChecker; set => isChecker = value; }

        public int ChipPlayerValue => (int)chipPlayer.PlayerNumber;


        private void Awake()
        {
            chipPosition = new ChipPosition();
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
            indexesToCheck = new Vector2Int[4]
            {
                chipPosition.Upper_Left(),
                chipPosition.Upper_Right(),
                chipPosition.Lower_Left(),
                chipPosition.Lower_Right()
            };

            for (int i = 0; i < 4; i++)
            {
                if (i < 2 && chipPlayer.PlayerNumber == PlayerNumber.TWO && !IsChecker) continue;
                else if (i == 2 && chipPlayer.PlayerNumber == PlayerNumber.ONE && !IsChecker) break;

                //EnemyChipCanBeDestroyed(indexesToCheck[0], indexesToCheck);
                //Se pregunta si la posición está dentro de los rangos del tablero
                if (!IndexIsOutOfRangeOrOccupied(indexesToCheck[i])) 
                    availableTiles.Add(CheckersBoard.TilesArray[indexesToCheck[i].x, indexesToCheck[i].y]);
                //Se modifica el material del tile para indicarle al jugador cómo moverse
            }
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
            if (PositionOutOfBounds(position)) return true;
            //se revisa si la casilla del tablero tiene ficha y en caso de que sí, se comparan colores.
            else if (CheckersBoard.TilesArray[position.x, position.y].CurrentChip &&
                (CheckersBoard.TilesArray[position.x, position.y].CurrentChip.checkerColor
                == checkerColor))
                return true;
            else if (CheckersBoard.TilesArray[position.x, position.y].CurrentChip &&
                (CheckersBoard.TilesArray[position.x, position.y].CurrentChip.checkerColor
                != checkerColor))
            {
                return EnemyChipCantBeDestroyed(position);
            }
            return false;
        }

        private bool PositionOutOfBounds(Vector2Int position)
        {
            return (position.x < 0 || position.x > CheckersBoard.rows - 1) || (position.y < 0 || position.y > CheckersBoard.cols - 1);
        }

        /// <summary>
        /// Detects if the available tiles have an enemy chip, if they have one, it will check if there is an empty space behind
        /// </summary>
        /// <param name="PosibleMovements">The array of positions the chip to move will check</param>
        /// <param name="selectedTilePosition"> The selected tile 2D position on the board </param>
        /// <returns></returns>
        public bool EnemyChipCantBeDestroyed(Vector2Int selectedTilePosition )
        {
            //Creates a double array foreach 

            Vector2Int res = new Vector2Int();
            switch (selectedTilePosition)
            {
                case Vector2Int v when v.Equals(chipPosition.Upper_Left()):
                    print("Tile is upper Left");
                    res = chipPosition.Upper_Left(v);
                    if (PositionOutOfBounds(res) )return true;
                    if (CheckersBoard.BOARD_INDEXES[res.x, res.y] != 0)
                        return true;
                    break;
                case Vector2Int v when v.Equals(chipPosition.Upper_Right()):
                    print("Tile is upper Right");
                    res = chipPosition.Upper_Right(v);
                    if (PositionOutOfBounds(res) )return PositionOutOfBounds(res);
                    if (CheckersBoard.BOARD_INDEXES[res.x,res.y] != 0)
                        return true;
                    break;
                case Vector2Int v when v.Equals(chipPosition.Lower_Left()):
                    print("Tile is lower Left");
                    res = chipPosition.Lower_Left(v);
                    if (PositionOutOfBounds(res) )return PositionOutOfBounds(res);
                    if (CheckersBoard.BOARD_INDEXES[res.x, res.y] != 0)
                        return true;
                    break;
                case Vector2Int v when v.Equals(chipPosition.Lower_Right()):
                    print("Tile is lower Right");
                    res = chipPosition.Lower_Right(v);
                    if (PositionOutOfBounds(res)) return PositionOutOfBounds(res);
                    if (CheckersBoard.BOARD_INDEXES[res.x, res.y] != 0)
                        return true;
                    break;
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

            //Vector2Int v =new Vector2Int();

            switch (eatenChipPositionInBoard)
            {
                case Vector2Int v when v.Equals(chipPosition.Upper_Left()):
                    if (!PositionOutOfBounds(v = chipPosition.Upper_Left(v)))
                        return v;
                    break;
                case Vector2Int v when v.Equals(chipPosition.Upper_Right()):
                    if (!PositionOutOfBounds(v = chipPosition.Upper_Right(v)))
                        return v;
                    break;
                case Vector2Int v when v.Equals(chipPosition.Lower_Left()):
                    if (!PositionOutOfBounds(v =chipPosition.Lower_Left(v)))
                        return v;
                    break;
                case Vector2Int v when v.Equals(chipPosition.Lower_Right()):
                    if (!PositionOutOfBounds(v =chipPosition.Lower_Right(v)))
                        return v;
                    break;
            }


            //if (chipPlayer.PlayerNumber == PlayerNumber.ONE)
            //{
            //    if(eatenChipPositionInBoard == indexesToCheck[0] && 
            //        !PositionOutOfBounds(v = new Vector2Int(eatenChipPositionInBoard.x - 1, eatenChipPositionInBoard.y - 1)))
            //    {
            //        return v;
            //    }
            //    else if (eatenChipPositionInBoard == indexesToCheck[1] &&
            //        !PositionOutOfBounds(v = new Vector2Int(eatenChipPositionInBoard.x - 1, eatenChipPositionInBoard.y + 1)))
            //    {
            //        return v;
            //    }
            //    else if (eatenChipPositionInBoard == indexesToCheck[2] &&
            //        !PositionOutOfBounds(v = new Vector2Int(eatenChipPositionInBoard.x + 1, eatenChipPositionInBoard.y - 1)))
            //    {
            //        return v;
            //    }
            //    else if (eatenChipPositionInBoard == indexesToCheck[3]
            //        && !PositionOutOfBounds(v = new Vector2Int(eatenChipPositionInBoard.x + 1, eatenChipPositionInBoard.y + 1)))
            //    {
            //        return v;
            //    }

            //}
            //else if(chipPlayer.PlayerNumber == PlayerNumber.TWO)
            //{
            //    if (eatenChipPositionInBoard == indexesToCheck[0] &&
            //        !PositionOutOfBounds(v = new Vector2Int(eatenChipPositionInBoard.x + 1, eatenChipPositionInBoard.y - 1)))
            //    {
            //        return v;
            //    }
            //    else if (eatenChipPositionInBoard == indexesToCheck[1] &&
            //        !PositionOutOfBounds(v = new Vector2Int(eatenChipPositionInBoard.x + 1, eatenChipPositionInBoard.y - 1)))
            //    {
            //        return v;
            //    }
            //    else if (eatenChipPositionInBoard == indexesToCheck[3] &&
            //        !PositionOutOfBounds(v = new Vector2Int(eatenChipPositionInBoard.x + 1, eatenChipPositionInBoard.y - 1)))
            //    {
            //        return v;
            //    }
            //    else if (eatenChipPositionInBoard == indexesToCheck[4] &&
            //        !PositionOutOfBounds(v = new Vector2Int(eatenChipPositionInBoard.x + 1, eatenChipPositionInBoard.y - 1)))
            //    {
            //        return v;
            //    }
            //}
            return eatenChipPositionInBoard;
        }


    }
}
