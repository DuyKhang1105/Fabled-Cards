using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemCardOpen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpName;
    [SerializeField] private TextMeshProUGUI tmpRarity;
    
    public void SetUI(BaseCardConfig card)
    {
        tmpName.text = card.cardName;
        tmpRarity.text = card.rarity.ToString();
    }
}
