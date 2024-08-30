using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
public class FallingGenerator : MonoBehaviour
{
    public FallingObject[] spawnObjs;

    public Action endEvent;
    public bool isEnd { get; private set; }
    public static FallingGenerator Create(FallingGenerator prefab, Vector2 worldPosition, float power = 4f)
    {
        var clone = Instantiate(prefab, worldPosition, Quaternion.identity);
        clone.Spawn(power);
        clone.StartCoroutine(clone.EndRoutine());
        return clone;
    }
    private void Spawn(float power)
    {
        float angle = 0.1f;
        float angle2 = -0.1f;

        for (int i = 0; i < spawnObjs.Length; ++i)
        {
            var clone = spawnObjs[i];
            if (i == 0)
            {
                clone.Init(0f, power);
            }
            else if (i % 2 == 0)
            {
                clone.Init(angle, power);
                angle += 0.1f;
            }
            else if (i % 2 == 1)
            {
                clone.Init(angle2, power);
                angle2 -= 0.1f;
            }
        }
    }
    private IEnumerator EndRoutine()
    {
        isEnd = false;
        for (int i = 0; i < spawnObjs.Length; ++i)
            yield return new WaitUntil(() => spawnObjs[i].isEnd == true);

        if (endEvent != null)
            endEvent.Invoke();
        isEnd = true;
    }

    public void RandomDeadorChangeLayer()
    {
        if (SkillManager.Instance.rockClones.Count > 20)
        {
            Destroy(this.gameObject);
            return;
        }
        for(int i = 0; i < spawnObjs.Length; i++)
        {
            int random = Random.Range(0, 2);
            if(random == 0)
            {
                spawnObjs[i].transform.parent = null;

                var renderer = spawnObjs[i].GetComponent<SpriteRenderer>();
                renderer.sortingLayerName = "Object";
                renderer.sortingOrder = -1;
                SkillManager.Instance.rockClones.Add(spawnObjs[i]);
            }
        }

        Destroy(this.gameObject);
    }
}
