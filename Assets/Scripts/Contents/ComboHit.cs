using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboHit : MonoBehaviour
{
    private Animator[] effects;
    void Start()
    {
        effects = GetComponentsInChildren<Animator>();
    }

    public IEnumerator PlayRoutine(float dmg , BattleStructure structure)
    {
        //타겟가져오기
        var targets = CombatManager.Instance.GetSortTarget(CombatManager.Instance.homegroundMonsters.Contains(structure.player), structure.skillData.targetLayer);
        var target = CombatManager.Instance.GetDetailTarget(structure.player.battleInstance.currentSelectDetailTargetType, structure.skillData, targets);
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);

        for(int i = 0; i < effects.Length; ++i)
        {
            effects[i].Play("exit");

            var checkDmg = target.DmgCheck(structure.player, structure.skillData);

            if (checkDmg == true)
            {
                target.Hurt(dmg, structure.player, structure.skillData);
            }
            yield return new WaitUntil(() => effects[i] == null);
            yield return waitTime;
        }
    }
}
