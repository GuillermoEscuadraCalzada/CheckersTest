using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public class CameraChipSelection : MonoBehaviour
    {
        private GameObject currentHovered; //The current hovered object

        CheckersBoard checkersBoard; //A reference to the instance of the checkers board

        private void Start()
        {
            checkersBoard = CheckersBoard.Instance; //Gets the instance of the board
        }

        private void LateUpdate()
        {
            OnHoverEnter();
            TileSelection();
            ClickChip();
        }

        /// <summary>
        /// Mouse enter to chip layer
        /// </summary>
        private void OnHoverEnter()
        {

            //Creates a ray from mouse position in camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Checks a detection of a raycast if a ray passes through a chip and belongs to the currentPlayer number
            if (Physics.Raycast(ray, out RaycastHit raycast, 100.0f, 1 << Chip.ChipLayer) 
                && raycast.transform.GetComponent<Chip>().ChipPlayerValue == checkersBoard.CurrentPlayer.PlayerNumber)
            {
                //Current chip is the same the mouse is pointing at
                if (checkersBoard.CurrentPlayer.SelectedChip == raycast.transform.GetComponent<Chip>())
                    return;
                currentHovered = raycast.transform.gameObject; //Gets game object of raycast target
                if (!currentHovered.GetComponent<Chip>()) return; //Target doesn't has a chip component
                currentHovered.GetComponent<Renderer>().material.color = new Color(0, 1, 0); //Change color of hovered chip
            }
            else
            { //No chip has mouse over it
                //current hovered is null or chip component equals to selected chip
                if (currentHovered == null|| currentHovered.GetComponent<Chip>() == checkersBoard.CurrentPlayer.SelectedChip)
                    return;
                //current hovered color returns to original.
                currentHovered.GetComponent<Renderer>().material.color = currentHovered.GetComponent<Chip>().OriginalColor;
                currentHovered = null; //sets hovered to null
            }
        }
        
        /// <summary>
        /// Mouse clicks a chip.
        /// </summary>
        private void ClickChip()
        {
            if (!Input.GetMouseButtonDown(0)) return; //Player didn't press left click

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Creates new ray from mouse position in camera space
            Chip currentSelectedChip = checkersBoard.CurrentPlayer.SelectedChip; //gets selected chip of currentPlayer

            if (Physics.Raycast(ray, out RaycastHit raycast))
            {
                Chip raycastChip = raycast.transform.GetComponent<Chip>();
                
                //Checks if the raycast detected a chip and if the chip belongs to the currentPlayer
                if (!raycastChip || !raycast.collider.CompareTag("Chip") 
                    || raycastChip.ChipPlayerValue != checkersBoard.CurrentPlayer.PlayerNumber
                    || !raycastChip.CanMove)
                    return;

                //Object game object is different to current selected chip
                if (raycast.transform.gameObject != currentSelectedChip)
                {
                    //Current selected chip is not null
                    if(currentSelectedChip != null)
                    {
                        //Material's color returns to original color
                        currentSelectedChip.GetComponent<Renderer>().material.color = currentSelectedChip.GetComponent<Chip>().OriginalColor;
                        currentSelectedChip.ToggleAvailableTiles(false); //Turns off available tiles for this chip
                    }
                    checkersBoard.CurrentPlayer.SelectedChip = currentSelectedChip = raycastChip;                  
                    currentSelectedChip.GetComponent<Renderer>().material.color = currentSelectedChip.GetComponent<Chip>().OriginalColor; 
                    currentSelectedChip.AvailableTilesToMove();
                    currentSelectedChip.ToggleAvailableTiles(true); //Turns off available tiles for this chip
                    currentSelectedChip.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
                }
            }
            else 
            {
                if (currentSelectedChip == null) return;
                currentSelectedChip.GetComponent<Renderer>().material.color = currentSelectedChip.GetComponent<Chip>().OriginalColor;
            }
        }

        /// <summary>
        /// The camera points to a tile with a selected chip
        /// </summary>
        private void TileSelection()
        {
            if(!Input.GetMouseButtonDown(0) || !checkersBoard.CurrentPlayer.SelectedChip) return;
            ///Creates a new ray from the position of the mouse
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Chip currentSelectedChip = checkersBoard.CurrentPlayer.SelectedChip;

            //Checks a detection of a raycast if a ray passes through a chip
            if (Physics.Raycast(ray, out RaycastHit raycast))
            { 
                if(!currentSelectedChip|| !raycast.transform.GetComponent<Tile>() || !raycast.collider.CompareTag("Tile")) return; //there is no currentSelectedChip, therefore, can't select tile to move
                currentSelectedChip.ToggleAvailableTiles(false);
                //Moves chip to the selected tile
                 currentSelectedChip.MoveToTile(raycast.transform.GetComponent<Tile>());
                currentSelectedChip.GetComponent<Renderer>().material.color = currentSelectedChip.GetComponent<Chip>().OriginalColor; 
                currentSelectedChip = null; //Set tile to null.
            }
        }
    }
}