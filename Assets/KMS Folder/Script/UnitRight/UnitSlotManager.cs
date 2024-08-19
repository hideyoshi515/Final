using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

// 유닛 목록에 유저가 보유중인 유닛을 표시하기 위한 스크립트
public class UnitSlotManager : MonoBehaviour
{
    public Transform contentParent; // UnitSlot들을 포함하는 부모 오브젝트
    private List<UnitData> userUnits; // 유저가 보유한 모든 유닛 데이터 목록
    private List<Image> unitImages = new List<Image>(); // 각 슬롯의 하위에 있는 유닛 이미지 컴포넌트 목록
    private List<Graphic> unitSlots = new List<Graphic>(); // 유닛 슬롯(Dropable) 
    public List<UnitDraggable> unitDraggables = new List<UnitDraggable>(); // 각 슬롯의 하위에 있는 UnitDraggable 컴포넌트 목록

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
        yield return new WaitUntil(() => UnitGameManager.Instance != null && UnitGameManager.Instance.GetUnits() != null);
        userUnits = UnitGameManager.Instance.GetUnits();
        AssignUnitsToSlots();
        UpdateDraggableStates();
    }

    private void AssignUnitsToSlots()
    {
        // 슬롯의 개수와 유저가 가진 유닛 개수 중 작은 값을 사용하여 순회
        int unitCount = Mathf.Min(unitImages.Count, userUnits.Count);

        for (int i = 0; i < unitCount; i++)
        {
            SetUnitData(unitImages[i], unitDraggables[i], userUnits[i]);
        }

        // 유저가 가진 유닛보다 슬롯이 많을 경우, 나머지 이미지 비우고 이벤트 비활성화
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
            bool isUnitSelected = UnitGameManager.Instance.IsUnitSelected(draggable.unitData);
            
            if (isUnitSelected)
            {
                draggable.SetDraggable(false);
                draggable.GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
            }
            else
            {
                draggable.SetDraggable(true);
                draggable.GetComponent<Image>().color = Color.white;
            }
        }
    }
    
    public void ResetUnitDrag()
    {
        foreach (var unitDraggable in unitDraggables)
        {
            if (unitDraggable != null)
            {
                unitDraggable.isDropped = false;
                unitDraggable.enabled = true;
                unitDraggable.GetComponent<CanvasGroup>().blocksRaycasts = true;
                
                Image originalImage = unitDraggable.GetComponent<Image>();
                if (originalImage != null)
                {
                    originalImage.color = Color.white;
                }
            }
        }
        UpdateDraggableStates();
    }
}