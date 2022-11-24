using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


#if UNITY_EDITOR
namespace Checkers
{

    public class CheckersBoard : MonoBehaviour
    {
        [SerializeField] Player player1;
        [SerializeField] Player player2;
        [SerializeField] Player currentPlayer;
        Player NextPlayer =>currentPlayer == player1 ? player2 : player1;

        Tile currentTile;

        public const int rowsAndCols = 8; //Las filas totales 

        /*Por medio de un evento, actualizar los índices del tablero*/
        public static int[,] BOARD_INDEXES = new int[rowsAndCols, rowsAndCols];
        
        public static Tile[,] TilesArray { get; private set; } = new Tile[rowsAndCols, rowsAndCols];

        //El transform que guarda todas las filas del tablero.
        [SerializeField] private Transform rowsOfTilesFather;

        private void Start()
        {
            StartBoardIndexes();
            StartGame();
        }

        public void StartGame()
        {
            //StartTurn(currentPlayer);
        }

        public void StartTurn(Player currentPlayer)
        {
            if (IsDraw())
            {
                //En caso de que ninguna ficha se pueda mover
                EndGame();
            }

            //if (currentPlayer == player1)
            //    currentPlayer = player2;

            //else if (currentPlayer == player2)
            //    currentPlayer = player1;
            
            //currentTile.ChipMovesToTile(); //Hacer público ChipMovesToTile
            ChangePlayerTurn(); 
        }

        public void ChangePlayerTurn()
        {
            EndTurn(currentPlayer);  //Terminar turno actual
            StartTurn(NextPlayer);    //Inicia nuevo turno
        }

        public void EndTurn(Player currentPlayer)
        {
            // Validar si alguno de los dos ha ganado
                //Win condition
            //
        }

        private bool IsDraw()
        {
            //Verificar que ya no haya movimientos posibles
            return false/*true*/;
        }

        public void EndGame()
        {

        }

        /// <summary>
        /// Se inicializan los valores del tablero
        /// </summary>
        private void StartBoardIndexes()
        {
            //Arreglo de 8x8 donde se guardarán temporalmente los objetos casilla del tablero
            for (int i = 0;i < rowsAndCols;i++)
            {
                for(int j = 0; j < rowsAndCols; j++)
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
