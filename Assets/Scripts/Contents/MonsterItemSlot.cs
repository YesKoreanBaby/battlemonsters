using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonsterItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler , IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public Image monsterImage { get; private set; }
    public Image hpImage { get; private set; }
    public Image heathImage { get; private set; }

    [System.NonSerialized]
    public int index;

    public MonsterInstance monsterInstance;

    private EntityMonster selectMonster;

    [System.NonSerialized]
    public bool isLock = false;

    [System.NonSerialized]
    public bool isBlock = false;

    private Image blockFilter;
    public void Init()
    {
        blockFilter = transform.GetChild(0).GetComponent<Image>();
        blockFilter.gameObject.SetActive(false);

        hpImage = transform.GetChild(1).GetChild(0).GetComponent<Image>();

        heathImage = transform.GetChild(2).GetComponent<Image>();
    }
    public void SettingData(int index, MonsterInstance monsterInstance)
    {
        monsterImage = GetComponent<Image>();
        this.index = index;
        this.monsterInstance = monsterInstance;

        this.monsterImage.sprite = monsterInstance.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

        bool check = (monsterInstance.monsterData.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (monsterInstance.monsterData.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (monsterInstance.monsterData.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
        float value = (monsterInstance.monsterWeight != MonsterWeight.Big) ? 60f : 35f;
        if (check == true)
            value = 60f;

        if(monsterInstance.monsterWeight == MonsterWeight.Middle)
        {
            var sprite = monsterInstance.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
            if (sprite.bounds.size.y * sprite.pixelsPerUnit >= 32)
                value = 35f;
        }
        monsterImage.SetNativeSize();
        monsterImage.rectTransform.sizeDelta = new Vector2(monsterImage.rectTransform.sizeDelta.x * value, monsterImage.rectTransform.sizeDelta.y * value);

        hpImage.fillAmount = monsterInstance.hp / monsterInstance.maxHp;

        Sprite heathSprite = null;
        MonsterDataBase.Instance.heathIcons.TryGetValue(monsterInstance.heathState, out heathSprite);
        if (heathSprite != null)
        {
            heathImage.gameObject.SetActive(true);
            heathImage.sprite = heathSprite;
        }
        else
            heathImage.gameObject.SetActive(false);
    }

    public void SettingDataEdit(int index, MonsterInstance monsterInstance)
    {
        monsterImage = GetComponent<Image>();
        this.index = index;
        this.monsterInstance = monsterInstance;

        this.monsterImage.sprite = monsterInstance.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;

        bool check = (monsterInstance.monsterData.monsterPrefab == MonsterDataBase.Instance.checkDrake) || (monsterInstance.monsterData.monsterPrefab == MonsterDataBase.Instance.checkBasilisk) || (monsterInstance.monsterData.monsterPrefab == MonsterDataBase.Instance.checkLivingLegend);
        float value = (monsterInstance.monsterWeight != MonsterWeight.Big) ? 60f : 35f;
        if (check == true)
            value = 60f;

        if (monsterInstance.monsterWeight == MonsterWeight.Middle)
        {
            var sprite = monsterInstance.monsterData.monsterPrefab.GetComponent<SpriteRenderer>().sprite;
            if (sprite.bounds.size.y * sprite.pixelsPerUnit >= 32)
                value = 35f;
        }
        monsterImage.SetNativeSize();
        monsterImage.rectTransform.sizeDelta = new Vector2(monsterImage.rectTransform.sizeDelta.x * value, monsterImage.rectTransform.sizeDelta.y * value);

        hpImage.fillAmount = 1f;
        heathImage.gameObject.SetActive(false);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isBlock == true)
            return;
        if (isLock == true)
            return;

        if (selectMonster != null)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);
            selectMonster.transform.position = worldPosition;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
       
    }

    public void OnEndDrag(PointerEventData eventData)
    {
     //   LongClickManager.Instance.OnPointerUp(EndDrag);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isBlock == true)
            return;
        if (isLock == true)
            return;

        SoundManager.Instance.PlayEffect(166, 1f);
        LongClickManager.Instance.OnPointerDown(BeginDrag);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isBlock == true)
            return;
        if (isLock == true)
            return;
        LongClickManager.Instance.OnPointerUp(EndDrag);
    }

    private void BeginDrag()
    {
        // Touch touch = Input.GetTouch(0);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0f);
        selectMonster = Instantiate(monsterInstance.monsterData.monsterPrefab, worldPosition, Quaternion.identity);
     //   selectMonster.battleInstance = monsterInstance;
        selectMonster.DonActiveShadowAndUI();

        var combatUI = CombatUI.Instance;
        combatUI.Blind();
        combatUI.LockSlot(this);
        combatUI.battleButton.interactable = false;
        combatUI.battleButton.transform.GetChild(0).gameObject.SetActive(false);

        
        var renderer = selectMonster.GetComponent<SpriteRenderer>();
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.75f);

        CombatManager.Instance.ActiveSelectBox(selectMonster.GetComponent<BoxCollider2D>());
        
       
    }

    private void EndDrag()
    {
        if (selectMonster != null)
        {
            var select = CombatManager.Instance.CheckSelectBox();
            if(select != null)
            {
                SoundManager.Instance.PlayEffect(167, 1f);
                select.Block();

                var clone = EntityMonster.CreateMonster(monsterInstance, select.fixedPosition, select.width, select.height, true);
                Destroy(selectMonster.gameObject);
                selectMonster = null;

                if(CombatUI.Instance.activeEdit == false)
                    CombatManager.Instance.SetFormationBuffDebuff(clone);

                CombatUI combatUI = this.transform.root.GetComponent<CombatUI>();
                combatUI.UnBlind();
                combatUI.UnLockSlot();
                combatUI.battleButton.interactable = (combatUI.saveButton.gameObject.activeInHierarchy) ? false : CombatManager.Instance.CheckPlayBattle();
                combatUI.battleButton.transform.GetChild(0).gameObject.SetActive(true);
                combatUI.tranningButton.interactable = false;

               

               
                CombatManager.Instance.InActiveSelectBox();

                Block();

                if(CombatManager.Instance.CheckMonsterCount())
                {
                   
                    var monsterItemSlots = combatUI.slots;
                    for(int i = 0; i < monsterItemSlots.Length; i++)
                    {
                        monsterItemSlots[i].Block();
                    }
                }
                else
                {
                    

                    if (CombatManager.Instance.monsterSpawnCount >= 1)
                    {
                        var monsterItemSlots = combatUI.slots;
                        for (int i = 0; i < monsterItemSlots.Length; i++)
                        {
                            if (monsterItemSlots[i].monsterInstance != null && monsterItemSlots[i].monsterInstance.monsterWeight == MonsterWeight.Big)
                                monsterItemSlots[i].Block();
                        }
                    }
                }

                
            }
            else
            {
                Destroy(selectMonster.gameObject);
                selectMonster = null;
                CombatUI combatUI = this.transform.root.GetComponent<CombatUI>();
                combatUI.UnBlind();
                combatUI.UnLockSlot();
                combatUI.battleButton.interactable = (combatUI.saveButton.gameObject.activeInHierarchy) ? false : CombatManager.Instance.CheckPlayBattle();
                combatUI.battleButton.transform.GetChild(0).gameObject.SetActive(true);
                CombatManager.Instance.InActiveSelectBox();

                
               
            }
        }
    }
    public void Block()
    {
        isBlock = true;
        blockFilter.gameObject.SetActive(true);
        heathImage.gameObject.SetActive(false);
        hpImage.transform.parent.gameObject.SetActive(false);
        if (monsterImage != null)
            monsterImage.color = Color.black;
    }
    public void UnBlock()
    {
        if(monsterInstance != null)
        {
            if (monsterInstance.heathState != MonsterHeathState.Faint)
            {
                isBlock = false;
                blockFilter.gameObject.SetActive(false);
                hpImage.transform.parent.gameObject.SetActive(true);

                if (monsterImage != null)
                    monsterImage.color = Color.white;
            }
        }
        else
        {
            isBlock = false;
            blockFilter.gameObject.SetActive(false);
            hpImage.transform.parent.gameObject.SetActive(true);
            if (monsterImage != null)
                monsterImage.color = Color.white;
        }
    }

    public void UnBlockEdit()
    {
        if (monsterInstance != null)
        {
            isBlock = false;
            blockFilter.gameObject.SetActive(false);
            hpImage.transform.parent.gameObject.SetActive(true);
            if (monsterImage != null)
                monsterImage.color = Color.white;
        }
    }
}
