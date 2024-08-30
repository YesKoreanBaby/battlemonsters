using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class ItemInfoSlot : MonoBehaviour, IPointerDownHandler
{
    [System.NonSerialized]
    public ItemElementalData itemData;

    [System.NonSerialized]
    public int index;

    private Image itemImage;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (itemData != null)
        {
            SoundManager.Instance.PlayEffect(166, 1f);

            var obj = this.transform.GetChild(0).gameObject;

            if (obj.activeInHierarchy == false)
            {
                if (ItemInventoryUI.Instance.currentSelectBox != null)
                {
                    ItemInventoryUI.Instance.currentSelectBox.SetActive(false);
                    ItemInventoryUI.Instance.currentSelectBox = obj;
                }
                else
                    ItemInventoryUI.Instance.currentSelectBox = obj;

                ItemInventoryUI.Instance.UpdateSellItemButton(index);

                obj.gameObject.SetActive(true);
                ItemInventoryUI.Instance.SetView(this);
            }
            else
                AlarmUI.Instance.PopUpForKey("consumitem", SetEffect);
        }
    }
    public void UpdateSlot(ItemElementalData itemData)
    {
        if (itemImage == null)
            itemImage = GetComponent<Image>();

        if(itemData != null)
        {
            this.gameObject.SetActive(true);
            itemImage.sprite = itemData.itemImage;
            this.itemData = itemData;
        }
        else
        {
            this.gameObject.SetActive(false);
            this.itemData = null;
        }
    }
    private void SetEffect()
    {
        int check = 0;
        if (itemData.effectType == ItemElementalEffectType.RecoveryHp)
        {
            var monsterInstance = TranningUI.Instance.currentMonsterInstance;
            if (monsterInstance.heathState == MonsterHeathState.Faint)
            {
                monsterInstance.hp = monsterInstance.maxHp;
                monsterInstance.heathState = MonsterHeathState.None;
            }
            else
                monsterInstance.hp = monsterInstance.maxHp;

            check = 1;
        }
        else if (itemData.effectType == ItemElementalEffectType.RecoveryStatus)
        {
            var monsterInstance = TranningUI.Instance.currentMonsterInstance;
            if (monsterInstance.heathState == MonsterHeathState.Faint)
            {
                monsterInstance.hp = Mathf.CeilToInt(monsterInstance.maxHp * 0.1f);
                monsterInstance.heathState = MonsterHeathState.None;

                check = 1;
            }
            else if (monsterInstance.heathState == MonsterHeathState.FullCondition)
                check = -1;
            else
            {
                monsterInstance.heathState = MonsterHeathState.None;
                check = 1;
            }
        }
        else if (itemData.effectType == ItemElementalEffectType.FullCondition)
        {
            var monsterInstance = TranningUI.Instance.currentMonsterInstance;
            if (monsterInstance.heathState == MonsterHeathState.None)
            {
                check = 1;
                monsterInstance.heathState = MonsterHeathState.FullCondition;
            }
            else
                check = -1;
        }
        else if (itemData.effectType == ItemElementalEffectType.Evolution)
        {
            var monsterInstance = TranningUI.Instance.currentMonsterInstance;
            var nextMonsterInstance = monsterInstance.Evolution();
            if (nextMonsterInstance != null)
            {
                int index = TranningUI.Instance.playerInventory.FindIndex(monsterInstance);

                TranningUI.Instance.playerInventory.monsterDatas[index] = nextMonsterInstance;

                check = 1;
            }
            else
                check = -1;
        }
        else if (itemData.effectType == ItemElementalEffectType.Dead)
        {
            var monsterInstance = TranningUI.Instance.currentMonsterInstance;
            if (monsterInstance.monsterData.isDeadLock == false)
            {
                var nextMonsterInstance = MonsterInstance.Instance(MonsterDataBase.Instance.deadTable[Random.Range(0, MonsterDataBase.Instance.deadTable.Length)]);
                nextMonsterInstance.previousMonsterData = monsterInstance;
                int index = TranningUI.Instance.playerInventory.FindIndex(monsterInstance);
                TranningUI.Instance.playerInventory.monsterDatas[index] = nextMonsterInstance;

                check = 1;
            }
            else
                check = -1;
        }
        else if (itemData.effectType == ItemElementalEffectType.Karma)
        {
            var monsterInstance = TranningUI.Instance.currentMonsterInstance;
            var previousMonsterInstance = monsterInstance.Karma();
            if (previousMonsterInstance != null)
            {
                int index = TranningUI.Instance.playerInventory.FindIndex(monsterInstance);
                TranningUI.Instance.playerInventory.monsterDatas[index] = previousMonsterInstance;

                check = 1;
            }
            else
                check = -1;
        }
        else if (itemData.effectType == ItemElementalEffectType.TranningBook)
        {
            var monsterInstance = TranningUI.Instance.currentMonsterInstance;
            float hp, mp, atk, def, dex, hrc, mrc, cri, ddg;
            itemData.GetTranningData(out hp, out mp, out atk, out def, out dex, out hrc, out mrc, out cri, out ddg);

            monsterInstance.maxHp += hp;
            if (monsterInstance.heathState != MonsterHeathState.Faint)
                monsterInstance.hp = Mathf.Min(monsterInstance.maxHp, monsterInstance.hp + hp);

            monsterInstance.maxMp += mp;
            monsterInstance.mp = Mathf.Min(monsterInstance.maxMp, monsterInstance.mp + mp);

            monsterInstance.atk += atk;

            monsterInstance.def += def;

            monsterInstance.maxDex = Mathf.Max(0.1f, monsterInstance.maxDex - dex);

            monsterInstance.manaRecoveryRatio = Mathf.Min(1f, monsterInstance.manaRecoveryRatio + mrc);

            monsterInstance.hpRecoveryRatio = Mathf.Min(1f, monsterInstance.hpRecoveryRatio + hrc);

            monsterInstance.creaticalRatio = Mathf.Min(1f, monsterInstance.creaticalRatio + cri);

            monsterInstance.repeatRatio = Mathf.Min(1f, monsterInstance.repeatRatio + ddg);

            check = 1;
        }
        else if(itemData.effectType == ItemElementalEffectType.Combine)
        {
            string text = TextManager.Instance.language == SystemLanguage.Korean ? "<b><size=38>몬스터 조합에 사용되는 \r\n\r\n아이템입니다\r\n\r\n<color=black><b><size=30>(SHOP -> 몬스터 -> 몬스터 조합)" : "<b><size=42>This item is used in \r\n\r\nmonster combinations.\r\n\r\n<color=black><b><size=28>(SHOP -> Monster -> Combination)";
            AlarmUI.Instance.PopUp(text, null);
            check = 2;
        }
        else if (itemData.effectType == ItemElementalEffectType.None)
            check = -1;

        if (TranningUI.Instance.currentActiveBox != null)
        {
            TranningUI.Instance.currentActiveBox.gameObject.SetActive(false);
            TranningUI.Instance.currentActiveBox = null;
        }


        if(check != 2)
        {
            TranningUI.Instance.UpdateMonsterStatus();
            CombatUI.Instance.UpdateMonsterImages();

            this.gameObject.SetActive(false);
            this.itemData = null;
            ItemInventory.Instance.RemoveConsumItem(index);

            ItemInventoryUI.Instance.Closed();

            if (AlarmUI.Instance.isPopUp == true)
                AlarmUI.Instance.Closed();

            if (check == -1)
                TranningUI.Instance.Invoke("SetFailed", 0.1f);
            else if (check == 1)
                TranningUI.Instance.Invoke("SetItemEffect", 0.1f);
        }
    }
}
