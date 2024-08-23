using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitDropable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    private Image image;
    private RectTransform rect;
    public UnitData assignedUnitData;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            UnitDraggable draggedUnit = eventData.pointerDrag.GetComponent<UnitDraggable>();
            if (draggedUnit != null && !draggedUnit.isDropped)
            {
                image.color = Color.yellow;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            UnitDraggable draggedUnit = eventData.pointerDrag.GetComponent<UnitDraggable>();
            if (draggedUnit != null && !draggedUnit.isDropped)
            {

                image.color = Color.white;
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        UnitDraggable draggedUnit = eventData.pointerDrag.GetComponent<UnitDraggable>();

        if (draggedUnit != null)
        {
            assignedUnitData = draggedUnit.unitData;

            int placement;
            
            if (gameObject.transform.parent.name.Contains("Left Wall Stage"))
            {
                placement = 1;
            }
            else if (gameObject.transform.parent.name.Contains("Right Wall Stage"))
            {
                placement = 2;
            }
            else
            {
                placement = 0;
            }
            
            int slotIndex = transform.GetSiblingIndex();

            UnitGameManagerA.Instance.SaveUnitPlacement(slotIndex, draggedUnit.unitData, placement);

            draggedUnit.transform.SetParent(transform);
            draggedUnit.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;

            Image dropZoneImage = GetComponent<Image>();
            if (dropZoneImage != null)
            {
                dropZoneImage.sprite = draggedUnit.unitData.UnitImage;
                dropZoneImage.color = Color.white;
            }
            
            draggedUnit.transform.SetParent(draggedUnit.previousParent);
            draggedUnit.GetComponent<RectTransform>().position = draggedUnit.previousParent.GetComponent<RectTransform>().position;
            
            UnitDraggable originalDraggable = draggedUnit.previousParent.GetComponentInChildren<UnitDraggable>();
            if (originalDraggable != null)
            { 
                originalDraggable.enabled = false;
            }
            draggedUnit.isDropped = true;
        }
    }
}