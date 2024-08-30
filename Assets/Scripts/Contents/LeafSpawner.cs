using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafSpawner : MonoBehaviour
{
    public Leaf[] leafs;
    public List<Leaf> startLeafs = new List<Leaf>();

    private bool isRunning = false;
    private void Start()
    {
        for (int i = 0; i < leafs.Length; ++i)
        {
            leafs[i].Init();
        }
    }

    private void Update()
    {
        if(isRunning == false && CombatManager.Instance.isStart == true)
            StartCoroutine(PlayRoutine());
    }

    private void OnDisable()
    {
        Stop();
    }

    private IEnumerator PlayRoutine()
    {
        isRunning = true;
        for (int i = 0; i < leafs.Length; ++i)
        {
            leafs[i].gameObject.SetActive(false);
        }

        int count = Random.Range(2, 6);

        for(int i = 0; i < count; ++i)
        {
            int index = Random.Range(0, leafs.Length);
            while (startLeafs.Contains(leafs[index]))
                index = Random.Range(0, leafs.Length);

            startLeafs.Add(leafs[index]);
            leafs[index].Play();
        }

        for(int i = 0; i < startLeafs.Count; ++i)
        {
            yield return new WaitUntil(() => startLeafs[i].isFalling == false);
        }

        startLeafs.Clear();
        isRunning = false;
    }

    private void Stop()
    {
        for (int i = 0; i < leafs.Length; ++i)
            leafs[i].Stop();
        StopAllCoroutines();
        startLeafs.Clear();
        isRunning = false;
    }
}
