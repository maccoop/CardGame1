using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScriptHelper
{ 
    public static string FormatHelper(this string sentences,params object[] parameters)
    {
        return string.Format(sentences, parameters);
    }

    public static bool RangeIn(this int value, int min, int max)
    {
        return value >= min && value <= max;
    }
}
