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
    
    private string idCard;
    public string IdCard => idCard;

    private bool isSelected = false;
    public bool IsSelected => isSelected;

    private void OnEnable()
    {
        ResetComponent();
    }

    public void SetSelected(BaseCardConfig card)
    {
        isSelected = true;

        idCard = card.id;
        cardAvatar.sprite = card.avatar;
        cardName.text = card.cardName;
    }
    
    public void ResetComponent()
    {
        isSelected = false;
        
        idCard = "";
        cardAvatar.sprite = null;
        cardName.text = "";
    }
}
