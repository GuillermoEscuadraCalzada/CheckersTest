using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NRKernal;

namespace Checkers
{
    public class CameraChipSelection : MonoBehaviour
    {
        public GameObject currentHovered; //The current hovered object

        CheckersBoard checkersBoard; //A reference to the instance of the checkers board

        private void Start()
        {
            checkersBoard = CheckersBoard.Instance; //Gets the instance of the board
        }

        /// <summary>
        /// Mouse enter to chip then highlights it
        /// </summary>
        /// <param name="hoverToggleOn">True if the chip needs to highlight or return to original material color</param>
        public void OnHoverEnter(Chip hoveredChip, bool hoverToggleOn = true)
        {
            if (hoveredChip.ChipPlayerValue != checkersBoard.CurrentPlayer.PlayerNumber
            || !hoveredChip.CanMove)
                return;
            if (hoverToggleOn)
            {
                //Current chip is the same the mouse is pointing at
                if (checkersBoard.CurrentPlayer.SelectedChip == hoveredChip)
                    return;
                currentHovered = hoveredChip.gameObject; //Gets game object of raycast target
                currentHovered.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
                return;
            }
            //current hovered is null or chip component equals to selected chip
            if (!currentHovered || currentHovered.GetComponent<Chip>() == checkersBoard.CurrentPlayer.SelectedChip)
                return;
            //current hovered color returns to original.
            currentHovered.GetComponent<Renderer>().material.color = currentHovered.GetComponent<Chip>().OriginalColor;
            currentHovered = null; //sets hovered to null

        }
        
        /// <summary>
        /// Mouse clicks a chip.
        /// </summary>
        public void ClickChip(Chip clickedChip)
        {

            Chip currentSelectedChip = checkersBoard.CurrentPlayer.SelectedChip; //gets selected chip of currentPlayer

            //Checks if the raycast detected a chip and if the chip belongs to the currentPlayer
            if (clickedChip.ChipPlayerValue != checkersBoard.CurrentPlayer.PlayerNumber
                || !clickedChip.CanMove)
                return;

            //game object is different to current selected chip
            if (clickedChip != currentSelectedChip)
            {
                //Current selected chip is not null
                if(currentSelectedChip != null)
                {
                    //Material's color returns to original color
                    currentSelectedChip.GetComponent<Renderer>().material.color = currentSelectedChip.GetComponent<Chip>().OriginalColor;
                    currentSelectedChip.ToggleAvailableTiles(false); //Turns off available tiles for this chip
                }
                checkersBoard.CurrentPlayer.SelectedChip = currentSelectedChip = clickedChip;                  
                currentSelectedChip.GetComponent<Renderer>().material.color = currentSelectedChip.OriginalColor; 
                currentSelectedChip.AvailableTilesToMove();
                currentSelectedChip.ToggleAvailableTiles(true); //Turns off available tiles for this chip
                currentSelectedChip.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
            }
        }

        /// <summary>
        /// The camera points to a tile with a selected chip
        /// </summary>
        public void TileSelection(Tile selectedTile)
        {
            Chip currentChip = checkersBoard.CurrentPlayer.SelectedChip;
            //if(!Input.GetMouseButtonDown(0) || !checkersBoard.CurrentPlayer.SelectedChip) return;
            if (!currentChip) return;

    
                if(!currentChip || !selectedTile) return; //there is no currentSelectedChip, therefore, can't select tile to move
            currentChip.ToggleAvailableTiles(false);
            //Moves chip to the selected tile
            currentChip.MoveToTile(selectedTile);
            currentChip.GetComponent<Renderer>().material.color = currentChip.OriginalColor;
            checkersBoard.CurrentPlayer.SelectedChip = null;
            
        }
    }
}