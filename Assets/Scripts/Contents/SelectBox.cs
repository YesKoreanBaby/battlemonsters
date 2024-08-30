using UnityEngine;

public class SelectBox : MonoBehaviour
{
    public Sprite onSelect;
    public Sprite offSelect;

    [System.NonSerialized]
    public Collider2D target;

    [System.NonSerialized]
    public Vector3 fixedPosition;

    [System.NonSerialized]
    public int width;

    [System.NonSerialized]
    public int height;

    [System.NonSerialized]
    public bool isBlock;

    private SpriteRenderer spriteRenderer;

    [System.NonSerialized]
    public bool isInit;
    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (isBlock == false)
            spriteRenderer.sprite = offSelect;
    }

    // Update is called once per frame

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision == target)
        {
            if (isBlock == false)
            {
                foreach(var box in CombatManager.Instance.selectBoxies)
                {
                    if(box.isBlock == false)
                    {
                        box.spriteRenderer.sprite = offSelect;
                        isInit = false;
                    }
                }

                spriteRenderer.sprite = onSelect;
                isInit = true;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision == target)
        {
            if (isBlock == false)
            {
                spriteRenderer.sprite = offSelect;
                isInit = false;
            }
        }
    }
    public void Block()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (isBlock == false)
            spriteRenderer.sprite = null;

        isBlock = true;
        isInit = false;
    }

    public void UnBlock()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (isBlock == true)
            spriteRenderer.sprite = offSelect;

        isBlock = false;
        isInit = false;
    }
}
