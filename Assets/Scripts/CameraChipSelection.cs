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

        private void OnHoverEnter()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycast, 100.0f, 1 << Chip.ChipLayer))
            {
                if (currentSelectedChip == raycast.transform.GetComponent<Chip>()) return;

                if(currentHovered != null && currentHovered.GetComponent<Chip>() != currentSelectedChip)
                    currentHovered.GetComponent<Renderer>().material.color = currentHovered.GetComponent<Chip>().OriginalColor;
                
                currentHovered = raycast.transform.gameObject;
                if (!currentHovered.GetComponent<Chip>()) return;
                currentHovered.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
            }
            else
            {
                if (currentHovered == null|| currentHovered.GetComponent<Chip>() == currentSelectedChip) return;
                currentHovered.GetComponent<Renderer>().material.color = currentHovered.GetComponent<Chip>().OriginalColor;
                currentHovered = null;
            }
        }
        
        private void ClickChip()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycast, 1000.0f))
            {
                if (!raycast.transform.GetComponent<Chip>()) return;
                if (raycast.transform.gameObject != currentSelectedChip)
                {
                    if(currentSelectedChip != null)
                        currentSelectedChip.GetComponent<Renderer>().material.color = currentSelectedChip.GetComponent<Chip>().OriginalColor;
                    currentSelectedChip = raycast.transform.GetComponent<Chip>();
                    currentSelectedChip.GetComponent<Renderer>().material.color = currentSelectedChip.GetComponent<Chip>().OriginalColor; 
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