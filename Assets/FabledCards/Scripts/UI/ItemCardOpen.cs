using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ItemCardOpen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpName;
    [SerializeField] private TextMeshProUGUI tmpRarity;

    private UnityAction<BaseCardConfig> onClickAction;
    private BaseCardConfig cardConfig;

    public void Init(BaseCardConfig card)
    {
        cardConfig = card;
        SetUI();
    }
    
    public void SetOnClick(UnityAction<BaseCardConfig> action)
    {
        Debug.LogError("SetOnClick");
        onClickAction += action;
    }
    
    public void SetUI()
    {
        tmpName.text = cardConfig.cardName;
        tmpRarity.text = cardConfig.rarity.ToString();
    }
    
    
    //TODO Card Base
    public void OnClick()
    {
        onClickAction?.Invoke(cardConfig);
    }
}
