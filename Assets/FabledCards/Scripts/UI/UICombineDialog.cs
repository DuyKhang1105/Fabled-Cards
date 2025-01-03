using System;
using System.Collections;
using System.Collections.Generic;
using KingCyber.Base.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UICombineDialog : UIBaseDialog
{
    [SerializeField] private ItemCardOpen itemCardOpen;
    [SerializeField] private GameObject content;
    [SerializeField] private TextMeshProUGUI tmpIdMixCard;
    [SerializeField] private GameObject buttonCombine;
    [SerializeField] private GameObject buttonReset;
    
    [SerializeField] private List<string> saveIdCards;
    [SerializeField] private List<ItemCardComponent> cardComponents;

    private void OnEnable()
    {
        saveIdCards = GameManager.Instance.GetSavedIDCards();
        Init();
    }

    public void Init()
    {
        CheckReset(false);
        SpawnSaveCard();
    }
    
    private void SpawnSaveCard()
    {
        foreach (var id in saveIdCards)
        {
            BaseCardConfig card = GameManager.Instance.GameBaseCardConfig.GetCardById(id);
            ItemCardOpen cardItem = Instantiate(itemCardOpen, content.transform);
            
            cardItem.Init(card);
            cardItem.SetOnClick(SelectCard);
        }
    }
    
    const int MAX_COMPONENT = 2;
    public void SelectCard(BaseCardConfig cardData)
    {
        foreach (var component in cardComponents)
        {
            bool isSelectd = component.IsSelected;

            if (!isSelectd)
            {
                component.SetSelected(cardData);
                return;
            }
        }
    }
    
    public void Combine()
    {
        if (!IsSelectedAll())
        {
            Debug.LogError("Please select all cards");
            return;
        }
        
        List<string> mixIds = new List<string>();
        foreach (var card in cardComponents)
        {
            mixIds.Add(card.IdCard);
        }
        
        tmpIdMixCard.text = string.Join("_", mixIds);
        CheckReset(true);
        
        //TODO save mix card and remove base card
    }
    
    private bool IsSelectedAll()
    {
        foreach (var component in cardComponents)
        {
            if (!component.IsSelected)
            {
                return false;
            }
        }

        return true;
    }

    private void CheckReset(bool canReset)
    {
        buttonReset.SetActive(canReset);
        buttonCombine.SetActive(!canReset);
    }
    
    public void ResetCombine()
    {
        foreach (var component in cardComponents)
        {
            component.ResetComponent();
        }
        
        tmpIdMixCard.text = "";
        CheckReset(false);
    }
}
