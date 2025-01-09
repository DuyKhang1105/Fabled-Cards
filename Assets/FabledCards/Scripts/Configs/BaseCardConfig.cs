using System;
using UnityEngine;

[Serializable]
public class BaseCardConfig
{
    public string id;
    public string cardName;
    public int rarity;
    public int Color;
}

public enum ColorType
{
    Red,
    Green,
    Blue,
    Yellow,
    Purple,
}
