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
        int index = int.Parse(id.Substring(0, 1));
        List<BaseCardConfig> cards = GetCardListByType(index);
        
        foreach (var card in cards)
        {
            if (card.id == id)
            {
                return card;
            }
        }
        
        return null;
    }
    
    public List<BaseCardConfig> GetCardListByType(int type)
    {
        switch (type)
        {
            case 1:
                return commonCards;
            case 2:
                return rareCards;
            case 3:
                return epicCards;
            default:
                return new List<BaseCardConfig>();
        }
    }
}