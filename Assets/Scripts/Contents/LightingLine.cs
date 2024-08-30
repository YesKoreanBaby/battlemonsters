using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingLine : MonoBehaviour
{
    public static GameObject Create(Transform obj, Transform obj2)
    {
        var clone = Instantiate(SkillManager.Instance.lightingLine);
        bool check = clone.SetLine(obj, obj2);

        if (check == false)
        {
            Destroy(clone.gameObject);
            return null;
        }
        else
            return clone.gameObject;
    }
    private bool SetLine(Transform obj, Transform obj2)
    {
        if (obj == null || obj2 == null)
            return false;
        transform.position = obj.transform.position;

        Vector2 dir = (obj2.position - obj.position).normalized;
        float degree = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, degree);

        float length = (obj2.position - obj.position).magnitude;


        //64«»ºø¿Ø¥÷¿∫ 0.2f 16«»ºø¿Ø¥÷¿Ã¥—±Ó 0.2 / 4 = 0.05
        float value = length * 0.05f;
        transform.localScale = new Vector2(value, value);

        return true;
    }
}
