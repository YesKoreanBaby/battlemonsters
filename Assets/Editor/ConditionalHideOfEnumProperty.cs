using UnityEditor;
using UnityEngine;


[CustomPropertyDrawer(typeof(ConditionalHideOfEnumAttribute))]
public class ConditionalHideOfEnumProperty : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideOfEnumAttribute condHAtt = (ConditionalHideOfEnumAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        bool wasEnabled = GUI.enabled;
        GUI.enabled = enabled;
        if (enabled || !condHAtt.HideInInspector)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }





        GUI.enabled = wasEnabled;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideOfEnumAttribute condHAtt = (ConditionalHideOfEnumAttribute)attribute;
        bool enabled = GetConditionalHideAttributeResult(condHAtt, property);

        if (enabled || !condHAtt.HideInInspector)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
        else
        {
            return -EditorGUIUtility.standardVerticalSpacing; // ���� �� ������ �������� ����
        }
    }

    private bool GetConditionalHideAttributeResult(ConditionalHideOfEnumAttribute condHAtt, SerializedProperty property)
    {
        bool enabled = false;

        // ���Ǻ� �ʵ��� �̸��� ������� SerializedProperty�� ã���ϴ�.
        string propertyPath = property.propertyPath; // ��: myArray.data[0]
        string conditionPath = propertyPath.Replace(property.name, condHAtt.ConditionalSourceField);

        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(conditionPath);

        if (sourcePropertyValue != null)
        {
            if (sourcePropertyValue.propertyType == SerializedPropertyType.Enum)
            {
                string enumValueName = sourcePropertyValue.enumNames[sourcePropertyValue.enumValueIndex];
                foreach (string enumValue in condHAtt.EnumValues)
                {
                    if (enumValueName == enumValue)
                    {
                        enabled = true;
                        break;
                    }
                }
            }
            else
            {
                Debug.LogWarning("ConditionalHideAttribute is only supported for enum fields: " + condHAtt.ConditionalSourceField);
            }
        }
        else
        {
            Debug.LogWarning("Condition field \"" + condHAtt.ConditionalSourceField + "\" not found in object: " + condHAtt.ConditionalSourceField);
        }

        return enabled;
    }

}
