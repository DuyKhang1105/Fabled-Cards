using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameCardConfig", menuName = "ScriptableObjects/GameCardConfig", order = 1)]
public class GameCardConfig : ScriptableObject
{
    public List<BaseCardConfig> commonCards;
    public List<BaseCardConfig> rareCards;
    public List<BaseCardConfig> epicCards;
    
    public BaseCardConfig GetCardById(string id)
    {
        foreach (var card in commonCards)
        {
            if (card.id == id)
            {
                return card;
            }
        }
        
        foreach (var card in rareCards)
        {
            if (card.id == id)
            {
                return card;
            }
        }
        
        foreach (var card in epicCards)
        {
            if (card.id == id)
            {
                return card;
            }
        }
        
        return null;
    }
}