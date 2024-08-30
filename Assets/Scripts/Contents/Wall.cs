using System.Collections;
using System.Data;
using UnityEngine;
using UnityEngine.Events;

public class Wall : MonoBehaviour
{
    public WallName wallType;
    public int maxHp = 0;

    private Animator anim;
    private BoxCollider2D boxCol;

    [System.NonSerialized]
    public int index = 0;

    [System.NonSerialized]
    public bool isHomeground;

    private HurtObject hurtObject;
    private int hp = 0;
    private void Start()
    {
        anim = GetComponent<Animator>();
        boxCol = GetComponent<BoxCollider2D>();

        if(maxHp > 0)
        {
            hurtObject = GetComponent<HurtObject>();
            hp = maxHp;
            hurtObject.HurtStart();
            hurtObject.SetHitMaterial("FlashWhite_wall");
        }
    }

    private void OnDestroy()
    {
        if (wallType == WallName.Bounce)
            SoundManager.Instance.StopEffect(62); //바람
        else if (wallType == WallName.DakeHoleRemove)
            SoundManager.Instance.StopEffect(140); // 어둠
        else if (wallType == WallName.IceBite)
            SoundManager.Instance.StopEffect(12); // 아이스
        else if (wallType == WallName.ElectronicBite)
            SoundManager.Instance.StopEffect(53); //전기
        else if (wallType == WallName.PoisonBite)
            SoundManager.Instance.StopEffect(75); //독
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((SkillManager.Instance.stopStructure.isHomeground != isHomeground))
        {
            if(SkillManager.Instance.stopStructure.isBlock == false)
            {
                var entityObj = collision.GetComponent<EntityMonster>();
                if (entityObj != null && entityObj.originMonster != null)
                {
                    Remove();
                    return;
                }
                   
                if (wallType == WallName.OneBreak)
                {
                    SkillManager.Instance.Stop(wallType);

                    if (maxHp > 0)
                    {
                        hp = Mathf.Max(0, hp - 1);
                        hurtObject.SetTime(Color.white, Color.white);
                        if (hp <= 0)
                        {
                            anim.Play("exit");
                            boxCol.enabled = false;
                            CombatManager.Instance.trapDic.Remove(index);
                        }
                        else
                        {
                            SoundManager.Instance.PlayEffect(34, 1f);
                        }
                    }
                    else
                    {
                        anim.Play("exit");
                        boxCol.enabled = false;
                        CombatManager.Instance.trapDic.Remove(index);
                    }
                }
                else if (wallType == WallName.Bounce)
                {
                    if (SkillManager.Instance.isBounceBlock == false)
                    {
                        SkillManager.Instance.isBlock = false;
                        SkillManager.Instance.isBounceBlock = true;
                        if (maxHp > 0)
                        {
                            hp = Mathf.Max(0, hp - 1);
                            hurtObject.SetTime(Color.white, Color.white);
                            if (hp <= 0)
                            {
                                anim.Play("exit");
                                boxCol.enabled = false;
                                CombatManager.Instance.trapDic.Remove(index);
                            }
                        }
                        else
                        {
                            anim.Play("exit");
                            boxCol.enabled = false;
                            CombatManager.Instance.trapDic.Remove(index);
                        }
                    }
                }
                else if(wallType == WallName.DakeHoleRemove)
                {
                    if (entityObj != null)
                    {
                        SkillManager.Instance.collisionDarkHole = this;
                    }

                    SkillManager.Instance.Stop(wallType);
                }
                else
                {
                    if (entityObj != null)
                    {
                        SkillManager.Instance.Stop(wallType);
                        if (maxHp > 0)
                        {
                            hp = Mathf.Max(0, hp - 1);
                            hurtObject.SetTime(Color.white, Color.white);
                            if (hp <= 0)
                            {
                                anim.Play("exit");
                                boxCol.enabled = false;
                                CombatManager.Instance.trapDic.Remove(index);
                            }
                        }
                        else
                        {
                            anim.Play("exit");
                            boxCol.enabled = false;
                            CombatManager.Instance.trapDic.Remove(index);
                        }
                    }
                }
            }
            else
            {
                anim.Play("exit");
                boxCol.enabled = false;
                CombatManager.Instance.trapDic.Remove(index);
                SkillManager.Instance.stopStructure.isBlock = false;
            }
        }
    }

    public void Remove()
    {
        anim.Play("exit");
        boxCol.enabled = false;
        CombatManager.Instance.trapDic.Remove(index);
    }

    private void Update()
    {
        if (hurtObject != null)
            hurtObject.HurtUpdate(false);
    }
}
