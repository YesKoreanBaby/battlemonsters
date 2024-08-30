using UnityEngine;

[CreateAssetMenu(fileName = "PercentSkillData", menuName = "Data/PercentSkillData")]
public class PercentSkillData : ScriptableObject
{
    public SkillData skillData;

    [Range(0, 100)]
    public int percent = 0;
}
