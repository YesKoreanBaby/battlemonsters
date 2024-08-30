using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemElementalEffectType { RecoveryHp, FullCondition, RecoveryStatus, Dead, Evolution, Karma, TranningBook, Combine, None }

[CreateAssetMenu(fileName = "ItemElementalData", menuName = "Data/ItemElementalData")]
public class ItemElementalData : ScriptableObject
{
    public Sprite itemImage;
    public ItemElementalEffectType effectType;
    public TranningType tranningType;
    
    public void GetTranningData(out float hp, out float mp, out float atk, out float def, out float dex, out float hrc, out float mrc, out float cri, out float ddg)
    {
        hp = mp = atk = def = dex = hrc = mrc = cri = ddg = 0;

        if (tranningType == TranningType.Balance)
        {
            hp = 10;
            mp = 10;
            atk = 2;
            def = 1;
        }
        else if (tranningType == TranningType.Genius)
        {
            hp = 10;
            mp = 10;
            atk = 5;
            def = 2;

            dex = 0.25f;

            mrc = 0.1f;
            hrc = 0.1f;
            cri = 0.05f;
            ddg = 0.05f;
        }
        else if (tranningType == TranningType.Atk)
        {
            atk = 5;
        }
        else if (tranningType == TranningType.Def)
        {
            def = 5;
        }
        else if (tranningType == TranningType.Speed)
        {
            dex = 0.25f;
        }
        else if (tranningType == TranningType.Mp)
        {
            mp = 20;
        }
        else if (tranningType == TranningType.Hp)
        {
            hp = 20;
        }
    }
}
