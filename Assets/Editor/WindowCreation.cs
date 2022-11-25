using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Checkers;
using System.Drawing.Printing;
using Unity.VisualScripting;

namespace CheckersEditor
{
#if UNITY_EDITOR
    public class WindowCreation : EditorWindow
    {
        string arrayString = "";
        private GameObject player1;
        private GameObject player2;

        [MenuItem("BoardGameWindow/Checkers")]
        public static void OpenCheckersWindow()
        {
            GetWindow(typeof(WindowCreation)).Show();

        }

        private void OnGUI()
        {

            EditorGUILayout.BeginHorizontal();
            player1 = (GameObject)EditorGUILayout.ObjectField(player1, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            player2 = (GameObject)EditorGUILayout.ObjectField(player2, typeof(GameObject), true);
            EditorGUILayout.EndHorizontal();

            PrinteBoardIndexes();

            PlayerChipsCount(player: player1? player1.GetComponent<Player>(): null);
            PlayerChipsCount(player: player2? player2.GetComponent<Player>(): null);
        }

        private void OnInspectorUpdate()
        {
            
            Repaint();
        }

        private void PrinteBoardIndexes()
        {

            for (int i = 0; i < CheckersBoard.rowsAndCols; i++)
            {
                arrayString = "";
                for (int j = 0; j < CheckersBoard.rowsAndCols; j++)
                {
                    arrayString += string.Format("{0}       ", CheckersBoard.BOARD_INDEXES[i, j]);
                }
                //arrayString += System.Environment.NewLine + System.Environment.NewLine;
                EditorGUILayout.LabelField(arrayString);
            }

        }

        private void PlayerChipsCount(Player player)
        {
            if(player)
                EditorGUILayout.LabelField($"{player.name} chips count: {player.playerChips.Count}");
            else
                EditorGUILayout.LabelField("Missing a player script");
        }
    }
#endif
}
