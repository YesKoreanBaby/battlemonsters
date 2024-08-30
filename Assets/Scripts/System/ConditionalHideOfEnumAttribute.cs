using UnityEngine;

public class ConditionalHideOfEnumAttribute : PropertyAttribute
{
    public string ConditionalSourceField;
    public string[] EnumValues;
    public bool HideInInspector;

    public ConditionalHideOfEnumAttribute(string conditionalSourceField, bool hideInInspector, params string[] enumValues)
    {
        ConditionalSourceField = conditionalSourceField;
        EnumValues = enumValues;
        HideInInspector = hideInInspector;
    }
}
