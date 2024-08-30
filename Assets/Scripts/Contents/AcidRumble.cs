using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class AcidRumble : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    public IEnumerator AcidRumbleRoutine(List<EntityMonster> targets, BattleStructure battleStructure, WaitForSeconds waitTime ,bool isHomeground)
    {
        float offsetValue = Random.Range(-0.75f, 0.75f);
        bool palseHomeground = !isHomeground;
        var position_0 = CombatManager.Instance.GetPosition(palseHomeground, 0, 0);
        position_0 = new Vector2(position_0.x + offsetValue, position_0.y);

        var position_1 = CombatManager.Instance.GetPosition(palseHomeground, 0, 1);
        position_1 = new Vector2(position_1.x + offsetValue, position_1.y);

        var position_2 = CombatManager.Instance.GetPosition(palseHomeground, 0, 2);
        position_2 = new Vector2(position_2.x + offsetValue, position_2.y);

        var position_3 = CombatManager.Instance.GetPosition(palseHomeground, 1, 0);
        position_3 = new Vector2(position_3.x + offsetValue, position_3.y);

        var position_4 = CombatManager.Instance.GetPosition(palseHomeground, 1, 1);
        position_4 = new Vector2(position_4.x + offsetValue, position_4.y);

        var position_5 = CombatManager.Instance.GetPosition(palseHomeground, 1, 2);
        position_5 = new Vector2(position_5.x + offsetValue, position_5.y);

        var position_6 = CombatManager.Instance.GetPosition(palseHomeground, 2, 0);
        position_6 = new Vector2(position_6.x + offsetValue, position_6.y);

        var position_7 = CombatManager.Instance.GetPosition(palseHomeground, 2, 1);
        position_7 = new Vector2(position_7.x + offsetValue, position_7.y);

        var position_8 = CombatManager.Instance.GetPosition(palseHomeground, 2, 2);
        position_8 = new Vector2(position_8.x + offsetValue, position_8.y);

        Vector2[] positionTable = new Vector2[9] { position_0, position_1, position_2, position_3, position_4, position_5, position_6, position_7, position_8};
      
        int repeatCount = 4;
        List<Vector2> positions = positionTable.ToList();
        GameObject explositionEffect = null;
        for (int i = 0; i < repeatCount; ++i)
        {
            int randomCount = Random.Range(2, 6);
            for(int j = 0; j < randomCount; ++j)
            {
                if (positions.Count <= 0)
                    positions.AddRange(positionTable);

                int index = Random.Range(0, positions.Count);
                Vector2 randomPosition = positions[index];
                positions.RemoveAt(index);


                explositionEffect = Instantiate(explosion, randomPosition, Quaternion.identity);
            }

            yield return new WaitUntil(() => explositionEffect == null);

            for (int j = 0; j < targets.Count; ++j)
            {
                if (targets[j] == null)
                    continue;
                if (targets[j] != null && targets[j].isDead == true)
                    continue;

                bool dmgCheck = targets[j].DmgCheck(battleStructure.player, battleStructure.skillData);

                if(dmgCheck)
                {
                    int dmgPerValue = Random.Range(0, 4);
                    float value = 0f;
                    if (dmgPerValue == 0)
                        value = 0.25f;
                    else if (dmgPerValue == 1)
                        value = 0.5f;
                    else if (dmgPerValue == 2)
                        value = 0.75f;
                    else if (dmgPerValue == 3)
                        value = 1f;

                    targets[j].Hurt(value * battleStructure.skillData.atk);
                    var findObj = targets.Find(x => x != null && x.isDead == false);
                    if (findObj == null)
                        yield break;

                    targets[j].sumStatusRatio = Mathf.Min(0.9f, targets[j].sumStatusRatio + battleStructure.skillData.statusRatio);
                }
            }
        }

        var fog = transform.GetChild(0).GetComponent<Animator>();
        fog.Play("acidrumble_fog");
        yield return new WaitForSeconds((1.6f / Time.timeScale) * 2f);

        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = (1f / Time.timeScale) * 2f;

        Vector2 startPosition = fog.transform.position;
        Vector2 endPosition = startPosition + new Vector2(isHomeground ? 3.5f : -3.5f, 0f);
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            fog.transform.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);
            yield return null;
        }


        yield return new WaitForSeconds(0.5f);

        for(int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i] != null && targets[i].isDead == true)
                continue;

            explositionEffect = Instantiate(explosion, targets[i].transform.position, Quaternion.identity);
        }

        yield return new WaitUntil(() => explositionEffect == null);
        yield return waitTime;

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i] != null && targets[i].isDead == true)
                continue;

            bool dmgCheck = targets[i].DmgCheck(battleStructure.player, battleStructure.skillData);
            if (dmgCheck)
            {
    
                targets[i].CriticalHurt(battleStructure.skillData.atk, battleStructure.player, battleStructure.skillData);
                var findObj = targets.Find(x => x != null && x.isDead == false);
                if (findObj == null)
                    yield break;
            }

            targets[i].sumStatusRatio = Mathf.Min(0.9f, targets[i].sumStatusRatio + battleStructure.skillData.statusRatio);
        }

        yield return waitTime;

        bool check = false;
        GameObject clone = null;
        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i] != null && targets[i].isDead == true)
                continue;

            float value = Random.Range(0f, 1f);
            if (value <= targets[i].sumStatusRatio)
            {
                check = true;
                clone = targets[i].dogDmgStatus.AddDeadlyPoisonFlag(targets[i], SkillManager.Instance.deadlyPoisonEffect);
            }

            targets[i].sumStatusRatio = 0f;
        }

        if(check == true)
        {
            yield return new WaitUntil(() => clone == null);
            yield return waitTime;
        }

        for (int i = 0; i < targets.Count; ++i)
        {
            if (targets[i] == null)
                continue;
            if (targets[i] != null && targets[i].isDead == true)
                continue;

            clone = Instantiate(SkillManager.Instance.debuffSpeedEffect, targets[i].center.position, Quaternion.identity);

            targets[i].battleInstance.maxDex = Mathf.Min(9, targets[i].battleInstance.maxDex + (targets[i].battleInstance.maxDex * battleStructure.skillData.buffValue));
            targets[i].battleInstance.dex = Mathf.Min(targets[i].battleInstance.maxDex, targets[i].battleInstance.dex + (targets[i].battleInstance.dex * battleStructure.skillData.buffValue));
        }

        yield return new WaitUntil(() => clone == null);
    }
}
