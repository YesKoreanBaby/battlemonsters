using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtentions
{
    // ��簪 �÷� ��ȯ( �ڵ� ���� : RGBA )
    public static Color GetHexColor(string hexCode)
    {
        Color color;
        if (ColorUtility.TryParseHtmlString(hexCode, out color))
        {
            return color;
        }

        Debug.LogError("[UnityExtension::HexColor]invalid hex code - " + hexCode);
        return Color.white;
    }
}
