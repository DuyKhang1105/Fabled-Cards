using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemCardComponent : MonoBehaviour
{
    [SerializeField] private ItemCardOpen itemCardOpen;
    
    private string idCard;
    public string IdCard => idCard;

    private bool isSelected = false;
    public bool IsSelected => isSelected;
    
    private UnityAction actionReSetComponent;

    private void OnEnable()
    {
        ResetComponent();
    }

    public void SetSelected(BaseCardConfig card)
    {
        isSelected = true;
        SetUI(card);
    }
    
    public void SetActionResetComponent(UnityAction action)
    {
        actionReSetComponent = action;
    }

    public void SetUI(BaseCardConfig card)
    {
        idCard = card.id;
        itemCardOpen.Init(card);
    }
    
    public void ResetComponent()
    {
        idCard = "";
        isSelected = false;
        
        itemCardOpen.ResetCard();
        actionReSetComponent?.Invoke();
    }
}
