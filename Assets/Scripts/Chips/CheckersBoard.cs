using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Checkers
{

    public class CheckersBoard : MonoBehaviour
    {
        [SerializeField] Player player1;
        [SerializeField] Player player2;
        [SerializeField] Player currentPlayer;
        [SerializeField] private Transform rowsOfTilesFather;

        public const int rowsAndCols = 8; //Las filas totales 

        /*Por medio de un evento, actualizar los índices del tablero*/
        public static int[,] BOARD_INDEXES = new int[rowsAndCols, rowsAndCols];
        
        public static Tile[,] TilesArray { get; private set; } = new Tile[rowsAndCols, rowsAndCols];

        public static CheckersBoard Instance;

        public Player CurrentPlayer => currentPlayer; 

        Player NextPlayer => currentPlayer = currentPlayer == player1 ? player2 : player1;

        //El transform que guarda todas las filas del tablero.

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
            }
            StartBoardIndexes();
        }
        private void Start()
        {
            StartGame();
            
        }
        public void StartGame()
        {
            StartTurn(currentPlayer);
        }

        public void StartTurn(Player currentPlayer)
        {
            currentPlayer.ToggleMobilityOfChips();
            CheckersBoardUI.Instance.UpdatePlayerTurn(currentPlayer.name);
        }

        public void ChangePlayerTurn()
        {
            EndTurn(currentPlayer);  //Ends currentPlayer turn
            StartTurn(NextPlayer);  //Starts next player turn
        }

        /// <summary>
        /// Ends the turn of the current player and checks for end game conditions
        /// </summary>
        /// <param name="currentPlayer">The turn of the current Player</param>
        public void EndTurn(Player currentPlayer)
        {
            if (IsDraw()|| SomePlayerHasNoMoreMoves () || SomePlayerHasEatenAllEnemyChips())
            {
                EndGame();
            }
        }
        /// <summary>
        /// Checks for every posible move of every chip after each turn
        /// </summary>
        /// <param name="list"></param>
        /// <param name="globalMoves"></param>
        public void CheckForPlayerPosibleMoves(List<Chip> list, ref int globalMoves)
        {             
            foreach (var chip in list)
            {
                chip.AvailableTilesToMove();
                if (chip.AvailableTiles.Count > 0) 
                    globalMoves++;
            }            
        }

        private bool IsDraw()
        {
            int globalAvailableMoves = 0;
            CheckForPlayerPosibleMoves(player1.playerChips, ref globalAvailableMoves);
            CheckForPlayerPosibleMoves(player2.playerChips, ref globalAvailableMoves);
            if(globalAvailableMoves == 0)
            {
                string s = player1.playerChips.Count > player2.playerChips.Count ? "Player 1 wins" : "Player 2 wins";
                print(s);
            }
            //Verificar que ya no haya movimientos posibles
            return globalAvailableMoves == 0;
        }

        private bool SomePlayerHasNoMoreMoves()
        {
            if (player1.playerChips.Count == 0 || player2.playerChips.Count == 0) return false;
            int playerOneMoves = 0, playerTwoMoves = 0;
            string s = "";
            CheckForPlayerPosibleMoves(player1.playerChips, ref playerOneMoves);
            CheckForPlayerPosibleMoves(player2.playerChips, ref playerTwoMoves);
            if (playerOneMoves == 0)
            {
                s = "PlayerTwo Wins";
                print(s);
                return true;
            }
            else if (playerTwoMoves == 0)
            {
                s = "Player One Wins";
                print(s);
                return true;
            }
            return false;
        }

        private bool SomePlayerHasEatenAllEnemyChips()
        {
            if (player1.playerChips.Count == 0)
            {
                print("Player 2 Wins");
                return true;
            }
            else if (player2.playerChips.Count == 0)
            {
                print("Player 1 Wins");
                return true;
            }
            return false;
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