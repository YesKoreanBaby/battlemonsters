using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float moveSpeed = 6f;
    public Transform endPoint;
    public Animator obj;
    public HashSet<EntityMonster> targets;
    public SkillData skillData;
    public EntityMonster player;
    void Start()
    {
        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        yield return new WaitUntil(() => obj.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        float m = (endPoint.position - obj.transform.position).magnitude;
        while(m > moveSpeed * Time.deltaTime)
        {
            m = (endPoint.position - obj.transform.position).magnitude;
            var dir = (endPoint.position - obj.transform.position).normalized;

            obj.transform.position += dir * moveSpeed *Time.deltaTime;
            yield return null;
        }

        obj.Play("exit");
        yield return new WaitUntil(() => obj == null);
        Destroy(this.gameObject);

        if(targets != null)
        {
            foreach (var mon in targets)
            {
                //데미지계산
                bool check = mon.DmgCheck(player, skillData);
                if (check)
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
                    mon.Hurt(Mathf.Max(mon.GetDefenseDeal(), (player.battleInstance.atk + skillData.atk) - mon.battleInstance.def) * value, player, skillData);
                    mon.checkLivingDeadHolyHurt = true;
                    mon.sumStatusRatio = Mathf.Min(0.9f, mon.sumStatusRatio + skillData.statusRatio);
                }
            }
        }
    }
}
