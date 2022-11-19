using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Checkers;
using System.Drawing.Printing;
using Unity.VisualScripting;

namespace CheckersEditor
{
    public class WindowCreation : EditorWindow
    {
        private CheckersBoard checkersBoard;

        [MenuItem("BoardGameWindow/Checkers")]
        public static void OpenCheckersWindow()
        {
            GetWindow(typeof(WindowCreation)).Show();

        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();            
            GUILayout.Label("BoardToCheck", EditorStyles.boldLabel);
            checkersBoard = (CheckersBoard)EditorGUILayout.ObjectField(checkersBoard, typeof(CheckersBoard), true);
            EditorGUILayout.EndHorizontal();

            PrinteBoardIndexes();

            
        }

        private void PrinteBoardIndexes()
        {
            
                string arrayString = "";
            for (int i = 0; i < CheckersBoard.rows; i++)
            {
                arrayString = "";
                for (int j = 0; j < CheckersBoard.cols; j++)
                {
                    arrayString += string.Format("{0}       ",CheckersBoard.BOARD_INDEXES[i, j]);
                }
                //arrayString += System.Environment.NewLine + System.Environment.NewLine;
                EditorGUILayout.LabelField(arrayString);
            }

        }
    }
}
