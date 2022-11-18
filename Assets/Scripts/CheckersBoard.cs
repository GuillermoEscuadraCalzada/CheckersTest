using UnityEngine;

namespace Checkers
{
    public class CheckersBoard : MonoBehaviour
    {
        public const int rows = 8; //Las filas totales 
        public const int cols = 8; //Las columnas totales

        /*Por medio de un evento, actualizar los índices del tablero*/
        public static int[,] BOARD_INDEXES = new int[rows, cols];

        //public event CheckerHasEntered;

        private void Awake()
        {
            StartBoardIndexes();
        }

        /// <summary>
        /// Se inicializan los valores del tablero
        /// </summary>
        private void StartBoardIndexes()
        {
            for(int i = 0;i < rows;i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    BOARD_INDEXES[i,j] = 0; //Comienzan en un valor de cero
                }
            }
        }

    }
}
