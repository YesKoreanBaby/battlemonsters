using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeteorSpawnPoints : MonoBehaviour
{
    public Vector2[] spawnPoints;
    public Meteor meteorPrefab;

    private List<Vector2> spawnPointClones = new List<Vector2>();

    public IEnumerator SpawnMeteoRoutine(bool isReverse, List<EntityMonster> originTargets, EntityMonster player, SkillData skillData)
    {
        List<Meteor> meteors = new List<Meteor>();
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        Meteor meteor = null;
        if (isReverse == false)
        {
            for (int i = 0; i < 24; i++)
            {
                var position = GetRandomPosition();
                meteors.Add(Instantiate(meteorPrefab, position, Quaternion.identity));
                meteor = meteors[i];
                meteor.player = player;
                meteor.skillData = skillData;

                int random = Random.Range(1, originTargets.Count);
                HashSet<EntityMonster> targets = new HashSet<EntityMonster>();
                List<EntityMonster> targetsList = originTargets.FindAll(x => x != null && x.isDead == false);
                targets.UnionWith(targetsList);
                if (targets.Count <= 0)
                    goto jump;
                else
                {
                    random = Random.Range(0, targets.Count);
                    for (int j = 0; j < random; ++j)
                        targets.Remove(targetsList[Random.Range(0, targetsList.Count)]);

                    meteor.targets = targets;
                }
                yield return waitTime;
            }
        }
        else
        {
            for (int i = 0; i < 24; i++)
            {
                var position = GetRandomPosition();
                position = new Vector2(-1 * position.x, position.y);
                var effect = Instantiate(meteorPrefab, position, Quaternion.identity);
                effect.transform.localScale = new Vector3(-1 * effect.transform.localScale.x, effect.transform.localScale.y, effect.transform.localScale.z);
                meteors.Add(effect);
                meteor = meteors[i];
                meteor.player = player;
                meteor.skillData = skillData;

                int random = Random.Range(1, originTargets.Count);
                HashSet<EntityMonster> targets = new HashSet<EntityMonster>();
                List<EntityMonster> targetsList = originTargets.FindAll(x => x != null && x.isDead == false);
                targets.UnionWith(targetsList);
                if (targets.Count <= 0)
                    goto jump;
                else
                {
                    random = Random.Range(0, targets.Count);
                    for (int j = 0; j < random; ++j)
                        targets.Remove(targetsList[Random.Range(0, targetsList.Count)]);

                    meteor.targets = targets;
                }
                yield return waitTime;
            }
        }

jump:
        yield return new WaitUntil(() => meteor == null);
    }
    private Vector2 GetRandomPosition()
    {
        if (spawnPointClones.Count <= 0)
        {
            spawnPointClones = spawnPoints.ToList();
            for (int i = 0; i < 20; ++i)
            {
                int randomIndex = Random.Range(0, spawnPointClones.Count);
                int randomIndex2 = Random.Range(0, spawnPointClones.Count);

                while (randomIndex == randomIndex2)
                {
                    randomIndex2 = Random.Range(0, spawnPointClones.Count);
                }

                var tmp = spawnPointClones[randomIndex];
                spawnPointClones[randomIndex] = spawnPointClones[randomIndex2];
                spawnPointClones[randomIndex2] = tmp;
            }
        }

        var value = spawnPointClones[0];
        spawnPointClones.RemoveAt(0);

        return value;
    }
}
