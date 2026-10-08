using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalMods
{
    static float neutral = 1f;
    static float advantage = 1.5f;
    static float disadvantage = 0.5f;

    public static float Modifier(ElementalTypes skillElement, ElementalTypes enemyElement)
    {
        float elementMod = neutral;

        if (skillElement == ElementalTypes.Red)
        {
            if (enemyElement == ElementalTypes.Green)
            {
                elementMod = advantage;
            }
            else if (enemyElement == ElementalTypes.Blue)
            {
                elementMod = disadvantage;
            }
        }
        else if (skillElement == ElementalTypes.Green)
        {
            if (enemyElement == ElementalTypes.Red)
            {
                elementMod = disadvantage;
            }
            else if (enemyElement == ElementalTypes.Blue)
            {
                elementMod = advantage;
            }
        }
        else if (skillElement == ElementalTypes.Blue)
        {
            if (enemyElement == ElementalTypes.Red)
            {
                elementMod = advantage;
            }
            else if (enemyElement == ElementalTypes.Green)
            {
                elementMod = disadvantage;
            }
        }

        return elementMod;
    }
}

public enum ElementalTypes
{
    None,
    Red,
    Green,
    Blue
}
