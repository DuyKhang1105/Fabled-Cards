using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ItemComponentInCombine : MonoBehaviour
{
    [SerializeField] private ItemCard itemCard;
    
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
    
    public void RegistrationActionResetComponent(UnityAction action)
    {
        actionReSetComponent = action;
    }
    
    public void UnRegistrationActionResetComponent()
    {
        actionReSetComponent = null;
    }

    public void SetUI(BaseCardConfig card)
    {
        idCard = card.id;
        itemCard.Init(card);
    }
    
    public void ResetComponent()
    {
        idCard = "";
        isSelected = false;
        
        itemCard.ResetCard();
        actionReSetComponent?.Invoke();
        actionReSetComponent = null;
    }
}
