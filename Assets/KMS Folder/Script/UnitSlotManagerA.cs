using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UnitSlotManagerA : MonoBehaviour
{
    public Transform contentParent;
    private List<UnitData> userUnits;
    private List<Image> unitImages = new List<Image>();
    private List<Graphic> unitSlots = new List<Graphic>();
    public List<UnitDraggable> unitDraggables = new List<UnitDraggable>();

    private void Awake()
    {
        for (int i = 0; i < contentParent.childCount; i++)
        {
            Transform slot = contentParent.GetChild(i);
            Image unitImage = slot.Find("Unit").GetComponent<Image>();
            UnitDraggable unitDraggable = slot.Find("Unit").GetComponent<UnitDraggable>();
            Graphic slotGraphic = slot.GetComponent<Graphic>();

            if (unitImage != null && unitDraggable != null && slotGraphic != null)
            {
                unitImages.Add(unitImage);
                unitDraggables.Add(unitDraggable);
                unitSlots.Add(slotGraphic);
            }
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() =>
            UnitGameManagerA.Instance != null && UnitGameManagerA.Instance.GetUnits() != null);
        userUnits = UnitGameManagerA.Instance.GetUnits();
        if (userUnits == null || userUnits.Count == 0)
        {
            Debug.LogError("userUnits is null or empty!");
        }
        else
        {
            AssignUnitsToSlots();
            UpdateDraggableStates();
        }
    }

    private void AssignUnitsToSlots()
    {
        int unitCount = Mathf.Min(unitImages.Count, userUnits.Count);

        for (int i = 0; i < unitCount; i++)
        {
            SetUnitData(unitImages[i], unitDraggables[i], userUnits[i]);
        }

        for (int i = unitCount; i < unitImages.Count; i++)
        {
            SetUnitData(unitImages[i], unitDraggables[i], null);
            unitSlots[i].raycastTarget = false;
        }
    }

    private void SetUnitData(Image unitImage, UnitDraggable unitDraggable, UnitData data)
    {
        if (data != null && data.UnitImage != null)
        {
            unitImage.sprite = data.UnitImage;
            unitImage.color = Color.white;
            unitImage.enabled = true;

            unitDraggable.unitData = data;
            unitDraggable.unitIndex = unitImages.IndexOf(unitImage);
            unitSlots[unitImages.IndexOf(unitImage)].raycastTarget = true;
        }
        else
        {
            unitImage.sprite = null;
            unitImage.enabled = false;
            unitDraggable.unitData = null;
            unitSlots[unitImages.IndexOf(unitImage)].raycastTarget = false;
        }
    }

    public void UpdateDraggableStates()
    {
        foreach (var draggable in unitDraggables)
        {
            if (draggable == null || draggable.unitData == null)
            {
                continue;
            }

            bool isUnitSelected = UnitGameManagerA.Instance.IsUnitSelected(draggable.unitData);

            if (isUnitSelected)
            {
                draggable.isDropped = true;
                draggable.enabled = false;
                draggable.GetComponent<CanvasGroup>().blocksRaycasts = false;
                draggable.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }
            else
            {
                draggable.isDropped = false;
                draggable.enabled = true;
                draggable.GetComponent<CanvasGroup>().blocksRaycasts = true;
                draggable.GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void ResetUnitDrag()
    {
        foreach (var unitDraggable in unitDraggables)
        {
                unitDraggable.isDropped = false;
                unitDraggable.enabled = true;
                CanvasGroup canvasGroup = unitDraggable.GetComponent<CanvasGroup>();
                if (canvasGroup != null)
                {
                    canvasGroup.blocksRaycasts = true;
                    Debug.Log($"Resetting blocksRaycasts for {unitDraggable.name} to true");
                }

                // 유닛의 시각적 상태 초기화
                Image originalImage = unitDraggable.GetComponent<Image>();
                if (originalImage != null)
                {
                    originalImage.color = Color.white;
                    Debug.Log($"Resetting color for {unitDraggable.name} to white");
                }
        }
        UpdateDraggableStates();
    }
}