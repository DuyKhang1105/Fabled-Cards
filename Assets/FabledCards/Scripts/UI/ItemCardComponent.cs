using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemCardComponent : MonoBehaviour
{
    [SerializeField] private Image cardAvatar;
    [SerializeField] private TextMeshProUGUI cardName;
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
        //itemCardOpen.Init(card);
        SetUI(card);
    }
    
    public void SetActionResetComponent(UnityAction action)
    {
        actionReSetComponent = action;
    }

    public void SetUI(BaseCardConfig card)
    {
        idCard = card.id;
        /*cardAvatar.sprite = card.avatar;
        cardName.text = card.cardName;
        cardAvatar.gameObject.SetActive(true);*/
        itemCardOpen.Init(card);
    }
    
    public void ResetComponent()
    {
        isSelected = false;
        
        /*idCard = "";
        cardAvatar.gameObject.SetActive(false);
        cardName.text = "";*/
        itemCardOpen.ResetCard();
        actionReSetComponent?.Invoke();
    }
}
