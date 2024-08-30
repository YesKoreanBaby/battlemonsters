using System.Collections;
using System.Linq;
using UnityEngine;

public class StarParent : MonoBehaviour
{
    public bool isRunning { get; private set; }

    public void PlayActive()
    {
        if (isRunning == false)
            StartCoroutine(ActiveRoutine());
    }
    public void PlayDontActive()
    {
        StartCoroutine(DonActiveRoutine());
    }
    private IEnumerator ActiveRoutine()
    {
        isRunning = true;

        var childs = this.transform.GetComponentsInChildren<Transform>(true).ToList();
        WaitForSeconds waitTime = new WaitForSeconds(0.25f);
        while(childs.Count > 0)
        {
            int count = Mathf.Min(childs.Count, Random.Range(2, 5));
            for(int i = 0; i < count; ++i)
                childs[i].gameObject.SetActive(true);

            childs.RemoveRange(0, count);
            yield return waitTime;
        }

        yield return new WaitForSeconds(1.5f);

        isRunning = false;
    }

    private IEnumerator DonActiveRoutine()
    {
        var childs = this.transform.GetComponentsInChildren<Animator>().ToList();
        for(int i = 0; i < childs.Count; ++i)
        {
            childs[i].Play("exit");
        }

        yield return new WaitForSeconds(0.95f);

        Destroy(this.gameObject);
    }
}
