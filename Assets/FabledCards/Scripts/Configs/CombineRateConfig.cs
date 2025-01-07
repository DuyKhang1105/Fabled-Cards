using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombineRateConfig
{
    [SerializeField] private float[] rarityChances = { 0f, 0f, 0f };
    
    public float[] RarityChances => rarityChances;
}
