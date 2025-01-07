using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            if (AreIDsEquivalent(card.id, id))
            {
                return card;
            }
        }
        
        return null;
    }
    
    private bool AreIDsEquivalent(string id1, string id2)
    {
        // Tách ID thành các thành phần dựa trên các ký tự đặc biệt
        var parts1 = id1.Split(new char[] { '&', '_' });
        var parts2 = id2.Split(new char[] { '&', '_' });

        // Sắp xếp các thành phần
        Array.Sort(parts1);
        Array.Sort(parts2);

        bool areEquivalent = parts1.SequenceEqual(parts2);
        return areEquivalent;
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