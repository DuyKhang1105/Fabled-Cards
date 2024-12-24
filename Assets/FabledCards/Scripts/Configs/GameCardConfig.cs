using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameCardConfig", menuName = "ScriptableObjects/GameCardConfig", order = 1)]
public class GameCardConfig : ScriptableObject
{
    public List<BaseCardConfig> commonCards;
    public List<BaseCardConfig> rareCards;
    public List<BaseCardConfig> epicCards;
}