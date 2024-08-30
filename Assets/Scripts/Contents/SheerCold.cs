using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SheerCold : MonoBehaviour
{
    public Animator sheercold_hit;

    List<Animator> sheercold_hitClones = null;

    public bool isRunning { get; private set; }
    public IEnumerator SheercoldRoutine(float minXPosition, float maxXPosition, float currentYPosition)
    {
        isRunning = true;

        sheercold_hitClones = new List<Animator>();
        float lerpSpeed = 1f;
        float currentTime = 0f;
        float lerpTime = 6 / CombatManager.Instance.timeScale;

        Vector2 startPosition = new Vector2(minXPosition, currentYPosition);
        Vector2 endPosition = new Vector2(maxXPosition, currentYPosition - 0.1f);

        float addValue = 1;
        float createPointTime = lerpTime * (0.05f * addValue);

        float yOffset = Random.Range(-0.05f, 0.05f);
        Animator current = Instantiate(sheercold_hit, startPosition + new Vector2(0f, yOffset), Quaternion.identity);
        sheercold_hitClones.Add(current);
        while (currentTime < lerpTime)
        {
            currentTime += Time.deltaTime * lerpSpeed;

            float currentSpeed = currentTime / lerpTime;
            this.transform.position = Vector3.Lerp(startPosition, endPosition, currentSpeed);

            if(currentTime >= createPointTime)
            {
                addValue += 1;
                createPointTime = lerpTime * (0.1f * addValue);

                yOffset = Random.Range(-0.05f, 0.05f);

                current = Instantiate(sheercold_hit, this.transform.position + new Vector3(0f, yOffset), Quaternion.identity);
                sheercold_hitClones.Add(current);
            }
            yield return null;
        }

        yield return new WaitUntil(() => current.GetCurrentAnimatorStateInfo(0).IsName("stay") == true);
        isRunning = false;
    }

    public IEnumerator SheercoldExitRoutine(BattleStructure structure, List<EntityMonster> targets)
    {
        isRunning = true;
        Animator clone = null;

        for(int i = 0; i < sheercold_hitClones.Count; ++i)
        {
            clone = sheercold_hitClones[i];
            sheercold_hitClones[i].Play("exit");
        }

        yield return new WaitForSeconds(1f);
        for(int i = 0; i < targets.Count; ++i)
        {
            targets[i].Hurt(structure.skillData.atk);
            //얼음상태
            if (targets[i] != null && targets[i].isDead == false)
            {
                float value = Random.Range(0f, 1f);
                if (value <= structure.skillData.statusRatio)
                    targets[i].sternStatus.Start(targets[i], SkillManager.Instance.iceCrystalEffect, targets[i].center, 10f);
            }
        }
        yield return new WaitUntil(() => clone == null);
      
        isRunning = false;
    }
}
