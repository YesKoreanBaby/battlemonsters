using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtObject : MonoBehaviour
{
    private Material hitMaterial;
    private Material originMaterial;
    private Material previousMaterial;
    private Material previousOriginMaterial;
    private float hurtTime = 0f;
    private float hitMaterialValue = 0;
    private bool isBlock = false;
    private bool hurtStartFlag = false;
    private SpriteRenderer spriteRenderer;
  
    public void SetHitMaterial(Material material)
    {
        previousMaterial = hitMaterial;
        hitMaterial = material;
    }
    public void SetHitMaterial(string text)
    {
        previousMaterial = hitMaterial;
        hitMaterial = Resources.Load<Material>(text);
    }
    public void SetOriginMaterial(Material material)
    {
        previousOriginMaterial = originMaterial;
        originMaterial = material;
    }
    public void PreviousHitMaterial()
    {
        hitMaterial = previousMaterial;
    }
    public void PreviousOriginMaterial()
    {
        originMaterial = previousOriginMaterial;
    }
    public void SetTime(Color originColor, Color hitColor)
    {
        if (isBlock == false)
        {
            hitMaterial.SetColor("_FlashColor", hitColor);
            hitMaterial.SetColor("_Color", originColor);
            hurtTime = 1f;
        }
    }
    public void HurtStart()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        hitMaterial = Resources.Load<Material>("FlashWhite");
        originMaterial = spriteRenderer.material;
    }

    public void ResetHurt()
    {
        hitMaterial.SetFloat("_FlashAmount", 0);
        hurtStartFlag = false;
        spriteRenderer.material = originMaterial;
        hurtTime = 0f;
    }
    public void HurtUpdate(bool isBlock)
    {
        this.isBlock = isBlock;
        if (isBlock)
        {
            hitMaterial.SetFloat("_FlashAmount", 0);
            hurtStartFlag = false;
            spriteRenderer.material = originMaterial;
        }
        else
        {
            if (hurtTime > 0f)
            {
                if (hurtStartFlag == false)
                {
                    hurtStartFlag = true;
                    spriteRenderer.material = hitMaterial;
                }
                hurtTime -= Time.deltaTime * 2f;
                hitMaterialValue = Mathf.Clamp(hurtTime, 0, 1);
                hitMaterial.SetFloat("_FlashAmount", hitMaterialValue);
            }
            else
            {
                if (hurtStartFlag == true)
                {
                    hitMaterial.SetFloat("_FlashAmount", 0);
                    hurtStartFlag = false;
                    spriteRenderer.material = originMaterial;

                }
            }
        }
    }
}
