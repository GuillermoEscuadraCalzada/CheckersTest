using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Checkers
{
    public class CameraChipSelection : MonoBehaviour
    {
        private GameObject currentHovered;
        private Chip currentSelectedChip;
        bool hasSelected = false;
        private void LateUpdate()
        {
            OnHoverEnter();
            ClickChip();
        }

        /// <summary>
        /// Mouse enter to chip layer
        /// </summary>
        private void OnHoverEnter()
        {
            //Creates a ray from mouse position in camera
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //Checks a detection of a raycast if a ray passes through a chip
            if (Physics.Raycast(ray, out RaycastHit raycast, 100.0f, 1 << Chip.ChipLayer))
            {
                //Current chip is the same the mouse is pointing at
                if (currentSelectedChip == raycast.transform.GetComponent<Chip>()) return;

                /*No sé por qué puse este código xdddd pero algo ha de ser, no se ha roto nada. Chance no sea  necesario*/
                ////the current hovered chip is different to the current selected chip
                //if(currentHovered != null && currentHovered.GetComponent<Chip>() != currentSelectedChip)
                //    currentHovered.GetComponent<Renderer>().material.color = currentHovered.GetComponent<Chip>().OriginalColor; //Set current hovered color to original color
                
                currentHovered = raycast.transform.gameObject; //Obtención del objeto que el mouse ve
                if (!currentHovered.GetComponent<Chip>()) return; //Si no tiene componente de chip, se regresa
                currentHovered.GetComponent<Renderer>().material.color = new Color(0, 1, 0); //se cambia el color del objeto
            }
            else
            { //No hay ninguna ficha con el mouse encima
                //Se pregunta si es una o si su componente es el chip seleccionado, para evitar hacer un cambio.
                if (currentHovered == null|| currentHovered.GetComponent<Chip>() == currentSelectedChip) return;
                //Se regresa el color original al material
                currentHovered.GetComponent<Renderer>().material.color = currentHovered.GetComponent<Chip>().OriginalColor;
                currentHovered = null; 
            }
        }
        
        /// <summary>
        /// Mouse clicks a chip.
        /// </summary>
        private void ClickChip()
        {
            if (!Input.GetMouseButtonDown(0)) return; //el jugador no presiona el click izquierdo  (por ahora)
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //Se crea un nuevo rayo a partir de su posición en la cámara
            //Se crea un raycast con una gran distancia.
            if (Physics.Raycast(ray, out RaycastHit raycast, 1000.0f))
            {
                //Se detecta que el objeto no tiene un componente de ficha o que el layer del objeto es de una casilla
                if (!raycast.transform.GetComponent<Chip>() || raycast.transform.gameObject.layer == 1 << Tile.TileLayer) return;
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
                    currentSelectedChip = raycast.transform.GetComponent<Chip>();
                    currentSelectedChip.GetComponent<Renderer>().material.color = currentSelectedChip.GetComponent<Chip>().OriginalColor; 
                    currentSelectedChip.ToggleAvailableTiles(true); //Turns off available tiles for this chip
                }
                currentSelectedChip.GetComponent<Renderer>().material.color = new Color(0, 0, 1);
            }
            else 
            {
                if (currentSelectedChip == null) return;
                    currentSelectedChip.GetComponent<Renderer>().material.color = currentSelectedChip.GetComponent<Chip>().OriginalColor;

            }
        }


    }
}