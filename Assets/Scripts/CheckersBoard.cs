using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace Checkers
{
    public class CheckersBoard : MonoBehaviour
    {
        public const int rows = 8; //Las filas totales 
        public const int cols = 8; //Las columnas totales

        /*Por medio de un evento, actualizar los índices del tablero*/
        public static int[,] BOARD_INDEXES = new int[rows, cols];
        
        public static Tile[,] TilesArray { get; private set; } = new Tile[rows, cols];

        //El transform que guarda todas las filas del tablero.
        [SerializeField] private Transform rowsOfTilesFather;

        private void Start()
        {
            StartBoardIndexes();
        }

        /// <summary>
        /// Se inicializan los valores del tablero
        /// </summary>
        private void StartBoardIndexes()
        {
            int i = 0; int j = 0;
            //Arreglo de 8x8 donde se guardarán temporalmente los objetos casilla del tablero

            for(i = 0;i < rows;i++)
            {
                for(j = 0; j < cols; j++)
                {
                    Tile tileOfChild = rowsOfTilesFather.GetChild(i).GetChild(j).GetComponent<Tile>();
                    BOARD_INDEXES[i,j] = 0; //Comienzan en un valor de cero

                    if (!tileOfChild) continue;
                    TilesArray[i, j] = tileOfChild;
                    TilesArray[i, j].PositionInBoard = new Vector2Int(i, j);
                    TilesArray[i, j].gameObject.name = $"Tile[{i},{j}]";
                }
            }
        }




    }
}
