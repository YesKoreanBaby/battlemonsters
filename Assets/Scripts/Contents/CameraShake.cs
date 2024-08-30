using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShakingMode { Random, MouseDir, Left, Right }
public class CameraShake : MonoBehaviour
{
    //카메라쉐이크관련
    private float shakeTimeRemainning, shakePower, shakeFadeTime, shakeRotation;
    public float rotationMultiflier = 7.5f;
    public bool allowRotation = false;
    public ShakingMode shakingMode = ShakingMode.Random;
    public Transform target;

    private void LateUpdate()
    {
        if (shakeTimeRemainning > 0f)
        {
            shakeTimeRemainning -= Time.deltaTime;

            if (shakingMode == ShakingMode.MouseDir)
            {
                Vector2 dir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - target.transform.position).normalized;
                Camera.main.transform.transform.position += new Vector3(dir.x * shakePower, dir.y * shakePower, 0f);
            }
            else if (shakingMode == ShakingMode.Random)
            {
                float xAmount = Random.Range(-1f, 1f) * shakePower;
                float yAmount = Random.Range(-1f, 1f) * shakePower;

                Camera.main.transform.transform.position += new Vector3(xAmount, yAmount, 0f);
            }
            else if(shakingMode == ShakingMode.Left)
            {
                float value = Random.Range(0.5f, 1f);
                float yAmount = Random.Range(-0.25f, 0.25f) * shakePower;
                Camera.main.transform.transform.position += new Vector3(-1 * value * shakePower, yAmount, 0f);
            }
            else if (shakingMode == ShakingMode.Right)
            {
                float value = Random.Range(0.5f, 1f);
                float yAmount = Random.Range(-0.25f, 0.25f) * shakePower;
                Camera.main.transform.transform.position += new Vector3(value * shakePower, yAmount, 0f);
            }


            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, shakeFadeTime * rotationMultiflier * Time.deltaTime);
        }

        if (allowRotation == true)
            Camera.main.transform.rotation = Quaternion.Euler(0f, 0f, shakeRotation * Random.Range(-1f, 1f));
    }

    public void StartShake(float length, float power, bool allowRotation = false, ShakingMode shakingMode = ShakingMode.MouseDir)
    {
        shakeTimeRemainning = length;
        shakePower = power;
        this.allowRotation = allowRotation;
        this.shakingMode = shakingMode;

        shakeFadeTime = power / length;

        shakeRotation = power * rotationMultiflier;
    }

    public bool CheckEnd() { return shakeTimeRemainning <= 0f; }
}
