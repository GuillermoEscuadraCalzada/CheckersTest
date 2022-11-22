using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


#if UNITY_EDITOR
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
            //Arreglo de 8x8 donde se guardarán temporalmente los objetos casilla del tablero
            for (int i = 0;i < rows;i++)
            {
                for(int j = 0; j < cols; j++)
                {
                    Tile tileOfChild = rowsOfTilesFather.GetChild(i).GetChild(j).GetComponent<Tile>();
                    BOARD_INDEXES[i, j] = 0; //Comienzan en un valor de cero
                   
                    if (!tileOfChild) continue; //la variable es nula

                    TilesArray[i, j] = tileOfChild; //se guarda la referencia al componente
                    TilesArray[i, j].PositionInBoard = new Vector2Int(i, j); //se crea una nueva posición para la casilla
                }
            }
        }

    }
}
#endif
