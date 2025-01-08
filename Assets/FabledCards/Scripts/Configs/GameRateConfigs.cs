using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RateConfigs", menuName = "ScriptableObjects/GameRateConfigs")]
public class GameRateConfigs : ScriptableObject
{
    [SerializeField] private float[] combineRateDefaultConfig;
    [SerializeField] private List<CombineRateConfig> combineRateConfigs;
    
    public float[] GetCombineRateConfigByScore(int score)
    {
        int index = score - 2;
        
        if (index < 0 || index >= combineRateConfigs.Count)
        {
            return combineRateDefaultConfig;
        }
        
        return combineRateConfigs[index].RarityChances;
    }
}
