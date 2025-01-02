using System;
using System.Collections;
using System.Collections.Generic;
using KingCyber.Base.UI;
using TMPro;
using UnityEngine;

public class UICombineDialog : UIBaseDialog
{
    [SerializeField] private ItemCardOpen itemCardOpen;
    [SerializeField] private GameObject content;
    [SerializeField] private List<string> saveIdCards;
    [SerializeField] private List<TextMeshProUGUI> tmpNameComponentCards;
    [SerializeField] private TextMeshProUGUI tmpNameMixCards;


    private void OnEnable()
    {
        saveIdCards = GameManager.Instance.GetSavedIDCards();
        Init();
    }

    public void Init()
    {
        foreach (var id in saveIdCards)
        {
            BaseCardConfig card = GameManager.Instance.GameCardConfig.GetCardById(id);
            ItemCardOpen cardItem = Instantiate(itemCardOpen, content.transform);
            
            cardItem.Init(card);
            cardItem.SetOnClick(SelectCard);
        }
    }
    
    public void SelectCard(BaseCardConfig cardData)
    {
        Debug.LogError("SelectCard");
        foreach (var tmpName in tmpNameComponentCards)
        {
            bool isSelectd = !String.IsNullOrEmpty(tmpName.text);
            Debug.LogError("isSelectd: " + isSelectd);

            if (!isSelectd)
            {
                tmpName.text = cardData.cardName;
                return;
            }
        }
    }
    
    public void Combine()
    {
        List<string> mixIds = new List<string>();
        foreach (var tmpName in tmpNameComponentCards)
        {
            mixIds.Add(tmpName.text);
        }
        
        tmpNameMixCards.text = string.Join("_", mixIds);
    }
}
