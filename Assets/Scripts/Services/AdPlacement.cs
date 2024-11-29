using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdPlacement
{
    private int _value;
    public static implicit operator AdPlacement(int value)
    {
        return new AdPlacement(value);
    }
    public static implicit operator int(AdPlacement value)
    {
        if (value == null)
            return 0;
        return value._value;
    }
    public static bool operator ==(AdPlacement one, AdPlacement two)
    {
        if (ReferenceEquals(one, null) && ReferenceEquals(two, null))
            return true;
        else if (ReferenceEquals(one, null) || ReferenceEquals(two, null))
            return false;
        return one._value == two._value;
    }
    public static bool operator !=(AdPlacement one, AdPlacement two)
    {
        if (ReferenceEquals(one, null) && ReferenceEquals(two, null))
            return false;
        else if (ReferenceEquals(one, null) || ReferenceEquals(two, null))
            return true;
        return one._value != two._value;
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(obj, null))
            return false;
        var type = obj.GetType();
        if (type.Equals(1.GetType()) || type.Equals(typeof(AdPlacement)))
        {
            var result = obj as AdPlacement;
            return this == result;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public static AdPlacement Banner = 0;
    public static AdPlacement BannerBottom = 1;
    public static AdPlacement BannerTop = 2;
    public static AdPlacement BannerCollapsible = 3;
    public static AdPlacement Interstitial = 100;
    public static AdPlacement Reward = 200;
    public static AdPlacement AppOpen = 300;
    public static AdPlacement AppResume = 301;

    public AdPlacement(int value)
    {
        this._value = value;
    }
}
