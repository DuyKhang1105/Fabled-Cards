using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemCardOpen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tmpName;
    [SerializeField] private Image imgAvatar;
    [SerializeField] private List<Image> imgRaritys;
    [SerializeField] private List<Sprite> spriteRates;

    private UnityAction<BaseCardConfig> onClickAction;
    private BaseCardConfig cardConfig;

    public void Init(BaseCardConfig card)
    {
        ResetCard();
        cardConfig = card;
        SetUI();
        SetRarity();
    }
    
    public void SetOnClick(UnityAction<BaseCardConfig> action)
    {
        onClickAction += action;
    }
    
    public void SetUI()
    {
        tmpName.text = cardConfig.cardName;
        imgAvatar.sprite = cardConfig.avatar;
    }
    
    private void SetRarity()
    {
        if (cardConfig == null || cardConfig.rarity > imgRaritys.Count)
        {
            return;
        }
        
        for (int i = 0; i < cardConfig.rarity; i++)
        {
            imgRaritys[i].sprite = GetSpriteRate();
            imgRaritys[i].gameObject.SetActive(true);
        }
    }
    
    private Sprite GetSpriteRate()
    {
        int index = int.Parse(cardConfig.id.Substring(0, 1)) - 1;
        return spriteRates[index];
    }
    
    //TODO Card Base
    public void OnClick()
    {
        onClickAction?.Invoke(cardConfig);
    }

    private void ResetCard()
    {
        for (int i = 0; i < imgRaritys.Count; i++)
        {
            imgRaritys[i].gameObject.SetActive(false);
        }
    }
}
