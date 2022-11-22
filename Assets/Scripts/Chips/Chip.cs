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
        public static int ChipLayer = 6;
        public Chip_Color checkerColor; //El color de la ficha
        [SerializeField] bool isChecker; //Si es una dama o no
        [SerializeField] Vector2Int positionInBoard; //posicion de la ficha en el arreglo de dos dimensiones del tablero
        [SerializeField] Player chipPlayer; //Referencia a su jugador

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
        /// <param name="isEndline">el booleano de la casilla a la que entr�</param>
        /// <param name="playerNumber">el n�mero de jugador al que pertenece la casilla</param>
        public void EvolveFromChipToChecker(Tile tileToCheck, PlayerNumber playerNumber)
        {
            //La casilla es el final de la l�nea y no le pertenece al jugador de esta ficha
            if (tileToCheck.IsEndline && playerNumber != chipPlayer.PlayerNumber)
                IsChecker = true; //Se hace una dama
            
        }

        private void OnMouseDown()
        {
            AvailableTilesToMove();    
        }


        /// <summary>
        /// Se realiza una b�squeda con el arreglo del tablero para saber qu� posiciones puede utilizar cada ficha.
        /// </summary>
        private void AvailableTilesToMove()
        {
            availableTiles.Clear();
            Vector2Int[] indexesToCheck = null;

            //Se pregunta si es el jugador uno
            if(chipPlayer.PlayerNumber == PlayerNumber.ONE)
            {
                ///Se establecen los movimientos para este jugador
                indexesToCheck = new Vector2Int[4]
                {
                    new Vector2Int(positionInBoard.x - 1, positionInBoard.y - 1),
                    new Vector2Int(positionInBoard.x - 1, positionInBoard.y + 1),
                    new Vector2Int(positionInBoard.x + 1, positionInBoard.y - 1),
                    new Vector2Int(positionInBoard.x + 1, positionInBoard.y + 1)
                };
            }
            //Se pregunta si es el jugador dos
            else if(chipPlayer.PlayerNumber == PlayerNumber.TWO)
            {
                //Se establecen los movimientos para este jugador
                indexesToCheck = new Vector2Int[4]
                                {
                    new Vector2Int(positionInBoard.x + 1, positionInBoard.y - 1),
                    new Vector2Int(positionInBoard.x + 1, positionInBoard.y + 1),
                    new Vector2Int(positionInBoard.x - 1, positionInBoard.y - 1),
                    new Vector2Int(positionInBoard.x - 1, positionInBoard.y + 1)
                                };
            }

            /*
                (pos.x - 1, pos.y - 1),     0,                  0
                0,                          pos,                0
                0,                          0,                  0
            */
            //Se pregunta si la posici�n est� dentro de los rangos del tablero
            if (!IndexIsOutOfRangeOrOccupied(indexesToCheck[0]))
                AddToAvailableTiles(indexesToCheck[0]); //Se modifica el material del tile para indicarle al jugador c�mo moverse


            /*
                0,                      0,                  (pos.x -1, pos.y + 1)
                0,                      pos,                0
                0,                      0,                  0
            */
            //Se pregunta si la posici�n est� dentro de los rangos del tablero
            if (!IndexIsOutOfRangeOrOccupied(indexesToCheck[1]))
                AddToAvailableTiles(indexesToCheck[1]); //Se modifica el material del tile para indicarle al jugador c�mo moverse
            if (isChecker)
            {
                /*
                    0,                      0,                  0
                    0,                      pos,                0
                    (pos.x + 1, pos.y - 1),  0,                  0
                */
            //Se pregunta si la posici�n est� dentro de los rangos del tablero
                if (!IndexIsOutOfRangeOrOccupied(indexesToCheck[2]))
                    AddToAvailableTiles(indexesToCheck[2]); //Se modifica el material del tile para indicarle al jugador c�mo moverse

                /*
                    0,                      0,                  0
                    0,                      pos,                0
                    0,                      0,                  (pos.x + 1, pos.y + 1)
                */
            //Se pregunta si la posici�n est� dentro de los rangos del tablero
                if (!IndexIsOutOfRangeOrOccupied(indexesToCheck[3]))
                    AddToAvailableTiles(indexesToCheck[3]); //Se modifica el material del tile para indicarle al jugador c�mo moverse
            }
            //ToggleAvailableTiles(true);
        
        }

        /// <summary>
        /// Se a�ade esta posici�n para los posibles movimientos del jugador.
        /// </summary>
        /// <param name="indexesToCheck"></param>
        private void AddToAvailableTiles(Vector2Int indexesToCheck)
        {
            availableTiles.Add(CheckersBoard.TilesArray[indexesToCheck.x, indexesToCheck.y]);
        }

        /// <summary>
        /// Se revisa si los �ndices indicados en el tablero est�n ocupados por fichas del mismo color <br></br>
        /// o si est�n fuera del rango del arreglo
        /// </summary>
        /// <param name="x">la fila donde se encuentra la ficha</param>
        /// <param name="y">la columna donde se encuentra la ficha</param>
        /// <returns>regresa verdadero si se encuentra fuera de rango o si hay una ficha del mismo color</returns>
        private bool IndexIsOutOfRangeOrOccupied(Vector2Int position)
        {
            //se revisa si la posici�n de la ficha est� fuera de rango
            if ((position.x < 0 || position.x > CheckersBoard.rows - 1) || (position.y < 0 || position.y > CheckersBoard.cols - 1)) return true;
            //se revisa si la casilla del tablero tiene ficha y en caso de que s�, se comparan colores.
            else if (CheckersBoard.TilesArray[position.x, position.y].CurrentChip &&
                (CheckersBoard.TilesArray[position.x, position.y].CurrentChip.checkerColor == checkerColor))            
                return true;
            
            return false;
        }

        public void ToggleAvailableTiles(bool toggle)
        {
            foreach (var tile in availableTiles)
            {
                tile.Renderer.material.color
                = toggle ? new Color(1, 0, 0) : tile.OriginalColor;
            }
        }

        void MoveToTile()
        {

            availableTiles.Clear();
        }
    }
}
