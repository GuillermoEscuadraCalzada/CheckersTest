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
        string arrayString = "";

        [MenuItem("BoardGameWindow/Checkers")]
        public static void OpenCheckersWindow()
        {
            GetWindow(typeof(WindowCreation)).Show();

        }

        private void OnGUI()
        {
            PrinteBoardIndexes();           
        }

        private void PrinteBoardIndexes()
        {
            
            for (int i = 0; i < CheckersBoard.rowsAndCols; i++)
            {
                arrayString = "";
                for (int j = 0; j < CheckersBoard.rowsAndCols; j++)
                {
                    arrayString += string.Format("{0}       ",CheckersBoard.BOARD_INDEXES[i, j]);
                }
                //arrayString += System.Environment.NewLine + System.Environment.NewLine;
                EditorGUILayout.LabelField(arrayString);
            }

        }
    }
}
