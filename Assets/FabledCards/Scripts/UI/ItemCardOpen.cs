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
        tmpName.text = cardConfig.id;
        imgAvatar.sprite = Resources.Load<Sprite>($"Cards/{cardConfig.id}"); //cardConfig.avatar;
        
        imgAvatar.gameObject.SetActive(true);
    }
    
    private void SetRarity()
    {
        if (cardConfig == null || cardConfig.rarity > imgRaritys.Count)
        {
            Debug.LogError("Card config is null or rarity is invalid");
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
    
    public void OnClick()
    {
        onClickAction?.Invoke(cardConfig);
    }

    public void ResetCard()
    {
        for (int i = 0; i < imgRaritys.Count; i++)
        {
            imgRaritys[i].gameObject.SetActive(false);
        }
        
        tmpName.text = "";
        imgAvatar.gameObject.SetActive(false);
        
    }
}
