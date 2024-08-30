using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryEventHandler : MonoBehaviour
{
    private static InventoryEventHandler instance = null;
    public static InventoryEventHandler Instance { get; private set; }
    public MonsterInstance currentMonsterInstance { get; private set; }
    public Image carryImage { get; private set; }
    public RectTransform rectTransform { get; private set; }
    public Canvas canvas { get; private set; }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        carryImage = GetComponent<Image>();
        carryImage.gameObject.SetActive(false);
        rectTransform = carryImage.rectTransform;
        canvas = this.GetComponent<Canvas>();
    }

    public void BeginDrag(MonsterItemSlot slot)
    {
        carryImage.gameObject.SetActive(true);
        carryImage.sprite = slot.monsterImage.sprite;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void EndDrag()
    {
        carryImage.gameObject.SetActive(false);
        currentMonsterInstance = null;
    }
}
