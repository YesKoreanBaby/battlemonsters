using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScollBackground : MonoBehaviour
{
    [SerializeField] RawImage rawImage;
    [SerializeField] float xSpeed, ySpeed;
    void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.position + new Vector2(xSpeed, ySpeed) * Time.deltaTime, rawImage.uvRect.size);
    }
}
